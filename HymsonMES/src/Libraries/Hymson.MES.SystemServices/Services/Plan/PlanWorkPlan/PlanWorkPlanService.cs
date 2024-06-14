using Hymson.MES.SystemServices.Dtos;

namespace Hymson.MES.SystemServices.Services.Plan
{
    /// <summary>
    /// 服务（生产计划）
    /// </summary>
    public class PlanWorkPlanService : IPlanWorkPlanService
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public PlanWorkPlanService()
        {
            // TODO 
        }

        /// <summary>
        /// 生产计划（同步）
        /// </summary>
        /// <param name="requestDtos"></param>
        /// <returns></returns>
        public async Task<int> SyncWorkOrderAsync(IEnumerable<WorkPlanDto> requestDtos)
        {
            // TODO 
            return await Task.FromResult(0);
        }

    }
}
