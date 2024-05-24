using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Quality;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Quality.Query;

namespace Hymson.MES.Data.Repositories.Quality
{
    /// <summary>
    /// 仓储接口（FQC检验参数组明细）
    /// </summary>
    public interface IQualFqcParameterGroupDetailRepository
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<int> InsertAsync(QualFqcParameterGroupDetailEntity entity);
        
        /// <summary>
        /// 新增（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        Task<int> InsertRangeAsync(IEnumerable<QualFqcParameterGroupDetailEntity> entities);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(QualFqcParameterGroupDetailEntity entity);
        
        /// <summary>
        /// 更新（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        Task<int> UpdateRangeAsync(IEnumerable<QualFqcParameterGroupDetailEntity> entities);

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
        /// 根据主表Id删除数据
        /// </summary>
        /// <param name="ParameterGroupId"></param>
        /// <returns></returns>
        Task<int> DeleteByMainIdAsync(long ParameterGroupId);

        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<QualFqcParameterGroupDetailEntity> GetByIdAsync(long id);
    
        /// <summary>
        /// 根据IDs获取数据（批量）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<IEnumerable<QualFqcParameterGroupDetailEntity>> GetByIdsAsync(long[] ids);

        /// <summary>
        /// 查询单个实体
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<QualFqcParameterGroupDetailEntity> GetEntityAsync(QualFqcParameterGroupDetailQuery query);

        /// <summary>
        /// 获取List
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<IEnumerable<QualFqcParameterGroupDetailEntity>> GetEntitiesAsync(QualFqcParameterGroupDetailQuery query);
        
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pagedQuery"></param>
        /// <returns></returns>
        Task<PagedInfo<QualFqcParameterGroupDetailEntity>> GetPagedListAsync(QualFqcParameterGroupDetailPagedQuery pagedQuery);

    }
}
