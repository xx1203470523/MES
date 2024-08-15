using Hymson.Infrastructure.Exceptions;
using Hymson.MES.BackgroundServices.NIO.Dtos;
using Hymson.MES.BackgroundServices.NIO.Dtos.ERP;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Common;
using Hymson.MES.Core.Domain.Plan;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Mavel;
using Hymson.MES.Data.NIO;
using Hymson.MES.Data.Repositories.Common;
using Hymson.MES.Data.Repositories.Common.Query;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Data.Repositories.NioPushSwitch;
using Hymson.MES.Data.Repositories.Plan;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.HttpClients;
using Hymson.MES.HttpClients.Requests.ERP;
using Hymson.MES.HttpClients.Requests.XnebulaWMS;
using Hymson.MES.HttpClients.Responses.NioErp;
using Hymson.MES.HttpClients.Responses.NioWms;
using Hymson.Utils;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.BackgroundServices.NIO.Services.ERP
{
    /// <summary>
    /// ERP相关数据推送
    /// </summary>
    public class ErpDataPushService : BasePushService, IErpDataPushService
    {
        /// <summary>
        /// WMS接口
        /// </summary>
        private IWMSApiClient _iWMSApiClient;

        /// <summary>
        /// 系统配置
        /// </summary>
        private readonly ISysConfigRepository _sysConfigRepository;

        /// <summary>
        /// 工单激活
        /// </summary>
        private readonly IPlanWorkOrderRepository _planWorkOrderRepository;

        /// <summary>
        /// BOM详情
        /// </summary>
        private readonly IProcBomDetailRepository _procBomDetailRepository;

        /// <summary>
        /// 物料
        /// </summary>
        private readonly IProcMaterialRepository _procMaterialRepository;

        /// <summary>
        /// 步骤表数量
        /// </summary>
        private readonly IManuSfcStepRepository _manuSfcStepRepository;

        /// <summary>
        /// ERP接口
        /// </summary>
        private readonly IERPApiClient _eRPApiClient;

        /// <summary>
        /// 末工序
        /// </summary>
        private readonly string PRODUCRE_END = "R01OP150";

        /// <summary>
        /// 构造函数
        /// </summary>
        public ErpDataPushService(INioPushSwitchRepository nioPushSwitchRepository, 
            INioPushRepository nioPushRepository,
            IWMSApiClient wMSApiClient,
            ISysConfigRepository sysConfigRepository,
            IPlanWorkOrderRepository planWorkOrderRepository,
            IProcBomDetailRepository procBomDetailRepository,
            IProcMaterialRepository procMaterialRepository,
            IManuSfcStepRepository manuSfcStepRepository,
            IERPApiClient eRPApiClient)
            : base(nioPushSwitchRepository, nioPushRepository)
        {
            _iWMSApiClient = wMSApiClient;
            _sysConfigRepository = sysConfigRepository;
            _planWorkOrderRepository = planWorkOrderRepository;
            _procBomDetailRepository = procBomDetailRepository;
            _procMaterialRepository = procMaterialRepository;
            _manuSfcStepRepository = manuSfcStepRepository;
            _eRPApiClient = eRPApiClient;
        }

        /// <summary>
        /// NIO合作伙伴精益与库存信息
        /// </summary>
        /// <returns></returns>
        public async Task NioStockInfoAsync()
        {
            var buzScene = BuzSceneEnum.ERP_ProductionCapacity;
            var config = await GetSwitchEntityAsync(buzScene);
            if (config == null) return;

            DateTime now = HymsonClock.Now();
            string nowStr = now.ToString("yyyy-MM-dd 00:00:00");

            List<StockMesNIODto> paramList = new List<StockMesNIODto>();
            //1. 取配置中的两个生产物料
            List<NIOConfigBaseDto> configList = new List<NIOConfigBaseDto>();
            List<string> matCodeList = new List<string>();
            var baseConfigList = await GetBaseConfigAsync();
            foreach(var item in baseConfigList)
            {
                NIOConfigBaseDto curConfig = JsonConvert.DeserializeObject<NIOConfigBaseDto>(item.Value);
                configList.Add(curConfig);

                StockMesNIODto paramModel = new StockMesNIODto() { MaterialCode = curConfig.VendorProductCode };
                paramList.Add(paramModel);
                matCodeList.Add(curConfig.VendorProductCode);
            }
            //1.1 获取站点信息
            var configEntities = await _sysConfigRepository.GetEntitiesAsync(new SysConfigQuery { Type = SysConfigEnum.MainSite });
            if (configEntities == null || !configEntities.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10139));
            }
            long siteId = long.Parse(configEntities.ElementAt(0).Value);

            //1.2 获取物料信息
            ProcMaterialsByCodeQuery matQuery = new ProcMaterialsByCodeQuery();
            matQuery.MaterialCodes = matCodeList;
            matQuery.SiteId = siteId;
            var matList = await _procMaterialRepository.GetByCodesAsync(matQuery);

            //1.3 获取末工序数量
            SfcStepProcedureQuery sfcQuery = new SfcStepProcedureQuery();
            sfcQuery.ProcedureCodeList = new List<string>() { PRODUCRE_END };
            //sfcQuery.EndDate = DateTime.Parse(nowStr);
            //sfcQuery.BeginDate = sfcQuery.EndDate.AddDays(-1);
            sfcQuery.BeginDate = Convert.ToDateTime(nowStr);
            sfcQuery.EndDate = sfcQuery.BeginDate.AddDays(1);
            var prodcutNumList = await _manuSfcStepRepository.GetSfcStepEndOpMavelAsync(sfcQuery);

            //2. 调用WMS接口
            var wmsResult = await _iWMSApiClient.NioStockInfoAsync(paramList);
            if(wmsResult == null || wmsResult.Data == null || wmsResult.Data.Count == 0)
            {
                return;
            }
            List<StockPush> dataList = wmsResult.Data;
            //3. 组装数据
            List<ProductionCapacityDto> dtos = new List<ProductionCapacityDto>();
            foreach(var item in dataList)
            {
                NIOConfigBaseDto ?curBaseConfig = configList.Where(m => m.VendorProductCode == item.MaterialCode).FirstOrDefault();
                if(curBaseConfig == null)
                {
                    continue;
                }

                long downNum = 0;
                var curMaterial = matList.Where(m => m.MaterialCode == item.MaterialCode).FirstOrDefault();
                if (curMaterial != null)
                {
                    var curProductNum = prodcutNumList.Where(m => m.ProductId == curMaterial.Id).FirstOrDefault();
                    if(curProductNum != null)
                    {
                        downNum = curProductNum.Num;
                    }
                }

                ProductionCapacityDto dto = new ProductionCapacityDto();
                dto.PartnerBusiness = item.PartnerBusiness;
                dto.MaterialCode = item.MaterialCode;
                dto.MaterialName = item.MaterialName;
                dto.ProductStockQualified = item.ProductStockQualified;
                dto.ProductStockRejection = item.ProductStockRejection;
                dto.ProductStockUndetermined = item.ProductStockUndetermined;
                dto.ProductBackUpMax = item.ProductBackUpMax;
                dto.ProductBackUpMin = item.ProductBackUpMin;
                dto.ParaConfigUnit = item.ParaConfigUnit;

                dto.WorkingSchedule = curBaseConfig.WorkingSchedule;
                dto.PlannedCapacity = curBaseConfig.PlannedCapacity;
                dto.Efficiency = curBaseConfig.Efficiency;
                dto.Beat = curBaseConfig.Beat;
                dto.DailyProductionPlan = curBaseConfig.Dailyproductionplan;
                dto.BottleneckProcess = curBaseConfig.BottleneckProcess;
                dto.ProductInNum = item.ProductInNum;

                dto.Date = HymsonClock.Now().ToString("yyyy-MM-dd HH:mm:ss");
                dto.DownlineNum = downNum;

                dtos.Add(dto);
            }

            //4. 保存数据至NIO
            await AddToPushQueueAsync(config, buzScene, dtos);
        }

        /// <summary>
        /// 关键下级键
        /// </summary>
        /// <returns></returns>
        public async Task NioKeyItemInfoAsync()
        {
            var buzScene = BuzSceneEnum.ERP_KeySubordinate;
            var config = await GetSwitchEntityAsync(buzScene);
            if (config == null) return;

            List<KeySubordinateDto> dtos = new List<KeySubordinateDto>();

            IEnumerable<ProcBomDetailView> materialList = new List<ProcBomDetailView>();
            //1. 取配置中的两个生产物料
            List<PlanWorkOrderEntity> orderList = new List<PlanWorkOrderEntity>();
            var baseConfigList = await GetBaseConfigAsync();
            foreach (var item in baseConfigList)
            {
                NIOConfigBaseDto curConfig = JsonConvert.DeserializeObject<NIOConfigBaseDto>(item.Value);
                string materialCode = curConfig.VendorProductCode;
                PlanWorkOrderMaterialQuery query = new PlanWorkOrderMaterialQuery();
                query.MaterialCode = materialCode;
                var order =  await _planWorkOrderRepository.GetOrderByMaterialCodeAsync(query);
                if (order == null)
                {
                    continue;
                }
                long bomId = order.ProductBOMId;
                //获取BOM中的物料
                IEnumerable<ProcBomDetailView> bomDetailList = await _procBomDetailRepository.GetListMainAsync(bomId);
                List<StockMesNIODto> paramList = bomDetailList.Select(m => new StockMesNIODto() { MaterialCode = m.MaterialCode }).ToList();
                
                //调用WMS获取信息,WMS只会返回关键下级键
                var wmsResult = await _iWMSApiClient.NioKeyItemInfoAsync(paramList);
                if(wmsResult == null || wmsResult.Data == null || wmsResult.Data.Count == 0)
                {
                    continue;
                }
                List<string> keyMaterialList = wmsResult.Data.Select(m => m.MaterialCode).Distinct().ToList();
                List<ErpMaterialDetail> erpMatList = new List<ErpMaterialDetail>();
                foreach (var keyMaterial in keyMaterialList)
                {
                    erpMatList.Add(new ErpMaterialDetail() { Code = keyMaterial });
                }

                //调用ERP接口，获取物料的供应商和合请购单数量
                MaterialRequest erpQuery = new MaterialRequest();
                erpQuery.cInvCode = erpMatList;
                erpQuery.Voudate = HymsonClock.Now();
                NioErpResponse? erpResult = await _eRPApiClient.MaterailQueryAsync(erpQuery);
                if(erpResult != null)
                {
                    erpResult.InitData(keyMaterialList);
                }

                //组装数据
                foreach(var wmsItem in wmsResult.Data)
                {
                    if(string.IsNullOrEmpty(wmsItem.SubordinateCode) == true)
                    {
                        continue;
                    }

                    KeySubordinateDto dto = new KeySubordinateDto();
                    dto.Date = HymsonClock.Now().ToString("yyyy-MM-dd HH:mm:ss");
                    //产品编码
                    dto.MaterialCode = curConfig.VendorProductCode;
                    dto.MaterialName = curConfig.VendorProductName;
                    //WMS取的BOM物料信息
                    dto.PartnerBusiness = wmsItem.PartnerBusiness;
                    dto.SubordinateStockQualified = wmsItem.SubordinateStockQualified;
                    dto.SubordinateStockRejection = wmsItem.SubordinateStockRejection;
                    dto.SubordinateStockUndetermined = wmsItem.SubordinateStockUndetermined;
                    dto.SubordinateCode = wmsItem.SubordinateCode;
                    dto.SubordinateName = wmsItem.SubordinateName;
                    dto.SubordinateMOQ = wmsItem.SubordinateMOQ;
                    dto.SubordinateLT = wmsItem.SubordinateLT;
                    dto.SubordinateBackUpMax = wmsItem.SubordinateBackUpMax;
                    dto.SubordinateBackUpMin = wmsItem.SubordinateBackUpMin;
                    dto.SubordinateSource = wmsItem.SubordinateSource;
                    dto.ParaConfigUnit = wmsItem.ParaConfigUnit;
                    //ERP物料信息
                    if(erpResult != null)
                    {
                        var curErpMat = erpResult.Data.Where(m => m.MaterialCode == wmsItem.SubordinateCode).FirstOrDefault();
                        if(curErpMat != null)
                        {
                            dto.SubordinatePartner = curErpMat.SupperialName;
                            dto.SubordinateArrivalPlan = 0;
                            dto.SubordinateDemandPlan = curErpMat.Num; //ERP-MES请购单
                        }
                    }
                    else
                    {
                        dto.SubordinatePartner = "ERP没有该信息";
                        dto.SubordinateArrivalPlan = 0;
                        dto.SubordinateDemandPlan = 0m; //ERP-MES请购单
                    }

                    //MES物料信息
                    ProcBomDetailView? curBomDetail = bomDetailList.Where(m => m.MaterialCode == wmsItem.SubordinateCode).FirstOrDefault();
                    if(curBomDetail != null)
                    {
                        dto.SubordinateDosage = curBomDetail.Usages;
                    }              

                    dtos.Add(dto);
                }
            }

            //4. 保存数据至NIO
            await AddToPushQueueAsync(config, buzScene, dtos);
        }

        /// <summary>
        /// 实际交付情况
        /// 时间以当晚推送，推送当天0点到24点
        /// </summary>
        /// <returns></returns>
        public async Task NioActualDeliveryAsync()
        {
            var buzScene = BuzSceneEnum.ERP_ActualDelivery;
            var config = await GetSwitchEntityAsync(buzScene);
            if (config == null) return;

            DateTime date = HymsonClock.Now();
            string dateStr = date.ToString("yyyy-MM-dd");
    
            //1. 取配置中的两个生产物料
            List<NIOConfigBaseDto> configList = new List<NIOConfigBaseDto>();
            var baseConfigList = await GetBaseConfigAsync();
            foreach (var item in baseConfigList)
            {
                NIOConfigBaseDto curConfig = JsonConvert.DeserializeObject<NIOConfigBaseDto>(item.Value);
                configList.Add(curConfig);
            }
            //2. 调用WMS接口
            List<string> materialCodeList = configList.Select(m => m.VendorProductCode).ToList();
            StockMesDataDto query = new StockMesDataDto();
            query.StartTime = Convert.ToDateTime(dateStr);
            query.EndTime = query.StartTime.AddDays(1);
            NioWmsActualDeliveryResponse? wmsResult = await _iWMSApiClient.NioActualDeliveryAsync(query);
            //3. 保存数据至NIO
            //if(wmsResult == null || wmsResult.Data == null || wmsResult.Data.Count == 0)
            //{
            //    return;
            //}
            List<ActualDeliveryDto> dtos = new List<ActualDeliveryDto>();
            //List<string> matList = configList.Select(m => m.VendorProductCode).Distinct().ToList();
            foreach(var item in configList)
            {
                ActualDeliveryDto dto = new ActualDeliveryDto();
                dto.MaterialCode = item.VendorProductCode;
                dto.MaterialName = item.VendorProductName;

                if (wmsResult != null && wmsResult.Data != null && wmsResult.Data.Count != 0)
                {
                    var curItemList = wmsResult.Data.Where(m => m.MaterialCode == item.VendorProductCode).ToList();
                    if (curItemList == null || curItemList.Count == 0)
                    {
                        dto.ShippedQty = 0;
                        dto.ActualDeliveryTime = GetTimestamp(HymsonClock.Now());
                        dtos.Add(dto);

                        continue;
                    }
                    var curItem = curItemList[0];

                    DateTime curDate = HymsonClock.Now();
                    if(curItem.ActualDeliveryDate != null)
                    {
                        curDate = (DateTime)curItem.ActualDeliveryDate;
                    }
                    
                    dto.ShippedQty = curItemList.Sum(m => m.ShippedQty);
                    dto.ActualDeliveryTime = GetTimestamp(curDate);
                }
                else
                {
                    dto.ShippedQty = 0;
                    dto.ActualDeliveryTime = GetTimestamp(HymsonClock.Now());
                }

                dtos.Add(dto);
            }

            await AddToPushQueueAsync(config, buzScene, dtos);
        }

        /// <summary>
        /// 获取基础配置数据
        /// </summary>
        /// <returns></returns>
        private async Task<IEnumerable<SysConfigEntity>> GetBaseConfigAsync()
        {
            //基础数据配置
            SysConfigQuery configQuery = new SysConfigQuery();
            configQuery.Type = SysConfigEnum.NioBaseConfig;
            configQuery.Codes = new List<string>() { "NioRotorConfig", "NioStatorConfig" };
            var baseConfigList = await _sysConfigRepository.GetEntitiesAsync(configQuery);
            if (baseConfigList == null || !baseConfigList.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10139)).WithData("name", "NioRotorConfig&NioStatorConfig");
            }

            return baseConfigList;
        }

        /// <summary>
        /// 获取时间戳
        /// </summary>
        /// <param name="createTime"></param>
        /// <param name="updateTime"></param>
        /// <returns></returns>
        private long GetTimestamp(DateTime date)
        {
            return (long)((DateTime)date - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Local)).TotalSeconds;
        }
    }
}
