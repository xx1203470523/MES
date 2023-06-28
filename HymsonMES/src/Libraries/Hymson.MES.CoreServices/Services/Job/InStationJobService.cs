using Hymson.Infrastructure.Exceptions;
using Hymson.MES.Core.Attribute.Job;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Job;
using Hymson.MES.CoreServices.Bos.Job;
using Hymson.MES.CoreServices.Services.Common.ManuCommon;
using Hymson.MES.CoreServices.Services.Common.ManuExtension;
using Hymson.MES.CoreServices.Services.Job;
using Hymson.MES.Data.Repositories.Process;

namespace Hymson.MES.CoreServices.Services.NewJob
{
    /// <summary>
    /// 进站
    /// </summary>
    [Job("进站", JobTypeEnum.Standard)]
    public class InStationJobService : IJobService
    {
        /// <summary>
        /// 服务接口（生产通用）
        /// </summary>
        private readonly IManuCommonService _manuCommonService;

        /// <summary>
        /// 仓储接口（工艺路线工序节点）
        /// </summary>
        private readonly IProcProcessRouteDetailNodeRepository _procProcessRouteDetailNodeRepository;

        /// <summary>
        /// 仓储接口（工艺路线工序连线）
        /// </summary>
        private readonly IProcProcessRouteDetailLinkRepository _procProcessRouteDetailLinkRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="manuCommonService"></param>
        /// <param name="procProcessRouteDetailNodeRepository"></param>
        /// <param name="procProcessRouteDetailLinkRepository"></param>
        public InStationJobService(IManuCommonService manuCommonService,
            IProcProcessRouteDetailNodeRepository procProcessRouteDetailNodeRepository,
            IProcProcessRouteDetailLinkRepository procProcessRouteDetailLinkRepository)
        {
            _manuCommonService = manuCommonService;
            _procProcessRouteDetailNodeRepository = procProcessRouteDetailNodeRepository;
            _procProcessRouteDetailLinkRepository = procProcessRouteDetailLinkRepository;
        }

        /// <summary>
        /// 参数校验
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task VerifyParamAsync<T>(T param) where T : JobBaseBo
        {
            if ((param is InStationRequestBo bo) == false) return;

            // 校验工序和资源是否对应
            var resourceIds = await param.Proxy.GetValueAsync(_manuCommonService.GetProcResourceIdByProcedureIdAsync, bo.ProcedureId);
            if (resourceIds == null || resourceIds.Any(a => a == bo.ResourceId) == false) throw new CustomerValidationException(nameof(ErrorCode.MES16317));

            // 获取生产条码信息
            var sfcProduceEntities = await param.Proxy.GetValueAsync(_manuCommonService.GetProduceEntitiesBySFCsAsync, bo);
            if (sfcProduceEntities == null || sfcProduceEntities.Any() == false) return;

            var sfcProduceBusinessEntities = await param.Proxy.GetValueAsync(_manuCommonService.GetProduceBusinessEntitiesBySFCsAsync, bo);

            // 合法性校验
            sfcProduceEntities.VerifySFCStatus(SfcProduceStatusEnum.lineUp);
            sfcProduceBusinessEntities?.VerifyProcedureLock(bo.SFCs, bo.ProcedureId);

            // 验证条码是否被容器包装
            await _manuCommonService.VerifyContainerAsync(bo);

            // 如果工序对应不上
            var firstProduceEntity = sfcProduceEntities.FirstOrDefault();
            if (firstProduceEntity == null) return;

            if (firstProduceEntity.ProcedureId != bo.ProcedureId)
            {
                var processRouteDetailLinks = await _procProcessRouteDetailLinkRepository.GetProcessRouteDetailLinksByProcessRouteIdAsync(firstProduceEntity.ProcessRouteId)
                    ?? throw new CustomerValidationException(nameof(ErrorCode.MES18213));

                var processRouteDetailNodes = await _procProcessRouteDetailNodeRepository.GetProcessRouteDetailNodesByProcessRouteIdAsync(firstProduceEntity.ProcessRouteId)
                    ?? throw new CustomerValidationException(nameof(ErrorCode.MES18208));

                // 判断上一个工序是否是随机工序
                var IsRandomPreProcedure = await _manuCommonService.IsRandomPreProcedureAsync(processRouteDetailLinks, processRouteDetailNodes, firstProduceEntity.ProcessRouteId, bo.ProcedureId);
                if (IsRandomPreProcedure == false) throw new CustomerValidationException(nameof(ErrorCode.MES16308));
            }
        }

        /// <summary>
        /// 数据组装
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<TResult> DataAssemblingAsync<T, TResult>(T param) where T : JobBaseBo where TResult : JobBaseBo, new()
        {
            await Task.CompletedTask;

            /*
            // 进站
            sfcProduceEntity.ResourceId = bo.ResourceId;

            // 更新状态，将条码由"排队"改为"活动"
            sfcProduceEntity.Status = SfcProduceStatusEnum.Activity;
            sfcProduceEntity.UpdatedBy = _currentUser.UserName;
            sfcProduceEntity.UpdatedOn = HymsonClock.Now();

            // 获取生产工单（附带工单状态校验）
            _ = await _manuCommonOldService.GetProduceWorkOrderByIdAsync(sfcProduceEntity.WorkOrderId);

            // 获取当前工序信息
            var procedureEntity = await _procProcedureRepository.GetByIdAsync(sfcProduceEntity.ProcedureId);

            // 检查是否测试工序
            if (procedureEntity.Type == ProcedureTypeEnum.Test)
            {
                // 超过复投次数，标识为NG
                if (sfcProduceEntity.RepeatedCount > procedureEntity.Cycle) throw new CustomerValidationException(nameof(ErrorCode.MES16036));
                sfcProduceEntity.RepeatedCount++;
            }

            // 检查是否首工序
            var isFirstProcedure = await _manuCommonOldService.IsFirstProcedureAsync(sfcProduceEntity.ProcessRouteId, sfcProduceEntity.ProcedureId);

            // 初始化步骤
            var sfcStep = new ManuSfcStepEntity
            {
                Id = IdGenProvider.Instance.CreateId(),
                SiteId = sfcProduceEntity.SiteId,
                SFC = sfcProduceEntity.SFC,
                ProductId = sfcProduceEntity.ProductId,
                WorkOrderId = sfcProduceEntity.WorkOrderId,
                WorkCenterId = sfcProduceEntity.WorkCenterId,
                ProductBOMId = sfcProduceEntity.ProductBOMId,
                ProcedureId = sfcProduceEntity.ProcedureId,
                Qty = sfcProduceEntity.Qty,
                EquipmentId = sfcProduceEntity.EquipmentId,
                ResourceId = sfcProduceEntity.ResourceId,
                CreatedBy = sfcProduceEntity.UpdatedBy,
                CreatedOn = sfcProduceEntity.UpdatedOn.Value,
                UpdatedBy = sfcProduceEntity.UpdatedBy,
                UpdatedOn = sfcProduceEntity.UpdatedOn.Value,
            };
            */

            return new TResult();
        }

        /// <summary>
        /// 执行入库
        /// </summary>
        /// <returns></returns>
        public async Task ExecuteAsync()
        {
            /*
            using var trans = TransactionHelper.GetTransactionScope();
            List<Task> tasks = new();

            // 更改状态
            rows = await _manuSfcProduceRepository.UpdateWithStatusCheckAsync(sfcProduceEntity);

            // 未更新到数据，事务回滚
            if (rows <= 0)
            {
                trans.Dispose();
                return rows;
            }

            // 更新工单统计表的 RealStart
            var updatePlanWorkOrderRealStartByWorkOrderIdTask = _planWorkOrderRepository.UpdatePlanWorkOrderRealStartByWorkOrderIdAsync(new UpdateWorkOrderRealTimeCommand
            {
                UpdatedOn = sfcProduceEntity.UpdatedOn,
                UpdatedBy = sfcProduceEntity.UpdatedBy,
                WorkOrderIds = new long[] { sfcProduceEntity.WorkOrderId }
            });
            tasks.Add(updatePlanWorkOrderRealStartByWorkOrderIdTask);

            // 插入 manu_sfc_step 状态为 进站
            sfcStep.Operatetype = ManuSfcStepTypeEnum.InStock;
            var manuSfcStepTask = _manuSfcStepRepository.InsertAsync(sfcStep);
            tasks.Add(manuSfcStepTask);

            // 如果是首工序，更新工单的 InputQty
            if (isFirstProcedure == true)
            {
                var updateInputQtyByWorkOrderIdTask = _planWorkOrderRepository.UpdateInputQtyByWorkOrderIdAsync(new UpdateQtyCommand
                {
                    UpdatedBy = sfcProduceEntity.UpdatedBy,
                    UpdatedOn = sfcProduceEntity.UpdatedOn,
                    WorkOrderId = sfcProduceEntity.WorkOrderId,
                    Qty = 1,
                });
                tasks.Add(updateInputQtyByWorkOrderIdTask);
            }

            await Task.WhenAll(tasks);
            trans.Complete();
            */

            await Task.CompletedTask;
        }

    }
}
