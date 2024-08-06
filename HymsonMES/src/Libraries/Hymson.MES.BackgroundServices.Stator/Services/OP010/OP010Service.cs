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
using Hymson.Utils.Tools;
using Hymson.WaterMark;
using Microsoft.Extensions.Logging;

namespace Hymson.MES.BackgroundServices.Stator.Services
{
    /// <summary>
    /// 服务
    /// </summary>
    public class OP010Service : IOP010Service
    {
        /// <summary>
        /// 日志接口
        /// </summary>
        private readonly ILogger<OP010Service> _logger;

        /// <summary>
        /// 仓储接口（工序）
        /// </summary>
        private readonly IOPRepository<OP010> _opRepository;

        /// <summary>
        /// 服务接口（基础）
        /// </summary>
        public readonly IBaseService _baseService;

        /// <summary>
        /// 服务接口（水位）
        /// </summary>
        public readonly IWaterMarkService _waterMarkService;

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
        /// <param name="logger"></param>
        /// <param name="opRepository"></param>
        /// <param name="baseService"></param>
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
        public OP010Service(ILogger<OP010Service> logger,
            IOPRepository<OP010> opRepository,
            IBaseService baseService,
            IWaterMarkService waterMarkService,
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
            _logger = logger;
            _opRepository = opRepository;
            _baseService = baseService;
            _waterMarkService = waterMarkService;
            _procProcedureRepository = procProcedureRepository;
            _manuSfcRepository = manuSfcRepository;
            _manuSfcInfoRepository = manuSfcInfoRepository;
            _manuSfcStepRepository = manuSfcStepRepository;
            _manuSfcCirculationRepository = manuSfcCirculationRepository;
            _manuProductBadRecordRepository = manuProductBadRecordRepository;
            _manuProductNgRecordRepository = manuProductNgRecordRepository;
        }

        /// <summary>
        /// 执行统计
        /// </summary>
        /// <param name="limitCount"></param>
        /// <returns></returns>
        public async Task<int> ExecuteAsync(int limitCount)
        {
            var producreCode = $"{typeof(OP010).Name}";
            var buzKey = $"Stator-{producreCode}";
            var waterMarkId = await _waterMarkService.GetWaterMarkAsync(buzKey);

            // 根据水位读取数据
            var opList = await _opRepository.GetListByStartWaterMarkIdAsync(new EntityByWaterMarkQuery
            {
                StartWaterMarkId = waterMarkId,
                Rows = limitCount
            });

            // 初始化对象
            var baseBo = await _baseService.GetStatorBaseConfigAsync();
            var summaryBo = new StatorSummaryBo { };

            // 读取当前工序
            var procedureEntity = await _procProcedureRepository.GetByCodeAsync(new EntityByCodeQuery
            {
                Site = baseBo.SiteId,
                Code = $"{StatorConst.PRODUCRE_PREFIX}{producreCode}"
            });
            if (procedureEntity == null) return 0;
            baseBo.ProcedureId = procedureEntity.Id;

            // 遍历记录
            var user = "LMS";
            var qty = 1;
            foreach (var opEntity in opList)
            {
                if (opEntity.Barcode == "-") continue;

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
                    SFC = opEntity.Barcode,
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
                    SFC = opEntity.Barcode,
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
                    SFC = opEntity.Barcode,
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

            var rows = 0;
            using var trans = TransactionHelper.GetTransactionScope();

            List<Task<int>> saveTasks = new()
            {
                _manuSfcRepository.ReplaceRangeAsync(summaryBo.ManuSFCEntities),
                _manuSfcInfoRepository.ReplaceRangeAsync(summaryBo.ManuSFCInfoEntities),
                _manuSfcStepRepository.InsertRangeAsync(summaryBo.ManuSfcStepEntities),
                _manuSfcCirculationRepository.InsertRangeAsync(summaryBo.ManuSfcCirculationEntities),
                _manuProductBadRecordRepository.InsertRangeAsync(summaryBo.ManuProductBadRecordEntities),
                _manuProductNgRecordRepository.InsertRangeAsync(summaryBo.ManuProductNgRecordEntities),
                _waterMarkService.RecordWaterMarkAsync(buzKey, opList.Max(m => m.index))
            };

            var rowArray = await Task.WhenAll(saveTasks);
            rows += rowArray.Sum();

            trans.Complete();
            return rows;
        }

    }
}
