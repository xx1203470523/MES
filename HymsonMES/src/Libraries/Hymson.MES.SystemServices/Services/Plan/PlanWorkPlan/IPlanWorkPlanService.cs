using Hymson.MES.SystemServices.Dtos;

namespace Hymson.MES.SystemServices.Services.Plan
{
    /// <summary>
    /// 服务接口（生产计划）
    /// </summary>
    public interface IPlanWorkPlanService
    {
        /// <summary>
        /// 同步信息（生产计划）
        /// </summary>
        /// <param name="requestDtos"></param>
        /// <returns></returns>
        Task<int> SyncWorkPlanAsync(IEnumerable<WorkPlanDto> requestDtos);
        /// <summary>
        /// 同步工单列表
        /// </summary>
        /// <param name="WorkCenterId">工作中心</param>
        /// <returns></returns>
        Task<IEnumerable<RotorWorkOrder>> SyncWorkOrderAsync(long WorkCenterId);

    }
}
