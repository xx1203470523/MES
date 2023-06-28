using Hymson.MES.Core.Attribute.Job;
using Hymson.MES.Core.Enums.Job;
using Hymson.MES.CoreServices.Bos.Job;
using Hymson.MES.CoreServices.Services.Common.ManuCommon;
using Hymson.MES.CoreServices.Services.Job;

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
        /// 构造函数
        /// </summary>
        /// <param name="manuCommonService"></param>
        public InStationJobService(IManuCommonService manuCommonService)
        {
            _manuCommonService = manuCommonService;
        }

        /// <summary>
        /// 参数校验
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task VerifyParamAsync<T>(T param) where T : JobBaseBo
        {
            if ((param is InStationRequestBo bo) == false) return;

            await Task.CompletedTask;
        }

        /// <summary>
        /// 数据组装
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<TResult> DataAssemblingAsync<T, TResult>(T param) where T : JobBaseBo where TResult : JobResultBo, new()
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
