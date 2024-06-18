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

    }
}