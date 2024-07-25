using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Plan;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Plan.Query;

namespace Hymson.MES.Data.Repositories.Plan
{
    /// <summary>
    /// 仓储接口（生产计划表）
    /// </summary>
    public interface IPlanWorkPlanRepository
    {
        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        Task<int> InsertsAsync(IEnumerable<PlanWorkPlanEntity> entities);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(PlanWorkPlanEntity entity);

        /// <summary>
        /// 批量更新 
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        Task<int> UpdatesAsync(IEnumerable<PlanWorkPlanEntity> entities);

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<int> DeletesAsync(DeleteCommand param);

        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<PlanWorkPlanEntity> GetByIdAsync(long id);

        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<IEnumerable<PlanWorkPlanEntity>> GetByIdsAsync(IEnumerable<long> ids);

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pageQuery"></param>
        /// <returns></returns>
        Task<PagedInfo<PlanWorkPlanEntity>> GetPagedInfoAsync(PlanWorkPlanPagedQuery pageQuery);

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<IEnumerable<PlanWorkPlanEntity>> GetEntitiesAsync(PlanWorkPlanQuery query);

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<IEnumerable<PlanWorkPlanEntity>> GetEntitiesAsync(PlanWorkPlanProductPagedQuery query);

    }
}
