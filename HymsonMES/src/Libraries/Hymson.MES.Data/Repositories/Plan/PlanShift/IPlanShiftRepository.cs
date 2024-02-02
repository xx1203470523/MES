using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Plan;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Plan.Query;

namespace Hymson.MES.Data.Repositories.Plan
{
    /// <summary>
    /// 仓储接口（班制）
    /// </summary>
    public interface IPlanShiftRepository
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<int> InsertAsync(PlanShiftEntity entity);

        /// <summary>
        /// 添加明细数据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<int> InsertDetailAsync(List<PlanShiftDetailEntity> entity);

        /// <summary>
        /// 新增（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        Task<int> InsertRangeAsync(IEnumerable<PlanShiftEntity> entities);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(PlanShiftEntity entity);
        
        /// <summary>
        /// 更新（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        Task<int> UpdateRangeAsync(IEnumerable<PlanShiftEntity> entities);

        /// <summary>
        /// 软删除  
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<int> DeleteAsync(long id);
        
        /// <summary>
        /// 软删除（批量）
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        Task<int> DeletesAsync(DeleteCommand command);


        /// <summary>
        /// 删除明细表Id
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<int> DeletesDetailByIdAsync(long[] ids);

        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<PlanShiftEntity> GetByIdAsync(long id);

        /// <summary>
        /// 根据主Id查详细
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<IEnumerable<PlanShiftDetailEntity>> GetByMainIdAsync(long id);
    
        /// <summary>
        /// 根据IDs获取数据（批量）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<IEnumerable<PlanShiftEntity>> GetByIdsAsync(long[] ids);

        /// <summary>
        /// 获取List
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<IEnumerable<PlanShiftEntity>> GetEntitiesAsync(PlanShiftQuery query);
        
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pagedQuery"></param>
        /// <returns></returns>
        Task<PagedInfo<PlanShiftEntity>> GetPagedListAsync(PlanShiftPagedQuery pagedQuery);
        Task<IEnumerable<PlanShiftEntity>> GetAllAsync(PlanShiftQuery query);
    }
}
