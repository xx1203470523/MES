using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Integrated;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Integrated.Command;
using Hymson.MES.Data.Repositories.Integrated.Query;

namespace Hymson.MES.Data.Repositories.Integrated
{
    /// <summary>
    /// 仓储接口（字段国际化）
    /// </summary>
    public interface IInteCustomFieldInternationalizationRepository
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<int> InsertAsync(InteCustomFieldInternationalizationEntity entity);
        
        /// <summary>
        /// 新增（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        Task<int> InsertRangeAsync(IEnumerable<InteCustomFieldInternationalizationEntity> entities);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(InteCustomFieldInternationalizationEntity entity);
        
        /// <summary>
        /// 更新（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        Task<int> UpdateRangeAsync(IEnumerable<InteCustomFieldInternationalizationEntity> entities);

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
        Task<InteCustomFieldInternationalizationEntity> GetByIdAsync(long id);
    
        /// <summary>
        /// 根据IDs获取数据（批量）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<IEnumerable<InteCustomFieldInternationalizationEntity>> GetByIdsAsync(long[] ids);

        /// <summary>
        /// 获取List
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<IEnumerable<InteCustomFieldInternationalizationEntity>> GetEntitiesAsync(InteCustomFieldInternationalizationQuery query);
        
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pagedQuery"></param>
        /// <returns></returns>
        Task<PagedInfo<InteCustomFieldInternationalizationEntity>> GetPagedListAsync(InteCustomFieldInternationalizationPagedQuery pagedQuery);

        /// <summary>
        /// 批量根据字段ID获取字段语言设置信息
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<IEnumerable<InteCustomFieldInternationalizationEntity>> GetEntitiesByCustomFieldIdsAsync(InteCustomFieldInternationalizationByCustomFieldIdsQuery query);

        /// <summary>
        /// 根据自定义字段IDs 批量硬删除
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        Task<int> DeletedByCustomFieldIdsAsync(InternationalizationDeleteByCustomFieldIdsCommand command);
    }
}
