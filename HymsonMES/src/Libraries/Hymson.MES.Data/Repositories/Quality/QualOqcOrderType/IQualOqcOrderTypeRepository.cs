using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Quality;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Quality.Query;

namespace Hymson.MES.Data.Repositories.Quality
{
    /// <summary>
    /// 仓储接口（OQC检验类型）
    /// </summary>
    public interface IQualOqcOrderTypeRepository
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<int> InsertAsync(QualOqcOrderTypeEntity entity);

        /// <summary>
        /// 新增（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        Task<int> InsertRangeAsync(IEnumerable<QualOqcOrderTypeEntity> entities);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(QualOqcOrderTypeEntity entity);

        /// <summary>
        /// 更新（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        Task<int> UpdateRangeAsync(IEnumerable<QualOqcOrderTypeEntity> entities);

        /// <summary>
        /// 更新已检数
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<int> UpdateCheckedQtyAsync(QualOqcOrderTypeEntity entity);

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
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<QualOqcOrderTypeEntity> GetByIdAsync(long id);

        /// <summary>
        /// 根据IDs获取数据（批量）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<IEnumerable<QualOqcOrderTypeEntity>> GetByIdsAsync(long[] ids);

        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        Task<IEnumerable<QualOqcOrderTypeEntity>> GetByOQCOrderIdAsync(long orderId);

        /// <summary>
        /// 获取List
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<IEnumerable<QualOqcOrderTypeEntity>> GetEntitiesAsync(QualOqcOrderTypeQuery query);

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pagedQuery"></param>
        /// <returns></returns>
        Task<PagedInfo<QualOqcOrderTypeEntity>> GetPagedListAsync(QualOqcOrderTypePagedQuery pagedQuery);

        /// <summary>
        /// 查询Entity
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<QualOqcOrderTypeEntity> GetEntityAsync(QualOqcOrderTypeQuery query);
    }
}
