/*
 *creator: Karl
 *
 *describe: 工单激活（物理删除）    服务接口 | 代码由框架生成
 *builder:  Karl
 *build datetime: 2023-04-12 11:14:23
 */
using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Plan;

namespace Hymson.MES.Services.Services.Plan
{
    /// <summary>
    /// 工单激活（物理删除） service接口
    /// </summary>
    public interface IPlanWorkOrderBindService
    {
        ///// <summary>
        ///// 获取分页List
        ///// </summary>
        ///// <param name="planWorkOrderBindPagedQueryDto"></param>
        ///// <returns></returns>
        //Task<PagedInfo<PlanWorkOrderBindDto>> GetPagedListAsync(PlanWorkOrderBindPagedQueryDto planWorkOrderBindPagedQueryDto);

        ///// <summary>
        ///// 新增
        ///// </summary>
        ///// <param name="planWorkOrderBindCreateDto"></param>
        ///// <returns></returns>
        //Task CreatePlanWorkOrderBindAsync(PlanWorkOrderBindCreateDto planWorkOrderBindCreateDto);

        ///// <summary>
        ///// 修改
        ///// </summary>
        ///// <param name="planWorkOrderBindModifyDto"></param>
        ///// <returns></returns>
        //Task ModifyPlanWorkOrderBindAsync(PlanWorkOrderBindModifyDto planWorkOrderBindModifyDto);

        ///// <summary>
        ///// 删除
        ///// </summary>
        ///// <param name="id"></param>
        ///// <returns></returns>
        //Task DeletePlanWorkOrderBindAsync(long id);

        ///// <summary>
        ///// 批量删除
        ///// </summary>
        ///// <param name="ids"></param>
        ///// <returns></returns>
        //Task<int> DeletesPlanWorkOrderBindAsync(long[] ids);

        ///// <summary>
        ///// 根据ID查询
        ///// </summary>
        ///// <param name="id"></param>
        ///// <returns></returns>
        //Task<PlanWorkOrderBindDto> QueryPlanWorkOrderBindByIdAsync(long id);

        /// <summary>
        /// 批量绑定激活的工单
        /// </summary>
        /// <param name="bindActivationWorkOrder"></param>
        /// <returns></returns>
        Task BindActivationWorkOrder(BindActivationWorkOrderDto bindActivationWorkOrder);

        /// <summary>
        /// 获取资源id上已经绑定的工单
        /// </summary>
        /// <param name="resourceId"></param>
        /// <returns></returns>
        Task<List<HasBindWorkOrderInfoDto>> GetHasBindWorkOrder(long resourceId);
    }
}
