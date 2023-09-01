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
        /// <summary>
        /// 批量绑定激活的工单
        /// </summary>
        /// <param name="bindActivationWorkOrder"></param>
        /// <returns></returns>
        Task BindActivationWorkOrderAsync(BindActivationWorkOrderDto bindActivationWorkOrder);

        /// <summary>
        /// 获取资源id上已经绑定的工单
        /// </summary>
        /// <param name="resourceId"></param>
        /// <returns></returns>
        Task<List<HasBindWorkOrderInfoDto>> GetHasBindWorkOrderAsync(long resourceId);
    }
}
