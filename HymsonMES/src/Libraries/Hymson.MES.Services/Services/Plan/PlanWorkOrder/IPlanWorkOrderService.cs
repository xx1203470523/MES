using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Services.Dtos.Manufacture;
using Hymson.MES.Services.Dtos.Plan;

namespace Hymson.MES.Services.Services.Plan.PlanWorkOrder
{
    /// <summary>
    /// 工单信息表 service接口
    /// </summary>
    public interface IPlanWorkOrderService
    {
        /// <summary>
        /// 获取分页List
        /// </summary>
        /// <param name="planWorkOrderPagedQueryDto"></param>
        /// <returns></returns>
        Task<PagedInfo<PlanWorkOrderListDetailViewDto>> GetPageListAsync(PlanWorkOrderPagedQueryDto planWorkOrderPagedQueryDto);

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="planWorkOrderCreateDto"></param>
        /// <returns></returns>
        Task<long> CreatePlanWorkOrderAsync(PlanWorkOrderCreateDto planWorkOrderCreateDto);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="planWorkOrderModifyDto"></param>
        /// <returns></returns>
        Task ModifyPlanWorkOrderAsync(PlanWorkOrderModifyDto planWorkOrderModifyDto);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task DeletePlanWorkOrderAsync(long id);

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="idsArr"></param>
        /// <returns></returns>
        Task<int> DeletesPlanWorkOrderAsync(long[] idsArr);

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<PlanWorkOrderDetailViewDto> QueryPlanWorkOrderByIdAsync(long id);

        /// <summary>
        /// 查询剩余可下单条码数量
        /// </summary>
        /// <param name="workOrderCode"></param>
        /// <returns></returns>
        Task<decimal> GetPlanWorkOrderByWorkOrderCodeAsync(string workOrderCode);

        Task<List<ManuRequistionOrderEntity>> GetPickHistoryByWorkOrderIdAsync(long workOrderId);

        /// <summary>
        /// 修改工单状态
        /// </summary>
        /// <param name="parms"></param>
        /// <returns></returns>
        Task ModifyWorkOrderStatusAsync(List<PlanWorkOrderChangeStatusDto> parms);

        /// <summary>
        /// 修改工单是否锁定
        /// </summary>
        /// <param name="parms"></param>
        /// <returns></returns>
        Task ModifyWorkOrderLockedAsync(List<PlanWorkOrderLockedDto> parms);

        /// <summary>
        /// 根据工单查询领料明细
        /// </summary>
        /// <param name="workOrderId"></param>
        /// <returns></returns>
        Task<List<ManuRequistionOrderDetailDto>> GetPickDetailByOrderIdAsync(long workOrderId);

    }
}
