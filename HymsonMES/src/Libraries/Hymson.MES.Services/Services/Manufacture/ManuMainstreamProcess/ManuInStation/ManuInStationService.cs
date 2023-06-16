using Hymson.Authentication;
using Hymson.Infrastructure.Exceptions;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Manufacture;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Data.Repositories.Plan;
using Hymson.MES.Data.Repositories.Plan.PlanWorkOrder.Command;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Services.Services.Manufacture.ManuMainstreamProcess.ManuCommon;
using Hymson.Snowflake;
using Hymson.Utils;
using Hymson.Utils.Tools;

namespace Hymson.MES.Services.Services.Manufacture.ManuMainstreamProcess.ManuInStation
{
    /// <summary>
    /// 进站
    /// </summary>
    public class ManuInStationService : IManuInStationService
    {
        /// <summary>
        /// 当前对象（登录用户）
        /// </summary>
        private readonly ICurrentUser _currentUser;

        /// <summary>
        /// 服务接口（生产通用）
        /// </summary>
        private readonly IManuCommonOldService _manuCommonOldService;

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
        /// 仓储接口（工单信息）
        /// </summary>
        private readonly IPlanWorkOrderRepository _planWorkOrderRepository;

        /// <summary>
        /// 仓储接口（工序维护）
        /// </summary>
        private readonly IProcProcedureRepository _procProcedureRepository;


        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="currentUser"></param>
        /// <param name="manuCommonOldService"></param>
        /// <param name="manuSfcRepository"></param>
        /// <param name="manuSfcStepRepository"></param>
        /// <param name="manuSfcProduceRepository"></param>
        /// <param name="planWorkOrderRepository"></param>
        /// <param name="procProcedureRepository"></param>
        public ManuInStationService(ICurrentUser currentUser,
            IManuCommonOldService manuCommonOldService,
            IManuSfcRepository manuSfcRepository,
            IManuSfcStepRepository manuSfcStepRepository,
            IManuSfcProduceRepository manuSfcProduceRepository,
             IPlanWorkOrderRepository planWorkOrderRepository,
            IProcProcedureRepository procProcedureRepository)
        {
            _currentUser = currentUser;
            _manuCommonOldService = manuCommonOldService;
            _manuSfcRepository = manuSfcRepository;
            _manuSfcStepRepository = manuSfcStepRepository;
            _manuSfcProduceRepository = manuSfcProduceRepository;
            _planWorkOrderRepository = planWorkOrderRepository;
            _procProcedureRepository = procProcedureRepository;
        }


        /// <summary>
        /// 执行（进站）
        /// </summary>
        /// <param name="sfcProduceEntity"></param>
        /// <returns></returns>
        public async Task<int> InStationAsync(ManuSfcProduceEntity sfcProduceEntity)
        {
            var rows = 0;

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

            // 更新数据
            using (var trans = TransactionHelper.GetTransactionScope())
            {
                /*
                 * 目前条码好像默认就是已使用，所以这里不需要再更新条码状态
                // 修改条码使用状态为"已使用"
                rows = await _manuSfcRepository.UpdateSfcIsUsedAsync(new ManuSfcUpdateIsUsedCommand
                {
                    Sfcs = new string[] { sfcProduceEntity.SFC },
                    UserId = sfcProduceEntity.UpdatedBy,
                    UpdatedOn = sfcProduceEntity.UpdatedOn,
                    IsUsed = YesOrNoEnum.Yes
                });
                */

                // 更改状态
                rows = await _manuSfcProduceRepository.UpdateWithStatusCheckAsync(sfcProduceEntity);

                // 未更新到数据，事务回滚
                if (rows <= 0)
                {
                    trans.Dispose();
                    return rows;
                }

                // 更新工单统计表的 RealStart
                rows += await _planWorkOrderRepository.UpdatePlanWorkOrderRealStartByWorkOrderIdAsync(new UpdateWorkOrderRealTimeCommand
                {
                    UpdatedOn = sfcProduceEntity.UpdatedOn,
                    UpdatedBy = sfcProduceEntity.UpdatedBy,
                    WorkOrderIds = new long[] { sfcProduceEntity.WorkOrderId }
                });

                // 插入 manu_sfc_step 状态为 进站
                sfcStep.Operatetype = ManuSfcStepTypeEnum.InStock;
                rows += await _manuSfcStepRepository.InsertAsync(sfcStep);

                // 如果是首工序，更新工单的 InputQty
                if (isFirstProcedure == true)
                {
                    rows += await _planWorkOrderRepository.UpdateInputQtyByWorkOrderId(new UpdateQtyCommand
                    {
                        UpdatedBy = sfcProduceEntity.UpdatedBy,
                        UpdatedOn = sfcProduceEntity.UpdatedOn,
                        WorkOrderId = sfcProduceEntity.WorkOrderId,
                        Qty = 1,
                    });
                }

                trans.Complete();
            }

            return rows;
        }
    }
}
