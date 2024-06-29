using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Plan;

namespace Hymson.MES.Services.Services.Plan
{
    /// <summary>
    /// 服务接口（生产计划）
    /// </summary>
    public interface IPlanWorkPlanService
    {
        /// <summary>
        /// 生成子工单
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<int> SaveAsync(PlanWorkPlanSaveDto dto);

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="idsArr"></param>
        /// <returns></returns>
        Task<int> DeletesAsync(long[] idsArr);

        /// <summary>
        /// 根据查询条件获取分页数据
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        Task<PagedInfo<PlanWorkPlanProductDto>> GetPageListAsync(PlanWorkPlanProductPagedQueryDto pagedQueryDto);

        /// <summary>
        /// 根据planProductId查询
        /// </summary>
        /// <param name="planProductId"></param>
        /// <returns></returns>
        Task<PlanWorkPlanProductDto?> QueryByIdAsync(long planProductId);

        /// <summary>
        /// 根据planProductId查询
        /// </summary>
        /// <param name="planProductId"></param>
        /// <returns></returns>
        Task<IEnumerable<PlanWorkPlanMaterialDto>?> QueryMaterialsByMainIdAsync(long planProductId);


    }
}   
