using Hymson.Infrastructure.Exceptions;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Manufacture;
using Hymson.MES.Data.Repositories.Common;
using Hymson.MES.Data.Repositories.Common.Query;
using Hymson.MES.Data.Repositories.Integrated.IIntegratedRepository;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Data.Repositories.Plan;
using Hymson.MES.Data.Repositories.Process;
using Hymson.Snowflake;
using Hymson.Utils;
using Hymson.Utils.Tools;
using Hymson.WaterMark;

namespace Hymson.MES.BackgroundServices.Stator.Services
{
    /// <summary>
    /// 服务
    /// </summary>
    public class BaseService : IBaseService
    {
        /// <summary>
        /// 服务接口（水位）
        /// </summary>
        private readonly IWaterMarkService _waterMarkService;

        /// <summary>
        /// 仓储接口（系统配置）
        /// </summary>
        private readonly ISysConfigRepository _sysConfigRepository;

        /// <summary>
        /// 仓储接口（工作中心）
        /// </summary>
        private readonly IInteWorkCenterRepository _inteWorkCenterRepository;

        /// <summary>
        /// 仓储接口（生产工单）
        /// </summary>
        private readonly IPlanWorkOrderRepository _planWorkOrderRepository;

        /// <summary>
        /// 仓储接口（工序维护）
        /// </summary>
        private readonly IProcProcedureRepository _procProcedureRepository;

        /// <summary>
        /// 仓储接口（条码）
        /// </summary>
        private readonly IManuSfcRepository _manuSfcRepository;

        /// <summary>
        /// 仓储接口（条码信息）
        /// </summary>
        private readonly IManuSfcInfoRepository _manuSfcInfoRepository;

        /// <summary>
        /// 仓储接口（条码步骤）
        /// </summary>
        private readonly IManuSfcStepRepository _manuSfcStepRepository;

        /// <summary>
        /// 仓储接口（条码流转）
        /// </summary>
        private readonly IManuSfcCirculationRepository _manuSfcCirculationRepository;

        /// <summary>
        /// 仓储接口（产品不良录入）
        /// </summary>
        private readonly IManuProductBadRecordRepository _manuProductBadRecordRepository;

        /// <summary>
        /// 仓储接口（产品NG记录表）
        /// </summary>
        private readonly IManuProductNgRecordRepository _manuProductNgRecordRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="waterMarkService"></param>
        /// <param name="sysConfigRepository"></param>
        /// <param name="inteWorkCenterRepository"></param>
        /// <param name="planWorkOrderRepository"></param>
        /// <param name="procProcedureRepository"></param>
        /// <param name="manuSfcRepository"></param>
        /// <param name="manuSfcInfoRepository"></param>
        /// <param name="manuSfcStepRepository"></param>
        /// <param name="manuSfcCirculationRepository"></param>
        /// <param name="manuProductBadRecordRepository"></param>
        /// <param name="manuProductNgRecordRepository"></param>
        public BaseService(IWaterMarkService waterMarkService,
            ISysConfigRepository sysConfigRepository,
            IInteWorkCenterRepository inteWorkCenterRepository,
            IPlanWorkOrderRepository planWorkOrderRepository,
            IProcProcedureRepository procProcedureRepository,
            IManuSfcRepository manuSfcRepository,
            IManuSfcInfoRepository manuSfcInfoRepository,
            IManuSfcStepRepository manuSfcStepRepository,
            IManuSfcCirculationRepository manuSfcCirculationRepository,
            IManuProductBadRecordRepository manuProductBadRecordRepository,
            IManuProductNgRecordRepository manuProductNgRecordRepository)
        {
            _waterMarkService = waterMarkService;
            _sysConfigRepository = sysConfigRepository;
            _inteWorkCenterRepository = inteWorkCenterRepository;
            _planWorkOrderRepository = planWorkOrderRepository;
            _procProcedureRepository = procProcedureRepository;
            _manuSfcRepository = manuSfcRepository;
            _manuSfcInfoRepository = manuSfcInfoRepository;
            _manuSfcStepRepository = manuSfcStepRepository;
            _manuSfcCirculationRepository = manuSfcCirculationRepository;
            _manuProductBadRecordRepository = manuProductBadRecordRepository;
            _manuProductNgRecordRepository = manuProductNgRecordRepository;
        }


        /// <summary>
        /// 获取基础配置（定子）
        /// </summary>
        /// <returns></returns>
        public async Task<BaseStatorBo> GetStatorBaseConfigAsync()
        {
            // 初始化对象
            var baseDto = new BaseStatorBo
            {
                User = "StatorTask",
                Time = HymsonClock.Now()
            };

            // 读取站点配置
            var siteConfigEntities = await _sysConfigRepository.GetEntitiesAsync(new SysConfigQuery { Type = SysConfigEnum.MainSite });
            if (siteConfigEntities == null || !siteConfigEntities.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10139))
                    .WithData("name", SysConfigEnum.MainSite.GetDescription());
            }

            // 站点配置
            var siteConfigEntity = siteConfigEntities.FirstOrDefault(f => f.Code == "MainSite")
                ?? throw new CustomerValidationException(nameof(ErrorCode.MES12842)).WithData("Msg", "站点配置不存在！");

            // 站点赋值
            baseDto.SiteId = siteConfigEntity.Value.ParseToLong();

            // 读取产线配置
            var lineConfigEntities = await _sysConfigRepository.GetEntitiesAsync(new SysConfigQuery { Type = SysConfigEnum.NioBaseConfig });
            if (lineConfigEntities == null || !lineConfigEntities.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10139))
                    .WithData("name", SysConfigEnum.NioBaseConfig.GetDescription());
            }

            // 定子线配置
            var lineConfigEntity = lineConfigEntities.FirstOrDefault(f => f.Code == "NioStatorConfig")
                ?? throw new CustomerValidationException(nameof(ErrorCode.MES12842)).WithData("Msg", "定子线配置不存在！");

            // 定子线赋值
            var configBo = lineConfigEntity.Value.ToDeserialize<NIOConfigBaseDto>();
            if (configBo != null)
            {
                // 读取产线
                var workLineEntity = await _inteWorkCenterRepository.GetByCodeAsync(new EntityByCodeQuery
                {
                    Site = baseDto.SiteId,
                    Code = configBo.ProductionLineId
                });
                if (workLineEntity == null) return baseDto;

                baseDto.WorkLineId = workLineEntity.Id;

                // 读取产线当前激活的工单
                var workOrderEntities = await _planWorkOrderRepository.GetByWorkLineIdAsync(workLineEntity.Id);
                if (workOrderEntities == null || !workOrderEntities.Any()) return baseDto;

                // 这里激活的工单应该只能有一个
                var workOrderEntity = workOrderEntities.FirstOrDefault();
                if (workOrderEntity == null) return baseDto;

                // 填充信息
                baseDto.WorkOrderId = workOrderEntity.Id;
                baseDto.ProductBOMId = workOrderEntity.ProductBOMId;
                baseDto.ProcessRouteId = workOrderEntity.ProcessRouteId;
                baseDto.ProductId = workOrderEntity.ProductId;
            }

            return baseDto;
        }

        /// <summary>
        /// 保存转换数据
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<StatorSummaryBo> ConvertDataAsync<T>(IEnumerable<T> entities) where T : BaseOPEntity
        {
            var producreCode = $"{typeof(T).Name}";

            // 初始化对象
            var baseBo = await GetStatorBaseConfigAsync();
            var summaryBo = new StatorSummaryBo { };

            // 读取当前工序
            var procedureEntity = await _procProcedureRepository.GetByCodeAsync(new EntityByCodeQuery
            {
                Site = baseBo.SiteId,
                Code = $"{StatorConst.PRODUCRE_PREFIX}{producreCode}"
            });
            if (procedureEntity == null) return summaryBo;
            baseBo.ProcedureId = procedureEntity.Id;

            // 遍历记录
            var user = "LMS";
            var qty = 1;
            foreach (var opEntity in entities)
            {
                if (opEntity.Barcode == "-") continue;

                // 条码
                var barCode = opEntity.Barcode;
                switch (producreCode)
                {
                    case "":
                        break;
                    default:
                        break;
                }

                // 条码ID
                var manuSFCId = IdGenProvider.Instance.CreateId();
                var manuSFCInfoId = IdGenProvider.Instance.CreateId();
                var manuSFCStepId = IdGenProvider.Instance.CreateId();
                var manuBadRecordId = IdGenProvider.Instance.CreateId();

                // 插入条码
                summaryBo.ManuSFCEntities.Add(new ManuSfcEntity
                {
                    Id = manuSFCId,
                    Qty = qty,
                    SFC = barCode,
                    IsUsed = YesOrNoEnum.No,
                    Type = SfcTypeEnum.NoProduce,
                    Status = SfcStatusEnum.Complete,

                    SiteId = baseBo.SiteId,
                    CreatedBy = baseBo.User,
                    CreatedOn = baseBo.Time,
                    UpdatedBy = user,
                    UpdatedOn = opEntity.RDate
                });

                // 插入条码信息
                summaryBo.ManuSFCInfoEntities.Add(new ManuSfcInfoEntity
                {
                    Id = manuSFCInfoId,
                    SfcId = manuSFCId,
                    WorkOrderId = baseBo.WorkOrderId,
                    ProductId = baseBo.ProductId,
                    ProductBOMId = baseBo.ProductBOMId,
                    ProcessRouteId = baseBo.ProcessRouteId,
                    IsUsed = false,

                    SiteId = baseBo.SiteId,
                    CreatedBy = baseBo.User,
                    CreatedOn = baseBo.Time,
                    UpdatedBy = user,
                    UpdatedOn = opEntity.RDate
                });

                // 插入步骤表
                summaryBo.ManuSfcStepEntities.Add(new ManuSfcStepEntity
                {
                    Id = manuSFCStepId,
                    Operatetype = ManuSfcStepTypeEnum.OutStock,
                    CurrentStatus = SfcStatusEnum.lineUp,
                    SFC = barCode,
                    ProductId = baseBo.ProductId,
                    WorkOrderId = baseBo.WorkOrderId,
                    WorkCenterId = baseBo.WorkLineId,
                    ProductBOMId = baseBo.ProductBOMId,
                    ProcessRouteId = baseBo.ProcessRouteId,
                    SFCInfoId = manuSFCInfoId,
                    Qty = qty,
                    VehicleCode = "",
                    ProcedureId = baseBo.ProcedureId,
                    ResourceId = null,
                    EquipmentId = null,
                    OperationProcedureId = baseBo.ProcedureId,
                    OperationResourceId = null,
                    OperationEquipmentId = null,

                    SiteId = baseBo.SiteId,
                    CreatedBy = baseBo.User,
                    CreatedOn = baseBo.Time,
                    UpdatedBy = user,
                    UpdatedOn = opEntity.RDate
                });

                // 如果是不合格
                var isOk = opEntity.Result == "OK";
                if (isOk) continue;

                // 插入不良记录
                summaryBo.ManuProductBadRecordEntities.Add(new ManuProductBadRecordEntity
                {
                    Id = manuBadRecordId,
                    FoundBadOperationId = baseBo.ProcedureId,
                    OutflowOperationId = baseBo.ProcedureId,
                    UnqualifiedId = 0,
                    SFC = barCode,
                    SfcInfoId = 0,
                    SfcStepId = manuSFCStepId,
                    Qty = 1,
                    Status = ProductBadRecordStatusEnum.Open,
                    Source = ProductBadRecordSourceEnum.EquipmentReBad,
                    Remark = "",

                    SiteId = baseBo.SiteId,
                    CreatedBy = baseBo.User,
                    CreatedOn = baseBo.Time,
                    UpdatedBy = user,
                    UpdatedOn = opEntity.RDate
                });

                // 插入NG记录
                summaryBo.ManuProductNgRecordEntities.Add(new ManuProductNgRecordEntity
                {
                    Id = IdGenProvider.Instance.CreateId(),
                    BadRecordId = manuBadRecordId,
                    UnqualifiedId = 0,
                    NGCode = "未知",

                    SiteId = baseBo.SiteId,
                    CreatedBy = baseBo.User,
                    CreatedOn = baseBo.Time,
                    UpdatedBy = user,
                    UpdatedOn = opEntity.RDate
                });
            }

            /*
            // 遍历记录
            var user = "LMS";
            var qty = 1;
            var waterLevel = 0;
            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                var dr = dataTable.Rows[i];
                var index = dr["index"].ParseToInt();
                var time = dr["RDate"].ToTime();
                var barCode = $"{dr["wire1_barcode"]}";

                // 更新水位
                waterLevel = index > waterLevel ? index : waterLevel;

                if (barCode == "-") continue;

                // 条码ID
                var manuSFCId = IdGenProvider.Instance.CreateId();
                var manuSFCInfoId = IdGenProvider.Instance.CreateId();
                var manuSFCStepId = IdGenProvider.Instance.CreateId();
                var manuBadRecordId = IdGenProvider.Instance.CreateId();

                // 插入条码
                summaryBo.ManuSFCEntities.Add(new ManuSfcEntity
                {
                    Id = manuSFCId,
                    Qty = qty,
                    SFC = barCode,
                    IsUsed = YesOrNoEnum.No,
                    Type = SfcTypeEnum.NoProduce,
                    Status = SfcStatusEnum.Complete,

                    SiteId = baseBo.SiteId,
                    CreatedBy = user,
                    CreatedOn = time,
                    UpdatedBy = baseBo.User,
                    UpdatedOn = baseBo.Time
                });

                // 插入条码信息
                summaryBo.ManuSFCInfoEntities.Add(new ManuSfcInfoEntity
                {
                    Id = manuSFCInfoId,
                    SfcId = manuSFCId,
                    WorkOrderId = baseBo.WorkOrderId,
                    ProductId = baseBo.ProductId,
                    ProductBOMId = baseBo.ProductBOMId,
                    ProcessRouteId = baseBo.ProcessRouteId,
                    IsUsed = false,

                    SiteId = baseBo.SiteId,
                    CreatedBy = user,
                    CreatedOn = time,
                    UpdatedBy = baseBo.User,
                    UpdatedOn = baseBo.Time
                });

                // 插入步骤表
                summaryBo.ManuSfcStepEntities.Add(new ManuSfcStepEntity
                {
                    Id = manuSFCStepId,
                    Operatetype = ManuSfcStepTypeEnum.OutStock,
                    CurrentStatus = SfcStatusEnum.lineUp,
                    SFC = barCode,
                    ProductId = baseBo.ProductId,
                    WorkOrderId = baseBo.WorkOrderId,
                    WorkCenterId = baseBo.WorkLineId,
                    ProductBOMId = baseBo.ProductBOMId,
                    ProcessRouteId = baseBo.ProcessRouteId,
                    SFCInfoId = manuSFCInfoId,
                    Qty = qty,
                    VehicleCode = "",
                    ProcedureId = baseBo.ProcedureId,
                    ResourceId = null,
                    EquipmentId = null,
                    OperationProcedureId = baseBo.ProcedureId,
                    OperationResourceId = null,
                    OperationEquipmentId = null,

                    SiteId = baseBo.SiteId,
                    CreatedBy = user,
                    CreatedOn = time,
                    UpdatedBy = baseBo.User,
                    UpdatedOn = baseBo.Time
                });

                // 如果是不合格
                var isOk = $"{dr["Result"]}" == "OK";
                if (isOk) continue;

                // 插入不良记录
                summaryBo.ManuProductBadRecordEntities.Add(new ManuProductBadRecordEntity
                {
                    Id = manuBadRecordId,
                    FoundBadOperationId = baseBo.ProcedureId,
                    OutflowOperationId = baseBo.ProcedureId,
                    UnqualifiedId = 0,
                    SFC = barCode,
                    SfcInfoId = 0,
                    SfcStepId = manuSFCStepId,
                    Qty = 1,
                    Status = ProductBadRecordStatusEnum.Open,
                    Source = ProductBadRecordSourceEnum.EquipmentReBad,
                    Remark = "",

                    SiteId = baseBo.SiteId,
                    CreatedBy = user,
                    CreatedOn = time,
                    UpdatedBy = baseBo.User,
                    UpdatedOn = baseBo.Time
                });

                // 插入NG记录
                summaryBo.ManuProductNgRecordEntities.Add(new ManuProductNgRecordEntity
                {
                    Id = IdGenProvider.Instance.CreateId(),
                    BadRecordId = manuBadRecordId,
                    UnqualifiedId = 0,
                    NGCode = "未知",

                    SiteId = baseBo.SiteId,
                    CreatedBy = user,
                    CreatedOn = time,
                    UpdatedBy = baseBo.User,
                    UpdatedOn = baseBo.Time
                });
            }
            */

            return summaryBo;
        }

        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="summaryBo"></param>
        /// <returns></returns>
        public async Task<int> SaveBaseDataAsync(StatorSummaryBo summaryBo)
        {
            var rows = 0;

            List<Task<int>> saveTasks = new()
            {
                _manuSfcRepository.ReplaceRangeAsync(summaryBo.ManuSFCEntities),
                _manuSfcInfoRepository.ReplaceRangeAsync(summaryBo.ManuSFCInfoEntities),
                _manuSfcStepRepository.InsertRangeAsync(summaryBo.ManuSfcStepEntities),
                _manuSfcCirculationRepository.InsertRangeAsync(summaryBo.ManuSfcCirculationEntities),
                _manuProductBadRecordRepository.InsertRangeAsync(summaryBo.ManuProductBadRecordEntities),
                _manuProductNgRecordRepository.InsertRangeAsync(summaryBo.ManuProductNgRecordEntities)
            };

            var rowArray = await Task.WhenAll(saveTasks);
            rows += rowArray.Sum();
            return rows;
        }

        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="buzKey"></param>
        /// <param name="waterLevel"></param>
        /// <param name="summaryBo"></param>
        /// <returns></returns>
        public async Task<int> SaveBaseDataWithCommitAsync(string buzKey, long waterLevel, StatorSummaryBo summaryBo)
        {
            var rows = 0;
            using var trans = TransactionHelper.GetTransactionScope();

            rows += await SaveBaseDataAsync(summaryBo);
            rows += await _waterMarkService.RecordWaterMarkAsync(buzKey, waterLevel);

            trans.Complete();
            return rows;
        }

    }
}
