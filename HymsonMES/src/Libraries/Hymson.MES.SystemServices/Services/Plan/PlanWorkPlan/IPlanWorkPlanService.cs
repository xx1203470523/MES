using Hymson.MES.SystemServices.Dtos;

namespace Hymson.MES.SystemServices.Services.Plan
{
    /// <summary>
    /// 服务接口（生产计划）
    /// </summary>
    public interface IPlanWorkPlanService
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        Task<long> CreateAsync(WorkPlanDto requestDto);

    }
}
