using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Warehouse;

namespace Hymson.MES.Data.Repositories.Warehouse
{
    /// <summary>
    /// 物料台账仓储接口
    /// </summary>
    public interface IWhMaterialStandingbookRepository
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="whMaterialStandingbookEntity"></param>
        /// <returns></returns>
        Task<int> InsertAsync(WhMaterialStandingbookEntity whMaterialStandingbookEntity);
        
        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="whMaterialStandingbookEntitys"></param>
        /// <returns></returns>
        Task<int> InsertsAsync(IEnumerable<WhMaterialStandingbookEntity>? whMaterialStandingbookEntitys);

        /// <summary>
        /// 批量新增(拼接字符串方式,解决插入速度太慢问题)
        /// </summary>
        /// <param name="whMaterialStandingbookEntitys"></param>
        /// <returns></returns>
        Task<int> InsertRangeByConcatSqlAsync(IEnumerable<WhMaterialStandingbookEntity> whMaterialStandingbookEntitys);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="whMaterialStandingbookEntity"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(WhMaterialStandingbookEntity whMaterialStandingbookEntity);
        
        /// <summary>
        /// 批量更新 
        /// </summary>
        /// <param name="whMaterialStandingbookEntitys"></param>
        /// <returns></returns>
        Task<int> UpdatesAsync(List<WhMaterialStandingbookEntity> whMaterialStandingbookEntitys);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<int> DeleteAsync(long id);
        
        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<int> DeletesAsync(long[] ids);

        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<WhMaterialStandingbookEntity> GetByIdAsync(long id);
    
        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<IEnumerable<WhMaterialStandingbookEntity>> GetByIdsAsync(long[] ids);

        /// <summary>
        /// 获取List
        /// </summary>
        /// <param name="whMaterialStandingbookQuery"></param>
        /// <returns></returns>
        Task<IEnumerable<WhMaterialStandingbookEntity>> GetWhMaterialStandingbookEntitiesAsync(WhMaterialStandingbookQuery whMaterialStandingbookQuery);
        
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="whMaterialStandingbookPagedQuery"></param>
        /// <returns></returns>
        Task<PagedInfo<WhMaterialStandingbookEntity>> GetPagedInfoAsync(WhMaterialStandingbookPagedQuery whMaterialStandingbookPagedQuery);
    }
}
