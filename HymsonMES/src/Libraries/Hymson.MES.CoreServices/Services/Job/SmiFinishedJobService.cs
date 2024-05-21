using Hymson.Infrastructure.Exceptions;
using Hymson.Localization.Services;
using Hymson.MES.Core.Attribute.Job;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Constants.Process;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Core.Domain.Warehouse;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Job;
using Hymson.MES.Core.Enums.Manufacture;
using Hymson.MES.Core.Enums.Warehouse;
using Hymson.MES.CoreServices.Bos.Common;
using Hymson.MES.CoreServices.Bos.Job;
using Hymson.MES.CoreServices.Services.Common;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Data.Repositories.Plan;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Data.Repositories.Warehouse;
using Hymson.Snowflake;

namespace Hymson.MES.CoreServices.Services.Job
{
    /// <summary>
    /// 半成品完成
    /// </summary>
    [Job("半成品完成", JobTypeEnum.Standard)]
    public class SmiFinishedJobService : IJobService
    {
        /// <summary>
        /// 服务接口（主数据）
        /// </summary>
        private readonly IMasterDataService _masterDataService;

        /// <summary>
        /// 仓储接口（工序）
        /// </summary>
        private readonly IProcProcedureRepository _procProcedureRepository;

        /// <summary>
        /// 仓储接口（生产工单）
        /// </summary>
        private readonly IPlanWorkOrderRepository _planWorkOrderRepository;

        /// <summary>
        /// 仓储接口（条码信息）
        /// </summary>
        private readonly IManuSfcRepository _manuSfcRepository;

        /// <summary>
        /// 仓储接口（条码步骤）
        /// </summary>
        private readonly IManuSfcStepRepository _manuSfcStepRepository;

        /// <summary>
        /// 仓储接口（条码生产信息）
        /// </summary>
        private readonly IManuSfcProduceRepository _manuSfcProduceRepository;

        /// <summary>
        /// 仓储接口（物料库存）
        /// </summary>
        private readonly IWhMaterialInventoryRepository _whMaterialInventoryRepository;

        /// <summary>
        /// 仓储接口（物料台账）
        /// </summary>
        private readonly IWhMaterialStandingbookRepository _whMaterialStandingbookRepository;

        /// <summary>
        /// 
        /// </summary>
        private readonly ILocalizationService _localizationService;


        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="masterDataService"></param>
        /// <param name="procProcedureRepository"></param>
        /// <param name="planWorkOrderRepository"></param>
        /// <param name="manuSfcRepository"></param>
        /// <param name="manuSfcStepRepository"></param>
        /// <param name="manuSfcProduceRepository"></param>
        /// <param name="whMaterialInventoryRepository"></param>
        /// <param name="whMaterialStandingbookRepository"></param>
        /// <param name="localizationService"></param>
        public SmiFinishedJobService(IMasterDataService masterDataService,
            IProcProcedureRepository procProcedureRepository,
            IPlanWorkOrderRepository planWorkOrderRepository,
            IManuSfcRepository manuSfcRepository,
            IManuSfcStepRepository manuSfcStepRepository,
            IManuSfcProduceRepository manuSfcProduceRepository,
            IWhMaterialInventoryRepository whMaterialInventoryRepository,
            IWhMaterialStandingbookRepository whMaterialStandingbookRepository,
            ILocalizationService localizationService)
        {
            _masterDataService = masterDataService;
            _procProcedureRepository = procProcedureRepository;
            _planWorkOrderRepository = planWorkOrderRepository;
            _manuSfcRepository = manuSfcRepository;
            _manuSfcStepRepository = manuSfcStepRepository;
            _manuSfcProduceRepository = manuSfcProduceRepository;
            _whMaterialInventoryRepository = whMaterialInventoryRepository;
            _whMaterialStandingbookRepository = whMaterialStandingbookRepository;
            _localizationService = localizationService;
        }


        /// <summary>
        /// 参数校验
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task VerifyParamAsync<T>(T param) where T : JobBaseBo
        {
            if (param is not JobRequestBo commonBo) return;
            if (commonBo == null) return;
            if (commonBo.OutStationRequestBos == null || !commonBo.OutStationRequestBos.Any()) return;

            // 取得不合格的条码
            var unQualifiedBos = commonBo.OutStationRequestBos.Where(w => w.IsQualified == false);
            if (unQualifiedBos.Any()) throw new CustomerValidationException(nameof(ErrorCode.MES17427))
                    .WithData("SFC", string.Join(',', unQualifiedBos.Select(s => s.SFC)));

            // 临时中转变量
            var multiSFCBo = new MultiSFCBo { SiteId = commonBo.SiteId, SFCs = commonBo.OutStationRequestBos.Select(s => s.SFC) };

            // 获取生产条码信息
            var sfcProduceEntities = await commonBo.Proxy!.GetDataBaseValueAsync(_masterDataService.GetProduceEntitiesBySFCsAsync, multiSFCBo);
            if (sfcProduceEntities == null || !sfcProduceEntities.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES17415)).WithData("SFC", string.Join(',', multiSFCBo.SFCs));
            }

            // 判断条码锁状态
            await commonBo.Proxy.GetValueAsync(_masterDataService.GetProduceBusinessEntitiesBySFCsAsync, multiSFCBo);

            // 判断条码状态是否是"完成"
            var sfcEntities = await _manuSfcRepository.GetListAsync(new ManuSfcQuery
            {
                SiteId = param.SiteId,
                SFCs = multiSFCBo.SFCs,
                Type = SfcTypeEnum.Produce
            })
                ?? throw new CustomerValidationException(nameof(ErrorCode.MES17104));

            if (sfcEntities.Any(a => a.Status == SfcStatusEnum.Complete))
            {
                // 结束
                throw new CustomerValidationException(nameof(ErrorCode.MES18220));
            }
        }

        /// <summary>
        /// 执行前节点
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<IEnumerable<JobBo>?> BeforeExecuteAsync<T>(T param) where T : JobBaseBo
        {
            return await Task.FromResult<IEnumerable<JobBo>?>(default);
        }

        /// <summary>
        /// 数据组装
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<object?> DataAssemblingAsync<T>(T param) where T : JobBaseBo
        {
            if (param is not JobRequestBo commonBo) return default;
            if (commonBo == null) return default;
            if (commonBo.OutStationRequestBos == null || !commonBo.OutStationRequestBos.Any()) return default;

            // 取得合格的条码
            var qualifiedBos = commonBo.OutStationRequestBos.Where(w => w.IsQualified == true);
            if (!qualifiedBos.Any()) return default;

            // 临时中转变量
            var multiSFCBo = new MultiSFCBo { SiteId = commonBo.SiteId, SFCs = qualifiedBos.Select(s => s.SFC) };

            // 获取生产条码信息
            var sfcProduceEntities = await commonBo.Proxy!.GetDataBaseValueAsync(_masterDataService.GetProduceEntitiesBySFCsAsync, multiSFCBo);
            if (sfcProduceEntities == null || !sfcProduceEntities.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES17415)).WithData("SFC", string.Join(',', multiSFCBo.SFCs));
            }

            // 查询工序信息
            var procProcedureEntity = await _procProcedureRepository.GetByIdAsync(commonBo.ProcedureId);
            var cycle = procProcedureEntity.Cycle ?? 1;

            // 读取条码信息
            var manuSFCEntities = await _manuSfcRepository.GetListAsync(new ManuSfcQuery
            {
                Ids = sfcProduceEntities.Select(s => s.SFCId)
            });

            // 待执行的命令
            SmiFinisheResponseSummaryBo responseSummaryBo = new();

            // 查询条码里面所有的工单信息
            var planWorkOrderEntities = await _planWorkOrderRepository.GetByIdsAsync(sfcProduceEntities.Select(s => s.WorkOrderId));

            // 遍历所有条码
            List<long> deleteSFCProduceIds = new();
            foreach (var requestBo in qualifiedBos)
            {
                var sfcProduceEntity = sfcProduceEntities.FirstOrDefault(s => s.SFC == requestBo.SFC)
                    ?? throw new CustomerValidationException(nameof(ErrorCode.MES17102)).WithData("SFC", requestBo.SFC);

                var planWorkOrderEntity = planWorkOrderEntities.FirstOrDefault(f => f.Id == sfcProduceEntity.WorkOrderId)
                    ?? throw new CustomerValidationException(nameof(ErrorCode.MES16367)).WithData("SFC", requestBo.SFC);

                // 验证条码对应的物料ID是否和工单物料ID一致
                if (sfcProduceEntity.ProductId == planWorkOrderEntity.ProductId) continue;

                // 读取产品基础信息
                var procMaterialEntity = await _masterDataService.GetProcMaterialEntityWithNullCheckAsync(sfcProduceEntity.ProductId);

                // 获取当前条码信息
                var manuSfcEntity = manuSFCEntities.FirstOrDefault(s => s.Id == sfcProduceEntity.SFCId);
                if (manuSfcEntity == null) continue;

                // 条码状态（当前状态）
                var currentStatus = sfcProduceEntity.Status;

                // 更新条码信息
                sfcProduceEntity.Status = SfcStatusEnum.Complete;
                sfcProduceEntity.UpdatedBy = commonBo.UserName;
                sfcProduceEntity.UpdatedOn = commonBo.Time;

                // 条码状态跟在制品状态一致
                manuSfcEntity.Status = sfcProduceEntity.Status;
                manuSfcEntity.UpdatedBy = commonBo.UserName;
                manuSfcEntity.UpdatedOn = commonBo.Time;

                // 组装（出站步骤数据）
                var stepEntity = new ManuSfcStepEntity
                {
                    // 插入 manu_sfc_step 状态为出站（默认值）
                    Operatetype = ManuSfcStepTypeEnum.OutStock,
                    CurrentStatus = currentStatus,
                    AfterOperationStatus = sfcProduceEntity.Status,
                    Id = IdGenProvider.Instance.CreateId(),
                    SFC = sfcProduceEntity.SFC,
                    ProductId = sfcProduceEntity.ProductId,
                    WorkOrderId = sfcProduceEntity.WorkOrderId,
                    WorkCenterId = sfcProduceEntity.WorkCenterId,
                    ProductBOMId = sfcProduceEntity.ProductBOMId,
                    ProcessRouteId= sfcProduceEntity.ProcessRouteId,
                    SFCInfoId = sfcProduceEntity.BarCodeInfoId,
                    Qty = sfcProduceEntity.Qty,
                    VehicleCode = requestBo.VehicleCode,
                    Remark = "SmiFinishedLastProcedureOutStation",
                    ProcedureId = commonBo.ProcedureId,
                    ResourceId = commonBo.ResourceId,
                    EquipmentId = commonBo.EquipmentId,
                    OperationProcedureId = commonBo.ProcedureId,
                    OperationResourceId = commonBo.ResourceId,
                    OperationEquipmentId = commonBo.EquipmentId,
                    SiteId = commonBo.SiteId,
                    CreatedBy = commonBo.UserName,
                    CreatedOn = commonBo.Time,
                    UpdatedBy = commonBo.UserName,
                    UpdatedOn = commonBo.Time
                };

                // 如果超过复投次数
                if (sfcProduceEntity.RepeatedCount >= cycle)
                {
                    // 不合格复投的话，改工序未当前工序
                    sfcProduceEntity.ProcedureId = commonBo.ProcedureId;
                    sfcProduceEntity.ResourceId = commonBo.ResourceId;

                    // 清空复投次数
                    sfcProduceEntity.RepeatedCount = 0;

                    // 标记条码为"在制-完成"
                    manuSfcEntity.Status = SfcStatusEnum.InProductionComplete;
                    sfcProduceEntity.Status = SfcStatusEnum.InProductionComplete;
                }
                else
                {
                    deleteSFCProduceIds.Add(sfcProduceEntity.Id);

                    // 新增 wh_material_inventory
                    responseSummaryBo.WhMaterialInventoryEntities.Add(new WhMaterialInventoryEntity
                    {
                        Id = IdGenProvider.Instance.CreateId(),
                        SupplierId = 0,//自制品 没有
                        MaterialId = sfcProduceEntity.ProductId,
                        MaterialBarCode = sfcProduceEntity.SFC,
                        //Batch = "",//自制品 没有
                        WorkOrderId = sfcProduceEntity.WorkOrderId,
                        MaterialType = MaterialInventoryMaterialTypeEnum.SelfMadeParts,
                        QuantityResidue = sfcProduceEntity.Qty,
                        ScrapQty = sfcProduceEntity.ScrapQty,
                        Status = WhMaterialInventoryStatusEnum.ToBeUsed,
                        Source = MaterialInventorySourceEnum.ManuComplete,
                        SiteId = commonBo.SiteId,
                        CreatedBy = commonBo.UserName,
                        CreatedOn = commonBo.Time,
                        UpdatedBy = commonBo.UserName,
                        UpdatedOn = commonBo.Time
                    });

                    // 新增 wh_material_standingbook
                    responseSummaryBo.WhMaterialStandingbookEntities.Add(new WhMaterialStandingbookEntity
                    {
                        Id = IdGenProvider.Instance.CreateId(),
                        MaterialCode = procMaterialEntity.MaterialCode,
                        MaterialName = procMaterialEntity.MaterialName,
                        MaterialVersion = procMaterialEntity.Version ?? "",
                        MaterialBarCode = sfcProduceEntity.SFC,
                        //Batch = "",//自制品 没有
                        Quantity = sfcProduceEntity.Qty,
                        Unit = procMaterialEntity.Unit ?? "",
                        Type = WhMaterialInventoryTypeEnum.ManuComplete,
                        Source = MaterialInventorySourceEnum.ManuComplete,
                        SiteId = commonBo.SiteId,
                        CreatedBy = commonBo.UserName,
                        CreatedOn = commonBo.Time,
                        UpdatedBy = commonBo.UserName,
                        UpdatedOn = commonBo.Time
                    });
                }

                responseSummaryBo.SFCEntities.Add(manuSfcEntity);
                responseSummaryBo.SFCProduceEntities.Add(sfcProduceEntity);
                responseSummaryBo.SFCStepEntities.Add(stepEntity);
            }

            // 删除 manu_sfc_produce_business
            responseSummaryBo.DeleteSFCProduceBusinesssByIdsCommand = new DeleteSFCProduceBusinesssByIdsCommand
            {
                SiteId = commonBo.SiteId,
                SfcInfoIds = deleteSFCProduceIds
            };

            // 物理删除 manu_sfc_produce
            responseSummaryBo.PhysicalDeleteSFCProduceByIdsCommand = new PhysicalDeleteSFCProduceByIdsCommand
            {
                SiteId = commonBo.SiteId,
                Ids = deleteSFCProduceIds
            };

            return responseSummaryBo;
        }

        /// <summary>
        /// 执行入库
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public async Task<JobResponseBo?> ExecuteAsync(object obj)
        {
            JobResponseBo responseBo = new();
            if (obj is not SmiFinisheResponseSummaryBo data) return responseBo;

            // 更新条码状态
            if (data.SFCEntities.Any())
            {
                // manu_sfc 修改为完成 且入库
                responseBo.Rows += await _manuSfcRepository.UpdateRangeWithStatusCheckAsync(data.SFCEntities);

                // 未更新到全部需更新的数据，事务回滚
                if (data.SFCEntities.Count > responseBo.Rows)
                {
                    responseBo.IsSuccess = false;
                    responseBo.Message = _localizationService.GetResource(nameof(ErrorCode.MES18216), string.Join(',', data.SFCEntities!.Select(s => s.SFC)));
                    return responseBo;
                }
            }

            // 更新数据
            List<Task<int>> tasks = new()
            {
                // 修改 manu_sfc_produce
                _manuSfcProduceRepository.UpdateRangeWithStatusCheckAsync(data.SFCProduceEntities),

                // 删除 manu_sfc_produce
                _manuSfcProduceRepository.DeletePhysicalRangeByIdsAsync(data.PhysicalDeleteSFCProduceByIdsCommand),

                // 删除 manu_sfc_produce_business
                _manuSfcProduceRepository.DeleteSfcProduceBusinessBySfcInfoIdsAsync(data.DeleteSFCProduceBusinesssByIdsCommand),

                // 插入 manu_sfc_step
                _manuSfcStepRepository.InsertRangeAsync(data.SFCStepEntities),

                // 入库
                _whMaterialInventoryRepository.InsertsAsync(data.WhMaterialInventoryEntities),
                _whMaterialStandingbookRepository.InsertsAsync(data.WhMaterialStandingbookEntities)
            };

            var rowArray = await Task.WhenAll(tasks);
            responseBo.Rows += rowArray.Sum();

            return responseBo;
        }

        /// <summary>
        /// 执行后节点
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<IEnumerable<JobBo>?> AfterExecuteAsync<T>(T param) where T : JobBaseBo
        {
            return await Task.FromResult<IEnumerable<JobBo>?>(default);
        }

    }
}
