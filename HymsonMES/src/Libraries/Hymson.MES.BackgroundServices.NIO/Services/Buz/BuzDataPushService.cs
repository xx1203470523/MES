using Hymson.Infrastructure.Exceptions;
using Hymson.MES.BackgroundServices.NIO.Dtos;
using Hymson.MES.BackgroundServices.NIO.Dtos.Buz;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Constants.Manufacture;
using Hymson.MES.Core.Domain.Common;
using Hymson.MES.Core.Domain.Plan;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Mavel;
using Hymson.MES.Data.Repositories.Common;
using Hymson.MES.Data.Repositories.Common.Query;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Data.Repositories.Parameter;
using Hymson.MES.Data.Repositories.Plan;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Data.Repositories.Process.Query;
using Hymson.Utils;
using Hymson.Utils.Tools;
using Hymson.WaterMark;
using Newtonsoft.Json;

namespace Hymson.MES.BackgroundServices.NIO.Services
{
    /// <summary>
    /// 推送服务（业务数据）
    /// </summary>
    public class BuzDataPushService : BasePushService, IBuzDataPushService
    {
        /// <summary>
        /// 仓储接口（蔚来推送开关）
        /// </summary>
        private readonly INioPushSwitchRepository _nioPushSwitchRepository;

        /// <summary>
        /// 仓储接口（蔚来推送）
        /// </summary>
        private readonly INioPushRepository _nioPushRepository;

        /// <summary>
        /// 系统配置
        /// </summary>
        private readonly ISysConfigRepository _sysConfigRepository;

        /// <summary>
        /// 水位表
        /// </summary>
        private readonly IWaterMarkService _waterMarkService;

        /// <summary>
        /// 产品参数
        /// </summary>
        private readonly IManuProductParameterRepository _manuProductParameterRepository;

        /// <summary>
        /// 标准参数
        /// </summary>
        private readonly IProcParameterRepository _procParameterRepository;

        /// <summary>
        /// 工序
        /// </summary>
        private readonly IProcProcedureRepository _procProcedureRepository;

        /// <summary>
        /// 步骤表数据
        /// </summary>
        private readonly IManuSfcStepRepository _manuSfcStepRepository;

        /// <summary>
        /// 工单
        /// </summary>
        private readonly IPlanWorkOrderRepository _planWorkOrderRepository;

        /// <summary>
        /// 操作员账号
        /// </summary>
        private readonly string NIO_USER_ID = "LMS001";

        /// <summary>
        /// 操作员密码
        /// </summary>
        private readonly string NIO_USER_NAME = "MAVLE";

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="nioPushSwitchRepository"></param>
        public BuzDataPushService(INioPushSwitchRepository nioPushSwitchRepository, INioPushRepository nioPushRepository,
            ISysConfigRepository sysConfigRepository,
            IWaterMarkService waterMarkService,
            IManuProductParameterRepository manuProductParameterRepository,
            IProcParameterRepository procParameterRepository,
            IProcProcedureRepository procProcedureRepository,
            IManuSfcStepRepository manuSfcStepRepository,
            IPlanWorkOrderRepository planWorkOrderRepository)
            : base(nioPushSwitchRepository, nioPushRepository)
        {
            _nioPushSwitchRepository = nioPushSwitchRepository;
            _nioPushRepository = nioPushRepository;
            _sysConfigRepository = sysConfigRepository;
            _waterMarkService = waterMarkService;
            _manuProductParameterRepository = manuProductParameterRepository;
            _procParameterRepository = procParameterRepository;
            _procProcedureRepository = procProcedureRepository;
            _manuSfcStepRepository = manuSfcStepRepository;
            _planWorkOrderRepository = planWorkOrderRepository;
        }

        /// <summary>
        /// 业务数据（控制项）
        /// </summary>
        /// <returns></returns>
        public async Task CollectionAsync()
        {
            var buzScene = BuzSceneEnum.Buz_Collection;
            var config = await GetSwitchEntityAsync(buzScene);
            if (config == null) return;

            //站点配置
            var configEntities = await _sysConfigRepository.GetEntitiesAsync(new SysConfigQuery { Type = SysConfigEnum.MainSite });
            if (configEntities == null || !configEntities.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10139));
            }
            long siteId = long.Parse(configEntities.ElementAt(0).Value);
            //获取基础配置
            var baseConfigList = await GetBaseConfig();
            //获取当前水位
            var startWaterMarkId = await _waterMarkService.GetWaterMarkAsync(BusinessKey.NioParam);
            //获取参数表数据
            EntityByWaterMarkQuery waterQuery = new EntityByWaterMarkQuery() { Rows = 100, StartWaterMarkId = startWaterMarkId };
            var paramList = await _manuProductParameterRepository.GetManuParamMavelAsync(waterQuery);
            if(paramList == null || paramList.Any() == false) return;
            //获取标准参数，用于获取根据ID获取名称（参数表在其他库）
            ProcParameterQuery paramQuery = new ProcParameterQuery() { SiteId = siteId };
            var baseParamList = await _procParameterRepository.GetProcParameterEntitiesAsync(paramQuery);
            if(baseParamList == null || baseParamList.Any() == false) return;
            //获取工序
            ProcProcedureQuery procedureQuery = new ProcProcedureQuery() { SiteId = siteId };
            var procedureList = await _procProcedureRepository.GetEntitiesAsync(procedureQuery);
            if (procedureList == null || procedureList.Any() == false) return;

            var dtos = new List<CollectionDto>();
            // TODO: 替换为实际数据
            foreach(var item in paramList)
            {
                //标准参数
                var curBaseParam = baseParamList.Where(m => m.Id == item.ParameterId).FirstOrDefault();
                if(curBaseParam == null)
                {
                    continue;
                }
                string paramCode = curBaseParam.ParameterCode;
                char firstChar = paramCode[0];
                if (firstChar != 'R' && firstChar != 'S')
                {
                    continue;
                }
                //工序
                string procedure = "未知";
                var curProcedure = procedureList.Where(m => m.Id == item.ProcedureId).FirstOrDefault();
                if(curProcedure != null)
                {
                    procedure = curProcedure.Code;
                }

                NIOConfigBaseDto curConfig = new NIOConfigBaseDto();
                if (firstChar == 'R')
                {
                    curConfig = JsonConvert.DeserializeObject<NIOConfigBaseDto>(GetRotorConfig(baseConfigList));
                }
                else
                {
                    curConfig = JsonConvert.DeserializeObject<NIOConfigBaseDto>(GetStatorConfig(baseConfigList));
                }

                CollectionDto model = new CollectionDto();
                model.PlantId = curConfig.PlantId;
                model.WorkshopId = curConfig.WorkshopId;
                model.ProductionLineId = curConfig.ProductionLineId;
                model.StationId = procedure;
                model.DeviceId = "无";
                model.VendorFieldCode = paramCode;
                model.DecimalValue = 0;
                model.StringValue = item.ParameterValue;
                model.BooleanValue = false;
                model.ProcessType = "final";
                model.NioProductCode = curConfig.NioProductCode;
                model.NioProductNum = curConfig.NioProductCode;
                model.NioProductName = curConfig.NioProductName;
                model.NioModel = "ES8";
                model.VendorProductNum = curConfig.VendorProductCode;
                model.VendorProductName = curConfig.VendorProductName;
                model.VendorProductSn = item.SFC;
                model.VendorProductTempSn = item.SFC;
                model.VendorProductCode = item.SFC;
                model.VendorProductBatch = item.SFC;
                model.OperatorAccount = item.CreatedBy;
                model.InputTime = GetTimestamp(HymsonClock.Now());
                model.OutputTime = model.InputTime;
                model.StationStatus = "passed";
                model.VendorStationStatus = model.StationStatus;
                model.VendorValueStatus = model.StationStatus;
                model.Debug = true;
                model.UpdateTime = GetTimestamp(HymsonClock.Now());

                dtos.Add(model);
            }

            //MES数据入库
            using var trans = TransactionHelper.GetTransactionScope();

            await AddToPushQueueAsync(config, buzScene, dtos);
            await _waterMarkService.RecordWaterMarkAsync(BusinessKey.NioParam, paramList.Max(x => x.Id));

            trans.Complete();
        }

        /// <summary>
        /// 业务数据（生产业务）
        /// </summary>
        /// <returns></returns>
        public async Task ProductionAsync()
        {
            var buzScene = BuzSceneEnum.Buz_Production;
            var config = await GetSwitchEntityAsync(buzScene);
            if (config == null) return;

            //站点配置
            var configEntities = await _sysConfigRepository.GetEntitiesAsync(new SysConfigQuery { Type = SysConfigEnum.MainSite });
            if (configEntities == null || !configEntities.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10139));
            }
            long siteId = long.Parse(configEntities.ElementAt(0).Value);
            //获取基础配置
            var baseConfigList = await GetBaseConfig();
            //获取当前水位
            var startWaterMarkId = await _waterMarkService.GetWaterMarkAsync(BusinessKey.NioManuData);
            //获取步骤数据
            EntityByWaterSiteIdQuery stepQuery = new EntityByWaterSiteIdQuery() { Rows = 100, SiteId = siteId, StartWaterMarkId = startWaterMarkId };
            var stepList = await _manuSfcStepRepository.GetSfcStepMavelAsync(stepQuery);
            //工单
            List<long> orderIdList= stepList.Where(m => m.WorkOrderId != 0).Select(m => m.WorkOrderId).Distinct().ToList();
            IEnumerable<PlanWorkOrderEntity> orderList = await _planWorkOrderRepository.GetByIdsAsync(orderIdList);

            var dtos = new List<ProductionDto> { };
            foreach(var item in stepList)
            {
                if(string.IsNullOrEmpty(item.Remark) == true)
                {
                    continue;
                }
                string procedureCode = item.Remark;
                NIOConfigBaseDto curConfig = new NIOConfigBaseDto();
                if (procedureCode.Length == 5)
                {
                    curConfig = JsonConvert.DeserializeObject<NIOConfigBaseDto>(GetRotorConfig(baseConfigList));
                }
                else
                {
                    curConfig = JsonConvert.DeserializeObject<NIOConfigBaseDto>(GetStatorConfig(baseConfigList));
                }
                //工单
                string curOrderCode = "0";
                var curOrder = orderList.Where(m => m.Id == item.WorkOrderId).FirstOrDefault();
                if(curOrder != null)
                {
                    curOrderCode = curOrder.OrderCode;
                }

                ProductionDto model = new ProductionDto();
                model.PlantId = curConfig.PlantId;
                model.WorkshopId = curConfig.WorkshopId;
                model.ProductionLineId = curConfig.ProductionLineId;
                model.StationId = procedureCode;
                model.VendorProductNum = curConfig.VendorProductCode;
                model.VendorProductName = curConfig.VendorProductName;
                model.VendorProductSn = item.SFC;
                model.VendorProductTempSn = item.SFC;
                model.VendorProductCode = curConfig.VendorProductCode;
                model.VendorProductBatch = curConfig.VendorProductCode;
                model.WorkorderId = curOrderCode;
                model.OperatorId = NIO_USER_ID;
                model.OperatorName = NIO_USER_NAME;
                model.InputTime = GetTimestamp(item.CreatedOn);
                model.OutputTime = model.InputTime;
                model.DeviceDeterminedStatus = item.ScrapQty > 0;
                model.DeterminedStatus = item.ScrapQty > 0;
                model.ManualDeterminedStatus = item.ScrapQty > 0;
                model.UpdateTime = GetTimestamp(HymsonClock.Now());

                dtos.Add(model);
            }

            //MES数据入库
            using var trans = TransactionHelper.GetTransactionScope();

            await AddToPushQueueAsync(config, buzScene, dtos);
            await _waterMarkService.RecordWaterMarkAsync(BusinessKey.NioParam, stepList.Max(x => x.Id));

            trans.Complete();
        }

        /// <summary>
        /// 业务数据（材料清单）
        /// </summary>
        /// <returns></returns>
        public async Task MaterialAsync()
        {
            var buzScene = BuzSceneEnum.Buz_Material;
            var config = await GetSwitchEntityAsync(buzScene);
            if (config == null) return;

            // TODO: 替换为实际数据
            var dtos = new List<MaterialDto> { };
            await AddToPushQueueAsync(config, buzScene, dtos);
        }

        /// <summary>
        /// 业务数据（产品一次合格率）
        /// </summary>
        /// <returns></returns>
        public async Task PassrateProductAsync()
        {
            var buzScene = BuzSceneEnum.Buz_PassrateProduct;
            var config = await GetSwitchEntityAsync(buzScene);
            if (config == null) return;

            // TODO: 替换为实际数据
            var dtos = new List<PassrateProductDto> { };
            await AddToPushQueueAsync(config, buzScene, dtos);
        }

        /// <summary>
        /// 业务数据（工位一次合格率）
        /// </summary>
        /// <returns></returns>
        public async Task PassrateStationAsync()
        {
            var buzScene = BuzSceneEnum.Buz_PassrateStation;
            var config = await GetSwitchEntityAsync(buzScene);
            if (config == null) return;

            // TODO: 替换为实际数据
            var dtos = new List<PassrateStationDto> { };
            await AddToPushQueueAsync(config, buzScene, dtos);
        }

        /// <summary>
        /// 业务数据（环境业务）
        /// </summary>
        /// <returns></returns>
        public async Task DataEnvAsync()
        {
            var buzScene = BuzSceneEnum.Buz_DataEnv;
            var config = await GetSwitchEntityAsync(buzScene);
            if (config == null) return;

            // TODO: 替换为实际数据
            var dtos = new List<DataEnvDto> { };
            await AddToPushQueueAsync(config, buzScene, dtos);
        }

        /// <summary>
        /// 业务数据（缺陷业务）
        /// </summary>
        /// <returns></returns>
        public async Task IssueAsync()
        {
            var buzScene = BuzSceneEnum.Buz_Issue;
            var config = await GetSwitchEntityAsync(buzScene);
            if (config == null) return;

            // TODO: 替换为实际数据
            var dtos = new List<IssueDto> { };
            await AddToPushQueueAsync(config, buzScene, dtos);
        }

        /// <summary>
        /// 业务数据（工单业务）
        /// </summary>
        /// <returns></returns>
        public async Task WorkOrderAsync()
        {
            var buzScene = BuzSceneEnum.Buz_WorkOrder;
            var config = await GetSwitchEntityAsync(buzScene);
            if (config == null) return;

            // TODO: 替换为实际数据
            var dtos = new List<WorkOrderDto> { };
            await AddToPushQueueAsync(config, buzScene, dtos);
        }

        /// <summary>
        /// 业务数据（通用业务）
        /// </summary>
        /// <returns></returns>
        public async Task CommonAsync()
        {
            var switchEntity = await GetSwitchEntityAsync(BuzSceneEnum.Buz_Common);
            if (switchEntity == null) return;

            // TODO: 替换为实际数据
            var dtos = new List<CommonDto> { };
            await switchEntity.ExecuteAsync(dtos);
        }

        /// <summary>
        /// 业务数据（附件）
        /// </summary>
        /// <returns></returns>
        public async Task AttachmentAsync()
        {
            var buzScene = BuzSceneEnum.Buz_Attachment;
            var config = await GetSwitchEntityAsync(buzScene);
            if (config == null) return;

            // TODO: 替换为实际数据
            var dtos = new List<AttachmentDto> { };
            await AddToPushQueueAsync(config, buzScene, dtos);
        }


        /// <summary>
        /// 获取基础配置数据
        /// </summary>
        /// <returns></returns>
        private async Task<IEnumerable<SysConfigEntity>> GetBaseConfig()
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
        /// 获取转子线
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        private string GetRotorConfig(IEnumerable<SysConfigEntity> list)
        {
            var config = list.Where(m => m.Code == "NioRotorConfig").FirstOrDefault();
            if (config != null && string.IsNullOrEmpty(config.Value) == false)
            {
                return config.Value;
            }
            return string.Empty;
        }

        /// <summary>
        /// 获取定子线配置
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        private string GetStatorConfig(IEnumerable<SysConfigEntity> list)
        {
            var config = list.Where(m => m.Code == "NioStatorConfig").FirstOrDefault();
            if (config != null && string.IsNullOrEmpty(config.Value) == false)
            {
                return config.Value;
            }
            return string.Empty;
        }

        /// <summary>
        /// 获取时间戳
        /// </summary>
        /// <param name="createTime"></param>
        /// <param name="updateTime"></param>
        /// <returns></returns>
        private long GetTimestamp(DateTime date)
        {
            return (long)((DateTime)date - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;
        }
    }
}
