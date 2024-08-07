using Hymson.Infrastructure.Exceptions;
using Hymson.MES.BackgroundServices.NIO.Dtos;
using Hymson.MES.BackgroundServices.NIO.Dtos.Buz;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Constants.Manufacture;
using Hymson.MES.Core.Domain.Common;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Core.Domain.NioPushCollection;
using Hymson.MES.Core.Domain.Plan;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Mavel;
using Hymson.MES.Data.NIO;
using Hymson.MES.Data.Repositories.Common;
using Hymson.MES.Data.Repositories.Common.Query;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Data.Repositories.Manufacture.Query;
using Hymson.MES.Data.Repositories.Mavel.Rotor;
using Hymson.MES.Data.Repositories.Mavel.Rotor.ManuRotorSfc.Query;
using Hymson.MES.Data.Repositories.NioPushCollection;
using Hymson.MES.Data.Repositories.NioPushSwitch;
using Hymson.MES.Data.Repositories.Parameter;
using Hymson.MES.Data.Repositories.Plan;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Data.Repositories.Process.Query;
using Hymson.Snowflake;
using Hymson.Utils;
using Hymson.Utils.Tools;
using Hymson.WaterMark;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Security.Policy;

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
        /// 物料
        /// </summary>
        private readonly IProcMaterialRepository _procMaterialRepository;

        /// <summary>
        /// 转子线主条码
        /// </summary>
        private readonly IManuRotorSfcRepository _manuRotorSfcRepository;

        /// <summary>
        /// 追溯条码
        /// </summary>
        private readonly IManuSfcCirculationRepository _manuSfcCirculationRepository;

        /// <summary>
        /// 领料单条码明细
        /// </summary>
        private readonly IManuRequistionOrderReceiveRepository _manuRequistionOrderReceiveRepository;

        /// <summary>
        /// 仓储接口（工单完工入库明细）
        /// </summary>
        private readonly IManuProductReceiptOrderDetailRepository _manuProductReceiptOrderDetailRepository;

        /// <summary>
        /// NIO推送参数
        /// </summary>
        private readonly INioPushCollectionRepository _nioPushCollectionRepository;

        /// <summary>
        /// 日志
        /// </summary>
        private readonly ILogger<IBuzDataPushService> _logger;

        /// <summary>
        /// 操作员账号
        /// </summary>
        private readonly string NIO_USER_ID = "LMS001";

        /// <summary>
        /// 操作员密码
        /// </summary>
        private readonly string NIO_USER_NAME = "MAVLE";

        /// <summary>
        /// NIO调试字段
        /// </summary>
        private readonly bool NIO_DEBUG = false;

        /// <summary>
        /// 水位数据行数
        /// 这边是每天推上次的，所以数量应该是所有的
        /// </summary>
        private readonly int WATER_ROWS = 100;

        /// <summary>
        /// 取所有行数
        /// </summary>
        private readonly int WATER_ALL_ROWS = 0;

        /// <summary>
        /// 转子线工序长度
        /// </summary>
        private readonly int ROTOR_OP_LEN = 5;

        /// <summary>
        /// 转子线
        /// </summary>
        private readonly string NIO_ROTOR_CONFIG = "NioRotorConfig";

        /// <summary>
        /// 定子线
        /// </summary>
        private readonly string NIO_STATOR_CONFIG = "NioStatorConfig";

        /// <summary>
        /// 转子线工序/参数首字母
        /// </summary>
        private readonly char ROTOR_CHAR = 'R';

        /// <summary>
        /// 定子线工序/参数首字母
        /// </summary>
        private readonly char STATOR_CHAR = 'S';

        /// <summary>
        /// 基础配置
        /// </summary>
        private List<string> BASE_CONFIG_LIST { get; set; } = new List<string>();

        /// <summary>
        /// 转子线推送包含NIO码的工序
        /// </summary>
        private readonly List<string> ROTOR_NIOSN_OP = new List<string>() { "ROP130", "ROP140", "ROP150" };

        /// <summary>
        /// 末工序
        /// </summary>
        private readonly string PRODUCRE_END = "R01OP150";

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
            IPlanWorkOrderRepository planWorkOrderRepository,
            IProcMaterialRepository procMaterialRepository,
            IManuRotorSfcRepository manuRotorSfcRepository,
            IManuSfcCirculationRepository manuSfcCirculationRepository,
            IManuRequistionOrderReceiveRepository manuRequistionOrderReceiveRepository,
            IManuProductReceiptOrderDetailRepository manuProductReceiptOrderDetailRepository,
            INioPushCollectionRepository nioPushCollectionRepository,
            ILogger<IBuzDataPushService> logger)
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
            _procMaterialRepository = procMaterialRepository;
            _manuRotorSfcRepository = manuRotorSfcRepository;
            _manuSfcCirculationRepository = manuSfcCirculationRepository;
            _manuRequistionOrderReceiveRepository = manuRequistionOrderReceiveRepository;
            _manuProductReceiptOrderDetailRepository = manuProductReceiptOrderDetailRepository;
            _nioPushCollectionRepository = nioPushCollectionRepository;
            _logger = logger;

            BASE_CONFIG_LIST = new List<string>() { NIO_ROTOR_CONFIG, NIO_STATOR_CONFIG };
        }

        /// <summary>
        /// 业务数据（控制项）
        /// </summary>
        /// <returns></returns>
        public async Task CollectionAsync()
        {
            _logger.LogInformation($"业务数据（控制项）CollectionAsync {HymsonClock.Now().ToString("yyyyMMdd HH:mm:ss")}");

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
            var baseConfigList = await GetBaseConfigAsync();
            //获取当前水位
            var startWaterMarkId = await _waterMarkService.GetWaterMarkAsync(BusinessKey.NioParam);
            //获取参数表数据
            EntityByWaterMarkQuery waterQuery = new EntityByWaterMarkQuery() { Rows = 50, StartWaterMarkId = startWaterMarkId };
            var paramList = await _manuProductParameterRepository.GetManuParamMavelAsync(waterQuery);
            if (paramList == null || paramList.Any() == false) return;
            //获取标准参数，用于获取根据ID获取名称（参数表在其他库）
            ProcParameterQuery paramQuery = new ProcParameterQuery() { SiteId = siteId };
            var baseParamList = await _procParameterRepository.GetProcParameterEntitiesAsync(paramQuery);
            if (baseParamList == null || baseParamList.Any() == false) return;
            //获取工序
            ProcProcedureQuery procedureQuery = new ProcProcedureQuery() { SiteId = siteId };
            var procedureList = await _procProcedureRepository.GetEntitiesAsync(procedureQuery);
            if (procedureList == null || procedureList.Any() == false) return;
            //成品码信息
            ZSfcQuery zSfcQuery = new ZSfcQuery();
            zSfcQuery.SiteId = siteId;
            zSfcQuery.SfcList = paramList.Select(m => m.SFC).Distinct().ToList();
            var nioSfcList = await _manuRotorSfcRepository.GetListByZSfcsAsync(zSfcQuery);

            var dtos = new List<CollectionDto>();
            List<NioPushCollectionEntity> nioList = new List<NioPushCollectionEntity>();
            // TODO: 替换为实际数据
            foreach (var item in paramList)
            {
                //标准参数
                var curBaseParam = baseParamList.Where(m => m.Id == item.ParameterId).FirstOrDefault();
                if (curBaseParam == null)
                {
                    continue;
                }
                string paramCode = curBaseParam.ParameterCode;
                char firstChar = paramCode[0];
                if (firstChar != ROTOR_CHAR && firstChar != STATOR_CHAR)
                {
                    continue;
                }
                //工序
                string procedure = "未知";
                var curProcedure = procedureList.Where(m => m.Id == item.ProcedureId).FirstOrDefault();
                if (curProcedure != null)
                {
                    procedure = curProcedure.Code;
                }
                //总成码,指定工序才有总成码
                string nioSfc = item.SFC;
                if (ROTOR_NIOSN_OP.Contains(procedure) == true && nioSfcList != null)
                {
                    var curNio = nioSfcList.Where(m => m.ZSfc == item.SFC).FirstOrDefault();
                    if (curNio != null)
                    {
                        nioSfc = curNio.Sfc;
                    }
                }

                CollectionDto model = new CollectionDto();
                NIOConfigBaseDto curConfig = new NIOConfigBaseDto();
                if (firstChar == ROTOR_CHAR)
                {
                    curConfig = JsonConvert.DeserializeObject<NIOConfigBaseDto>(GetRotorConfig(baseConfigList));
                    model.DataType = 1;
                }
                else
                {
                    curConfig = JsonConvert.DeserializeObject<NIOConfigBaseDto>(GetStatorConfig(baseConfigList));
                    model.DataType = 2;
                }

                model.PlantId = curConfig.PlantId;
                model.WorkshopId = curConfig.WorkshopId;
                model.ProductionLineId = curConfig.ProductionLineId;
                model.StationId = procedure;
                //model.DeviceId = "无";
                model.VendorFieldCode = paramCode;
                if(curBaseParam.DataType == DataTypeEnum.Numeric) //数字类型
                {
                    decimal tmpDecValue = 0;
                    if (decimal.TryParse(item.ParameterValue, out tmpDecValue) == true) //能转成decimal
                    {
                        model.DecimalValue = tmpDecValue;
                        model.StringValue = "";
                    }
                    else
                    {
                        model.DecimalValue = 0;
                        model.StringValue = item.ParameterValue;
                    }
                }
                else
                {
                    model.DecimalValue = 0;
                    model.StringValue = item.ParameterValue;
                }
                model.BooleanValue = null;
                model.ProcessType = "final";
                //model.NioProductCode = curConfig.NioProductCode;
                model.NioProductNum = curConfig.NioProductCode;
                model.NioProductName = curConfig.NioProductName;
                //model.NioModel = "ES8";
                model.VendorProductNum = curConfig.VendorProductCode;
                model.VendorProductName = curConfig.VendorProductName;
                model.VendorProductSn = nioSfc;
                model.VendorProductTempSn = item.SFC;
                //model.VendorProductCode = item.SFC;
                //model.VendorProductBatch = item.SFC;
                model.OperatorAccount = item.CreatedBy;
                model.InputTime = GetTimestamp(HymsonClock.Now());
                model.OutputTime = model.InputTime;
                model.StationStatus = "passed";
                model.VendorStationStatus = model.StationStatus;
                model.VendorValueStatus = model.StationStatus;
                //model.Debug = NIO_DEBUG;
                model.UpdateTime = GetTimestamp(HymsonClock.Now());

                dtos.Add(model);

                var tmpStr = JsonConvert.SerializeObject(model);
                NioPushCollectionEntity nioModel = JsonConvert.DeserializeObject<NioPushCollectionEntity>(tmpStr);
                nioModel.Id = IdGenProvider.Instance.CreateId();
                nioModel.CreatedOn = HymsonClock.Now();
                nioModel.UpdatedOn = nioModel.CreatedOn;
                nioModel.CreatedBy = NIO_USER_ID;
                nioModel.UpdatedBy = NIO_USER_ID;
                nioList.Add(nioModel);
            }
            if (dtos == null || dtos.Count == 0)
            {
                return;
            }

            long nioId = IdGenProvider.Instance.CreateId();
            nioList.ForEach(m => m.NioPushId = nioId);

            //MES数据入库
            using var trans = TransactionHelper.GetTransactionScope();

            await AddToPushQueueAsync(config, buzScene, dtos, nioId);
            await _nioPushCollectionRepository.InsertRangeAsync(nioList);
            await _waterMarkService.RecordWaterMarkAsync(BusinessKey.NioParam, paramList.Max(x => x.Id));

            trans.Complete();
        }

        /// <summary>
        /// 业务数据（生产业务）
        /// </summary>
        /// <returns></returns>
        public async Task ProductionAsync()
        {
            _logger.LogInformation($"业务数据（生产业务）ProductionAsync {HymsonClock.Now().ToString("yyyyMMdd HH:mm:ss")}");

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
            var baseConfigList = await GetBaseConfigAsync();
            //获取当前水位
            var startWaterMarkId = await _waterMarkService.GetWaterMarkAsync(BusinessKey.NioManuData);
            //获取步骤数据
            EntityByWaterSiteIdQuery stepQuery = new EntityByWaterSiteIdQuery() { Rows = WATER_ROWS, SiteId = siteId, StartWaterMarkId = startWaterMarkId };
            var stepList = await _manuSfcStepRepository.GetSfcStepMavelAsync(stepQuery);
            if(stepList == null || stepList.Count() == 0)
            {
                return;
            }
            //工单
            List<long> orderIdList = stepList.Where(m => m.WorkOrderId != 0).Select(m => m.WorkOrderId).Distinct().ToList();
            IEnumerable<PlanWorkOrderEntity> orderList = await _planWorkOrderRepository.GetByIdsAsync(orderIdList);
            //获取批次信息
            List<string> sfcList = stepList.Select(m => m.SFC).Distinct().ToList();
            List<SfcBatchDto> sfcBatchList = await GetSfcBatchListAsync(siteId, sfcList);
            //成品码信息
            ZSfcQuery zSfcQuery = new ZSfcQuery();
            zSfcQuery.SiteId = siteId;
            zSfcQuery.SfcList = sfcList;
            var nioSfcList = await _manuRotorSfcRepository.GetListByZSfcsAsync(zSfcQuery);

            var dtos = new List<ProductionDto> { };
            foreach (var item in stepList)
            {
                if (string.IsNullOrEmpty(item.Remark) == true)
                {
                    continue;
                }
                string procedureCode = item.Remark;
                NIOConfigBaseDto curConfig = new NIOConfigBaseDto();
                if (procedureCode[0] == ROTOR_CHAR)
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
                if (curOrder != null)
                {
                    curOrderCode = curOrder.OrderCode;
                }
                //条码批次
                string sfcBatch = string.Empty;
                var sfcBatchModel = sfcBatchList.Where(m => m.Sfc == item.SFC).FirstOrDefault();
                if (sfcBatchModel != null)
                {
                    sfcBatch = sfcBatchModel.Batch;
                }
                //总成码,指定工序才有总成码
                string nioSfc = string.Empty;
                if (ROTOR_NIOSN_OP.Contains(procedureCode) == true && nioSfcList != null)
                {
                    var curNio = nioSfcList.Where(m => m.ZSfc == item.SFC).FirstOrDefault();
                    if (curNio != null)
                    {
                        nioSfc = curNio.Sfc;
                    }
                }

                ProductionDto model = new ProductionDto();
                model.PlantId = curConfig.PlantId;
                model.WorkshopId = curConfig.WorkshopId;
                model.ProductionLineId = curConfig.ProductionLineId;
                model.StationId = procedureCode;
                model.VendorProductNum = curConfig.VendorProductCode;
                model.VendorProductName = curConfig.VendorProductName;
                model.VendorProductSn = string.IsNullOrEmpty(nioSfc) ? item.SFC : nioSfc;
                model.VendorProductTempSn = item.SFC;
                //model.VendorProductCode = curConfig.VendorProductCode;
                //model.VendorProductBatch = sfcBatch;
                model.WorkorderId = curOrderCode;
                model.OperatorId = NIO_USER_ID;
                model.OperatorName = NIO_USER_NAME;
                model.InputTime = GetTimestamp(item.CreatedOn);
                model.OutputTime = model.InputTime;
                //model.DeviceDeterminedStatus = item.ScrapQty > 0;
                model.DeterminedStatus = item.ScrapQty > 0;
                //model.ManualDeterminedStatus = item.ScrapQty > 0;
                model.UpdateTime = GetTimestamp(HymsonClock.Now());

                dtos.Add(model);
            }

            //MES数据入库
            using var trans = TransactionHelper.GetTransactionScope();

            await AddToPushQueueAsync(config, buzScene, dtos);
            await _waterMarkService.RecordWaterMarkAsync(BusinessKey.NioManuData, stepList.Max(x => x.Id));

            trans.Complete();
        }

        /// <summary>
        /// 业务数据（材料清单）
        /// 只适用于转子线
        /// </summary>
        /// <returns></returns>
        public async Task MaterialAsync()
        {
            _logger.LogInformation($"业务数据（材料清单）MaterialAsync {HymsonClock.Now().ToString("yyyyMMdd HH:mm:ss")}");

            var buzScene = BuzSceneEnum.Buz_Material;
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
            var baseConfigList = await GetBaseConfigAsync();
            //获取当前水位
            var startWaterMarkId = await _waterMarkService.GetWaterMarkAsync(BusinessKey.NioMaterial);
            DateTime startWaterMarkTime = DateTime.Now;
            if (startWaterMarkId != 0)
            {
                startWaterMarkTime = UnixTimestampMillisToDateTime(startWaterMarkId);
            }
            else
            {
                startWaterMarkTime = DateTime.Parse("2024-08-01 01:01:01");
            }
            //获取已经走完的追溯记录
            EntityByWaterMarkTimeQuery rotorQuery = new EntityByWaterMarkTimeQuery();
            rotorQuery.StartWaterMarkTime = startWaterMarkTime;
            rotorQuery.Rows = WATER_ROWS;
            var rotorSfcList = await _manuRotorSfcRepository.GetListAsync(rotorQuery);
            if (rotorSfcList == null || rotorSfcList.Count() == 0)
            {
                return;
            }
            List<string> nioSfcList = rotorSfcList.Select(m => m.Sfc).ToList();

            //获取铁芯码和轴码
            List<string> txSfcList = rotorSfcList.Select(m => m.TxSfc).ToList();
            List<string> zSfcList = rotorSfcList.Select(m => m.ZSfc).ToList();
            List<string> allSfcList = new List<string>();
            allSfcList.AddRange(txSfcList);
            allSfcList.AddRange(zSfcList);
            //查询铁芯码和轴码所有的上料记录
            ManuSfcCirculationBySfcsQuery cirQuery = new ManuSfcCirculationBySfcsQuery();
            cirQuery.SiteId = siteId;
            cirQuery.Sfc = allSfcList;
            IEnumerable<ManuSfcCirculationEntity> cirSfcList = await _manuSfcCirculationRepository.GetSfcMoudulesAsync(cirQuery);
            if (cirSfcList == null || cirSfcList.Count() == 0)
            {
                return;
            }
            List<string> sfcMatList = rotorSfcList.Select(m => m.SfcMaterialCode).Distinct().ToList();
            List<string> txMatList = rotorSfcList.Select(m => m.TxSfcMaterialCode).Distinct().ToList();
            List<string> zMatList = rotorSfcList.Select(m => m.ZSfcMaterialCode).Distinct().ToList();
            List<string> cirMatList = new List<string>();
            cirMatList.AddRange(sfcMatList);
            cirMatList.AddRange(txMatList);
            cirMatList.AddRange(cirMatList);
            cirMatList = cirMatList.Where(m => string.IsNullOrEmpty(m) == false).Distinct().ToList();

            //查询铁芯码，轴码，总成码物料的数据
            ProcMaterialsByCodeQuery matQuery = new ProcMaterialsByCodeQuery();
            matQuery.SiteId = siteId;
            matQuery.MaterialCodes = cirMatList;
            var materialList = await _procMaterialRepository.GetByCodesAsync(matQuery);
            //查询上料物料信息
            List<long> upMatIdList = cirSfcList.Select(m => m.CirculationProductId).Distinct().ToList();
            var upMaterialList = await _procMaterialRepository.GetByIdsAsync(upMatIdList);
            //获取批次信息
            List<string> batchSfcList = new List<string>();
            batchSfcList.AddRange(allSfcList);
            batchSfcList.AddRange(nioSfcList);
            //List<SfcBatchDto> sfcBatchList = await GetSfcBatchListAsync(siteId, batchSfcList);

            // TODO: 替换为实际数据
            var dtos = new List<MaterialDto> { };

            foreach (var item in rotorSfcList)
            {
                string sfc = item.Sfc;
                var sfcMaterial = materialList.Where(m => m.MaterialCode == item.SfcMaterialCode).FirstOrDefault();
                //var sfcBatch = sfcBatchList.Where(m => m.Sfc == sfc).FirstOrDefault();
                string txSfc = item.TxSfc;
                var txSfcMaterial = materialList.Where(m => m.MaterialCode == item.TxSfcMaterialCode).FirstOrDefault();
                //var txSfcBatch = sfcBatchList.Where(m => m.Sfc == txSfc).FirstOrDefault();
                string zSfc = item.ZSfc;
                var zSfcMaterial = materialList.Where(m => m.MaterialCode == item.ZSfcMaterialCode).FirstOrDefault();
                //var zSfcBatch = sfcBatchList.Where(m => m.Sfc == zSfc).FirstOrDefault();

                MaterialDto model = new MaterialDto();
                model.VendorProductNum = sfcMaterial == null ? "010301000001" : sfcMaterial.MaterialCode;
                model.VendorProductSn = sfc;
                model.ChildSn = txSfc;
                model.ChildName = txSfcMaterial == null ? "铁芯" : txSfcMaterial.MaterialName;
                model.ChildNum = txSfcMaterial == null ? "030201000004" : txSfcMaterial.MaterialCode;
                model.UpdateTime = GetTimestamp(HymsonClock.Now());
                dtos.Add(model);

                MaterialDto zModel = new MaterialDto();
                zModel.VendorProductNum = sfcMaterial == null ? "010301000001" : sfcMaterial.MaterialCode;
                zModel.VendorProductSn = sfc;
                zModel.ChildSn = zSfc;
                zModel.ChildName = zSfcMaterial == null ? "电动机轴" : zSfcMaterial.MaterialName;
                zModel.ChildNum = zSfcMaterial == null ? "030203000012" : zSfcMaterial.MaterialCode;
                zModel.UpdateTime = GetTimestamp(HymsonClock.Now());
                dtos.Add(zModel);

                #region 是否追溯明细

                //if (false)
                //{
                //    //查铁芯上料数据
                //    List<string> existTxList = new List<string>();
                //    var curTxSfcList = cirSfcList.Where(m => m.SFC == txSfc).ToList();
                //    foreach (var txItem in curTxSfcList)
                //    {
                //        if (existTxList.Contains(txItem.CirculationBarCode) == true)
                //        {
                //            continue;
                //        }
                //        existTxList.Add(txItem.CirculationBarCode);

                //        MaterialDto txModel = new MaterialDto();
                //        txModel.VendorProductNum = sfcMaterial?.MaterialCode;
                //        txModel.VendorProductName = sfcMaterial?.MaterialName;
                //        txModel.VendorProductSn = sfc;
                //        txModel.VendorProductCode = txModel.VendorProductNum;
                //        txModel.VendorProductBatch = txModel.VendorProductNum;

                //        var parentModel = upMaterialList.Where(m => m.Id == txItem.CirculationProductId).FirstOrDefault();
                //        txModel.ParentCode = txItem.CirculationBarCode;
                //        txModel.ParentName = parentModel?.MaterialName;
                //        txModel.ParentBatch = txItem.CirculationBarCode;
                //        txModel.ParentNum = txItem.CirculationBarCode;
                //        txModel.ParentHardwareRevision = "1.0";
                //        txModel.ParentSoftwareRevision = "1.0";

                //        txModel.ChildCode = txSfc;
                //        txModel.ChildBatch = txSfc;
                //        txModel.ChildName = txSfcMaterial?.MaterialName;
                //        txModel.ChildCode = txSfcMaterial?.MaterialCode;
                //        //txModel.ChildSn = txSfc;
                //        txModel.ChildQualityControl = "";
                //        txModel.ChildHardwareRevision = "1.0";
                //        txModel.ChildSoftwareRevision = "1.0";
                //        txModel.ChildType = "转子";
                //        txModel.ChildProductionTime = GetTimestamp(HymsonClock.Now());
                //        txModel.ChildVendorName = "无";
                //        txModel.UpdateTime = GetTimestamp(HymsonClock.Now());
                //        txModel.Debug = NIO_DEBUG;

                //        dtos.Add(txModel);
                //    }

                //    //查轴码上料数据
                //    List<string> zExistList = new List<string>();
                //    var curZSfcList = cirSfcList.Where(m => m.SFC == zSfc).ToList();
                //    foreach (var zItem in curZSfcList)
                //    {
                //        if (zExistList.Contains(zItem.CirculationBarCode) == true)
                //        {
                //            continue;
                //        }
                //        zExistList.Add(zItem.CirculationBarCode);

                //        MaterialDto zModel = new MaterialDto();

                //        zModel.VendorProductNum = sfcMaterial?.MaterialCode;
                //        zModel.VendorProductName = sfcMaterial?.MaterialName;
                //        zModel.VendorProductSn = sfc;
                //        zModel.VendorProductCode = zModel.VendorProductNum;
                //        zModel.VendorProductBatch = zModel.VendorProductNum;

                //        var parentModel = upMaterialList.Where(m => m.Id == zItem.CirculationProductId).FirstOrDefault();
                //        zModel.ParentCode = zItem.CirculationBarCode;
                //        zModel.ParentName = parentModel?.MaterialName;
                //        zModel.ParentBatch = zItem.CirculationBarCode;
                //        zModel.ParentNum = zItem.CirculationBarCode;
                //        zModel.ParentHardwareRevision = "1.0";
                //        zModel.ParentSoftwareRevision = "1.0";

                //        zModel.ChildCode = zSfc;
                //        zModel.ChildBatch = zSfc;
                //        zModel.ChildName = zSfcMaterial?.MaterialName;
                //        zModel.ChildCode = zSfcMaterial?.MaterialCode;
                //        //zModel.ChildSn = txSfc;
                //        zModel.ChildQualityControl = "";
                //        zModel.ChildHardwareRevision = "1.0";
                //        zModel.ChildSoftwareRevision = "1.0";
                //        zModel.ChildType = "转子";
                //        zModel.ChildProductionTime = GetTimestamp(HymsonClock.Now());
                //        zModel.ChildVendorName = "无";
                //        zModel.UpdateTime = GetTimestamp(HymsonClock.Now());
                //        zModel.Debug = NIO_DEBUG;

                //        dtos.Add(zModel);
                //    }

                //}

                #endregion
            }

            DateTime? maxUpdateTime = rotorSfcList.Max(x => x.UpdatedOn);
            long timestamp = GetTimestampInMilliseconds(maxUpdateTime);

            //MES数据入库
            using var trans = TransactionHelper.GetTransactionScope();

            await AddToPushQueueAsync(config, buzScene, dtos);
            await _waterMarkService.RecordWaterMarkAsync(BusinessKey.NioMaterial, timestamp);

            trans.Complete();
        }

        /// <summary>
        /// 业务数据（产品一次合格率）
        /// </summary>
        /// <returns></returns>
        public async Task PassrateProductAsync()
        {
            _logger.LogInformation($"业务数据（产品一次合格率）PassrateProductAsync {HymsonClock.Now().ToString("yyyyMMdd HH:mm:ss")}");

            var buzScene = BuzSceneEnum.Buz_PassrateProduct;
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
            var baseConfigList = await GetBaseConfigAsync();
            IEnumerable<NIOConfigBaseDto> baseDataList = GetBaseData(baseConfigList);
            //获取当前水位
            var startWaterMarkId = await _waterMarkService.GetWaterMarkAsync(BusinessKey.NioPassrateProduct);
            //获取步骤数据
            EntityByWaterSiteIdQuery stepQuery = new EntityByWaterSiteIdQuery()
            { Rows = WATER_ALL_ROWS, SiteId = siteId, StartWaterMarkId = startWaterMarkId };
            var stepList = await _manuSfcStepRepository.GetSfcStepMavelAsync(stepQuery);
            if (stepList == null || !stepList.Any()) return;

            //型号
            List<long> materialIdList = stepList.Where(m => m.ProductId != 0).Select(m => m.ProductId).Distinct().ToList();
            IEnumerable<ProcMaterialEntity> materialList = await _procMaterialRepository.GetByIdsAsync(materialIdList);

            var dtos = new List<PassrateProductDto> { };

            //数据预处理
            stepList = stepList.Where(m => m.ProductId != 0).ToList();
            List<long> productIdList = stepList.Select(m => m.ProductId).Distinct().ToList();
            foreach (var item in productIdList)
            {
                //物料是否存在系统中
                ProcMaterialEntity? curMat = materialList.Where(m => m.Id == item).FirstOrDefault();
                if (curMat == null)
                {
                    continue;
                }
                //物料是否在当前配置的型号
                string materialCode = curMat.MaterialCode;
                NIOConfigBaseDto? curConfig = baseDataList.Where(m => m.VendorProductCode == materialCode).FirstOrDefault();
                if (curConfig == null)
                {
                    continue;
                }

                var curlAllStepList = stepList.Where(m => m.ProductId == item).Where(m => m.Remark == PRODUCRE_END).ToList();

                PassrateProductDto model = new PassrateProductDto();
                model.PlantId = curConfig.PlantId;
                model.WorkshopId = curConfig.WorkshopId;
                model.ProductionLineId = curConfig.ProductionLineId;
                model.VendorProductNum = curConfig.VendorProductCode;
                model.VendorProductName = curConfig.VendorProductName;
                model.PassRateTarget = Convert.ToDecimal(curConfig.PassRateTarget);
                model.PassRate = GetPassRate(curlAllStepList);
                model.UpdateTime = GetTimestamp(HymsonClock.Now());

                dtos.Add(model);
            }

            using var trans = TransactionHelper.GetTransactionScope();

            await AddToPushQueueAsync(config, buzScene, dtos);
            await _waterMarkService.RecordWaterMarkAsync(BusinessKey.NioPassrateProduct, stepList.Max(x => x.Id));

            trans.Complete();
        }

        /// <summary>
        /// 业务数据（工位一次合格率）
        /// NIO说不需要
        /// </summary>
        /// <returns></returns>
        public async Task PassrateStationAsync()
        {
            //蔚来说不需要
            return;

            var buzScene = BuzSceneEnum.Buz_PassrateStation;
            var config = await GetSwitchEntityAsync(buzScene);
            if (config == null) return;

            //站点配置
            var configEntities = await _sysConfigRepository.GetEntitiesAsync(new SysConfigQuery { Type = SysConfigEnum.MainSite });
            if (configEntities == null || !configEntities.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10139)).WithData("name", "MainSite");
            }
            long siteId = long.Parse(configEntities.ElementAt(0).Value);
            //获取工序一次良率
            var passrateStationConfigList = await _sysConfigRepository.GetEntitiesAsync(new SysConfigQuery { Type = SysConfigEnum.RotorPassrateStation });
            if (passrateStationConfigList == null || !passrateStationConfigList.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10139)).WithData("name", "RotorPassrateStation");
            }
            string passrateConfig = passrateStationConfigList.ElementAt(0).Value;
            //获取基础配置
            var baseConfigList = await GetBaseConfigAsync();
            IEnumerable<NIOConfigBaseDto> baseDataList = GetBaseData(baseConfigList);
            //获取当前水位
            var startWaterMarkId = await _waterMarkService.GetWaterMarkAsync(BusinessKey.NioPassrateProcedure);
            //获取步骤数据
            EntityByWaterSiteIdQuery stepQuery = new EntityByWaterSiteIdQuery()
            { Rows = WATER_ALL_ROWS, SiteId = siteId, StartWaterMarkId = startWaterMarkId };
            var stepList = await _manuSfcStepRepository.GetSfcStepMavelAsync(stepQuery);
            if (stepList == null || !stepList.Any()) return;

            //获取工序
            ProcProcedureQuery procedureQuery = new ProcProcedureQuery() { SiteId = siteId };
            var procedureList = await _procProcedureRepository.GetEntitiesAsync(procedureQuery);
            if (procedureList == null || procedureList.Any() == false) return;

            var dtos = new List<PassrateStationDto> { };
            //数据处理
            foreach (var item in procedureList)
            {
                var curStepList = stepList.Where(m => m.ProcedureId == item.Id).ToList();
                if (curStepList == null || curStepList.Any() == false)
                {
                    continue;
                }
                string procedureCode = item.Code;
                NIOConfigBaseDto curConfig = new NIOConfigBaseDto();
                if (procedureCode[0] == ROTOR_CHAR)
                {
                    curConfig = JsonConvert.DeserializeObject<NIOConfigBaseDto>(GetRotorConfig(baseConfigList));
                }
                else
                {
                    curConfig = JsonConvert.DeserializeObject<NIOConfigBaseDto>(GetStatorConfig(baseConfigList));
                }

                PassrateStationDto model = new PassrateStationDto();
                model.PlantId = curConfig.PlantId;
                model.WorkshopId = curConfig.WorkshopId;
                model.ProductionLineId = curConfig.ProductionLineId;
                model.VendorProductNum = curConfig.VendorProductCode;
                model.VendorProductName = curConfig.VendorProductName;
                model.StationId = item.Code;
                model.PassRateTarget = GetPassrateStation(passrateConfig, item.Code);
                model.PassRate = GetPassRate(curStepList);
                model.UpdateTime = GetTimestamp(HymsonClock.Now());
                model.Debug = NIO_DEBUG;

                dtos.Add(model);
            }

            using var trans = TransactionHelper.GetTransactionScope();

            await AddToPushQueueAsync(config, buzScene, dtos);
            await _waterMarkService.RecordWaterMarkAsync(BusinessKey.NioPassrateProcedure, stepList.Max(x => x.Id));

            trans.Complete();

            decimal GetPassrateStation(string configValue, string procedureCode)
            {
                decimal result = 0.9m;
                if (string.IsNullOrEmpty(configValue) == true)
                {
                    return result;
                }

                List<string> list = configValue.Split('&').ToList();
                foreach (var item in list)
                {
                    List<string> itemList = item.Split('=').ToList();
                    if (itemList.Count != 2)
                    {
                        return result;
                    }
                    if (itemList[0] == "default")
                    {
                        result = Convert.ToDecimal(itemList[1]);
                        continue;
                    }
                    if (itemList[0] == procedureCode)
                    {
                        result = Convert.ToDecimal(itemList[1]);
                        break;
                    }
                }

                return result;
            }
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
            _logger.LogInformation($"业务数据（缺陷业务）IssueAsync {HymsonClock.Now().ToString("yyyyMMdd HH:mm:ss")}");

            var buzScene = BuzSceneEnum.Buz_Issue;
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
            var baseConfigList = await GetBaseConfigAsync();
            //获取当前水位
            var startWaterMarkId = await _waterMarkService.GetWaterMarkAsync(BusinessKey.NioIssue);
            //获取参数表数据
            EntityByWaterMarkQuery waterQuery = new EntityByWaterMarkQuery() { Rows = WATER_ROWS, StartWaterMarkId = startWaterMarkId };
            var paramList = await _manuProductParameterRepository.GetManuNgParamMavelAsync(waterQuery);
            if (paramList == null || paramList.Any() == false) return;
            //获取标准参数，用于获取根据ID获取名称（参数表在其他库）
            ProcParameterQuery paramQuery = new ProcParameterQuery() { SiteId = siteId };
            var baseParamList = await _procParameterRepository.GetProcParameterEntitiesAsync(paramQuery);
            if (baseParamList == null || baseParamList.Any() == false) return;
            //获取工序
            ProcProcedureQuery procedureQuery = new ProcProcedureQuery() { SiteId = siteId };
            var procedureList = await _procProcedureRepository.GetEntitiesAsync(procedureQuery);
            if (procedureList == null || procedureList.Any() == false) return;

            //处理数据
            var dtos = new List<IssueDto> { };

            foreach (var item in paramList)
            {
                //标准参数
                var curBaseParam = baseParamList.Where(m => m.Id == item.ParameterId).FirstOrDefault();
                if (curBaseParam == null)
                {
                    continue;
                }
                string paramCode = curBaseParam.ParameterCode;
                char firstChar = paramCode[0];
                if (firstChar != ROTOR_CHAR && firstChar != STATOR_CHAR)
                {
                    continue;
                }
                //工序
                string procedure = "未知";
                var curProcedure = procedureList.Where(m => m.Id == item.ProcedureId).FirstOrDefault();
                if (curProcedure != null)
                {
                    procedure = curProcedure.Code;
                }

                NIOConfigBaseDto curConfig = new NIOConfigBaseDto();
                if (firstChar == ROTOR_CHAR)
                {
                    curConfig = JsonConvert.DeserializeObject<NIOConfigBaseDto>(GetRotorConfig(baseConfigList));
                }
                else
                {
                    curConfig = JsonConvert.DeserializeObject<NIOConfigBaseDto>(GetStatorConfig(baseConfigList));
                }
                if (curConfig == null)
                {
                    continue;
                }

                IssueDto model = new IssueDto();
                model.PlantId = curConfig.PlantId;
                model.WorkshopId = curConfig.WorkshopId;
                model.ProductionLineId = curConfig.ProductionLineId;
                model.StationId = procedure;
                model.VendorProductNum = curConfig.VendorProductCode;
                model.VendorProductSn = item.SFC;
                model.VendorProductTempSn = item.SFC;
                model.VendorIssueCode = paramCode;
                model.VendorIssueName = curBaseParam.ParameterName;
                model.UpdateTime = GetTimestamp(HymsonClock.Now());

                dtos.Add(model);
            }

            using var trans = TransactionHelper.GetTransactionScope();

            await AddToPushQueueAsync(config, buzScene, dtos);
            await _waterMarkService.RecordWaterMarkAsync(BusinessKey.NioIssue, paramList.Max(x => x.Id));

            trans.Complete();
        }

        /// <summary>
        /// 业务数据（工单业务）
        /// </summary>
        /// <returns></returns>
        public async Task WorkOrderAsync()
        {
            _logger.LogInformation($"业务数据（工单业务）WorkOrderAsync {HymsonClock.Now().ToString("yyyyMMdd HH:mm:ss")}");

            var buzScene = BuzSceneEnum.Buz_WorkOrder;
            var config = await GetSwitchEntityAsync(buzScene);
            if (config == null) return;

            //站点配置
            var configEntities = await _sysConfigRepository.GetEntitiesAsync(new SysConfigQuery { Type = SysConfigEnum.MainSite });
            if (configEntities == null || !configEntities.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10139)).WithData("name", "MainSite");
            }
            long siteId = long.Parse(configEntities.ElementAt(0).Value);
            //获取基础配置
            var baseConfigList = await GetBaseConfigAsync();
            IEnumerable<NIOConfigBaseDto> baseDataList = GetBaseData(baseConfigList);
            //获取工单数据
            var planOrderList = await _planWorkOrderRepository.GetWorkOrderMavelAsync(siteId);
            //处理数据
            var dtos = new List<WorkOrderDto> { };
            foreach (var item in planOrderList)
            {
                var curConfig = baseDataList.Where(m => m.VendorProductCode == item.MaterialCode).FirstOrDefault();
                if (curConfig == null)
                {
                    continue;
                }

                if (item.FinishProductQuantity <= 0)
                {
                    continue;
                }

                WorkOrderDto model = new WorkOrderDto();
                model.PlantId = curConfig.PlantId;
                model.WorkshopId = curConfig.WorkshopId;
                model.ProductionLineId = curConfig.ProductionLineId;
                //model.VendorProductName = curConfig.VendorProductName;
                model.VendorProductCode = curConfig.VendorProductCode;
                //model.NioProductNum = curConfig.NioProductCode;
                //model.NioProductCode = curConfig.NioProductCode;
                //model.NioProductName = curConfig.NioProductName;
                //model.NioModel = "ES8";
                model.Quantity = (int)item.FinishProductQuantity;
                //model.NioHardwareRevision = "1.0";
                //model.NioSoftwareRevision = "1.0";
                //model.NioProjectName = "";
                //model.Launched = NIO_DEBUG;
                model.UpdateTime = GetTimestamp(HymsonClock.Now());

                model.WorkorderId = item.OrderCode;
                model.OrderCreateTime = GetTimestamp(item.CreatedOn);

                dtos.Add(model);
            }

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
        private async Task<IEnumerable<SysConfigEntity>> GetBaseConfigAsync()
        {
            //基础数据配置
            SysConfigQuery configQuery = new SysConfigQuery();
            configQuery.Type = SysConfigEnum.NioBaseConfig;
            configQuery.Codes = BASE_CONFIG_LIST; //new List<string>() { "NioRotorConfig", "NioStatorConfig" };
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
            var config = list.Where(m => m.Code == NIO_ROTOR_CONFIG).FirstOrDefault();
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
            var config = list.Where(m => m.Code == NIO_STATOR_CONFIG).FirstOrDefault();
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
            return (long)((DateTime)date - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Local)).TotalSeconds;
        }

        /// <summary>
        /// 获取基础配置数据
        /// </summary>
        /// <returns></returns>
        private IEnumerable<NIOConfigBaseDto> GetBaseData(IEnumerable<SysConfigEntity> list)
        {
            List<NIOConfigBaseDto> resultList = new List<NIOConfigBaseDto>();

            //基础数据配置
            foreach (var item in list)
            {
                NIOConfigBaseDto model = JsonConvert.DeserializeObject<NIOConfigBaseDto>(item.Value);
                resultList.Add(model);
            }

            return resultList;
        }

        /// <summary>
        /// 获取良率
        /// </summary>
        /// <param name="stepList"></param>
        /// <returns></returns>
        private decimal GetPassRate(List<ManuSfcStepEntity> stepList)
        {
            decimal result = 0;
            //获取所有条码
            List<string> sfcList = stepList.Select(m => m.SFC).Distinct().ToList();
            //获取所有NG条码
            List<string> ngSfcList = stepList.Where(m => m.ScrapQty > 0).Select(m => m.SFC).Distinct().ToList();
            //获取NG条码是否有重复，并且第一条是NG的
            int firstOkNg = 0; //第一次OK后续NG
            var ngStepList = stepList.Where(m => ngSfcList.Contains(m.SFC) == true).ToList();
            foreach (var item in ngSfcList)
            {
                var curNgStepList = ngStepList.Where(m => m.SFC == item).ToList();
                if (curNgStepList.Count > 1)
                {
                    //查找最早一个
                    var maxEarNg = curNgStepList.OrderBy(m => m.CreatedOn).FirstOrDefault();
                    if (maxEarNg!.ScrapQty > 0)
                    {
                        ++firstOkNg;
                    }
                }
            }

            result = (sfcList.Count - ngSfcList.Count + firstOkNg) / sfcList.Count;
            result = decimal.Round(result, 4);
            return result;
        }

        /// <summary>
        /// 转为毫秒时间戳
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        private long GetTimestampInMilliseconds(DateTime? dateTime)
        {
            if (dateTime == null)
            {
                return 0;
            }

            // 首先将本地时间转换为UTC时间  
            DateTime utcDateTime = ((DateTime)dateTime).ToUniversalTime();
            // 然后计算UTC时间与Unix纪元（1970年1月1日UTC）之间的差值  
            DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Local);
            TimeSpan timeSpan = utcDateTime - epoch;
            return (long)timeSpan.TotalMilliseconds;
        }

        /// <summary>
        /// 将Unix时间戳（毫秒）转换为DateTime  
        /// </summary>
        /// <param name="timestamp"></param>
        /// <returns></returns>
        private DateTime UnixTimestampMillisToDateTime(long timestamp)
        {
            // 将Unix时间戳转换为UTC DateTime  
            DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            DateTime utcDateTime = epoch.AddMilliseconds(timestamp);
            // 然后将UTC DateTime转换为本地时间  
            DateTime localDateTime = utcDateTime.ToLocalTime();
            return localDateTime;
        }

        /// <summary>
        /// 获取条码批次信息
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="sfcList"></param>
        /// <returns></returns>
        private async Task<List<SfcBatchDto>> GetSfcBatchListAsync(long siteId, List<string> sfcList)
        {
            List<SfcBatchDto> resultList = new List<SfcBatchDto>();

            //获取条码对应批次码-(铁芯，轴码)
            ManuRequistionOrderReceiveQuery matQuery = new ManuRequistionOrderReceiveQuery();
            matQuery.SiteId = siteId;
            matQuery.MaterialBarCodeList = sfcList;
            var barCodeBatchList = await _manuRequistionOrderReceiveRepository.GetEntitiesAsync(matQuery);
            if (barCodeBatchList != null && barCodeBatchList.Count() > 0)
            {
                resultList.AddRange(barCodeBatchList.Select(m => new SfcBatchDto()
                {
                    Sfc = m.MaterialBarCode,
                    Batch = m.Batch
                }));
            }
            //获取条码对应批次码-(成品码)
            QueryManuProductReceiptOrderDetail detailQuery = new QueryManuProductReceiptOrderDetail();
            detailQuery.SiteId = siteId;
            detailQuery.SFCs = sfcList;
            var snList = await _manuProductReceiptOrderDetailRepository.GetListAsync(detailQuery);
            if (snList != null && snList.Count() > 0)
            {
                resultList.AddRange(snList.Select(m => new SfcBatchDto()
                {
                    Sfc = m.Sfc,
                    Batch = m.Batch
                }));
            }

            return resultList;
        }
    }
}
