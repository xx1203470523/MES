using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Integrated;
using Hymson.MES.Core.Enums.Integrated;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Integrated.Query;

namespace Hymson.MES.Data.Repositories.Integrated
{
    /// <summary>
    /// 仓储接口（自定字段业务实现）
    /// </summary>
    public interface IInteCustomFieldBusinessEffectuateRepository
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<int> InsertAsync(InteCustomFieldBusinessEffectuateEntity entity);
        
        /// <summary>
        /// 新增（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        Task<int> InsertRangeAsync(IEnumerable<InteCustomFieldBusinessEffectuateEntity> entities);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(InteCustomFieldBusinessEffectuateEntity entity);
        
        /// <summary>
        /// 更新（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        Task<int> UpdateRangeAsync(IEnumerable<InteCustomFieldBusinessEffectuateEntity> entities);

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
        Task<InteCustomFieldBusinessEffectuateEntity> GetByIdAsync(long id);
    
        /// <summary>
        /// 根据IDs获取数据（批量）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<IEnumerable<InteCustomFieldBusinessEffectuateEntity>> GetByIdsAsync(long[] ids);

        /// <summary>
        /// 获取List
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<IEnumerable<InteCustomFieldBusinessEffectuateEntity>> GetEntitiesAsync(InteCustomFieldBusinessEffectuateQuery query);
        
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pagedQuery"></param>
        /// <returns></returns>
        Task<PagedInfo<InteCustomFieldBusinessEffectuateEntity>> GetPagedListAsync(InteCustomFieldBusinessEffectuatePagedQuery pagedQuery);

        /// <summary>
        /// 根据业务ID硬删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<int> DeleteTrueByBusinessIdAsync(long businessId);

        /// <summary>
        /// 通过业务ID获取业务数据
        /// </summary>
        /// <param name="businessId"></param>
        /// <returns></returns>
        Task<IEnumerable<InteCustomFieldBusinessEffectuateEntity>> GetBusinessEffectuatesByBusinessIdAsync(long businessId);

        /// <summary>
        /// 获取自定义字段值
        /// </summary>
        /// <param name="businessId"></param>
        /// <param name="businessType"></param>
        /// <param name="customFieldName"></param>
        /// <returns></returns>
        Task<string?> GetCustomeFieldValue(long businessId, InteCustomFieldBusinessTypeEnum businessType, string customFieldName);

        /// <summary>
        /// 根据业务ID硬删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<int> DeleteTrueByBusinessIdsAsync(long[] ids);
    }
}
