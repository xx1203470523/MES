using Hymson.MES.SystemServices.Dtos.Plan;

namespace Hymson.MES.SystemServices.Services.Plan
{
    /// <summary>
    /// 
    /// </summary>
    public interface IPlanWorkOrderService
    {
        /// <summary>
        /// 添加工单 整厂MES创建工单
        /// 目前对方只要求一个工单号字段，不合理后续可能修改
        /// </summary>
        /// <param name="planWorkOrderDto"></param>
        /// <returns></returns>
        Task AddWorkOrderAsync(PlanWorkOrderDto planWorkOrderDto);
    }
}
