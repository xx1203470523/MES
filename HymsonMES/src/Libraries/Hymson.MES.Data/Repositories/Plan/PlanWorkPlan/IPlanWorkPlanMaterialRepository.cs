using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Plan;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Plan.Query;

namespace Hymson.MES.Data.Repositories.Plan
{
    /// <summary>
    /// 仓储接口（生产计划物料表）
    /// </summary>
    public interface IPlanWorkPlanMaterialRepository
    {
        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        Task<int> InsertsAsync(IEnumerable<PlanWorkPlanMaterialEntity> entities);

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<int> DeletesAsync(DeleteCommand param);

        /// <summary>
        /// 删除（批量）
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        Task<int> DeleteByParentIdsAsync(DeleteByParentIdsCommand command);

        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<PlanWorkPlanMaterialEntity> GetByIdAsync(long id);

        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<IEnumerable<PlanWorkPlanMaterialEntity>> GetByIdsAsync(IEnumerable<long> ids);

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pageQuery"></param>
        /// <returns></returns>
        Task<PagedInfo<PlanWorkPlanMaterialEntity>> GetPagedInfoAsync(PlanWorkPlanPagedQuery pageQuery);

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<IEnumerable<PlanWorkPlanMaterialEntity>> GetEntitiesAsync(PlanWorkPlanQuery query);
        Task<IEnumerable<PlanWorkPlanMaterialEntity>> GetEntitiesByPlanIdAsync(PlanWorkPlanByPlanIdQuery query);

    }
}
