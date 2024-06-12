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
        /// 创建
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<long> CreateAsync(WorkPlanDto requestDto)
        {
            // TODO 
            return await Task.FromResult(0);
        }

    }
}
