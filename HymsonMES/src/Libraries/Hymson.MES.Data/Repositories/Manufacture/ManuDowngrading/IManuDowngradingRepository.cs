/*
 *creator: Karl
 *
 *describe: 降级录入仓储类 | 代码由框架生成
 *builder:  Karl
 *build datetime: 2023-08-10 10:15:26
 */
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Data.Repositories.Common.Command;

namespace Hymson.MES.Data.Repositories.Manufacture
{
    /// <summary>
    /// 降级录入仓储接口
    /// </summary>
    public interface IManuDowngradingRepository
    {
        #region 
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="manuDowngradingEntity"></param>
        /// <returns></returns>
        Task<int> InsertAsync(ManuDowngradingEntity manuDowngradingEntity);

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="manuDowngradingEntitys"></param>
        /// <returns></returns>
        Task<int> InsertsAsync(IEnumerable<ManuDowngradingEntity>? manuDowngradingEntitys);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="manuDowngradingEntity"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(ManuDowngradingEntity manuDowngradingEntity);

        /// <summary>
        /// 批量更新 
        /// </summary>
        /// <param name="manuDowngradingEntitys"></param>
        /// <returns></returns>
        Task<int> UpdatesAsync(List<ManuDowngradingEntity> manuDowngradingEntitys);

        /// <summary>
        /// 删除  
        /// 最好使用批量删除，可以设置更新人和更新时间
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<int> DeleteAsync(long id);

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
        Task<ManuDowngradingEntity> GetByIdAsync(long id);

        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<IEnumerable<ManuDowngradingEntity>> GetByIdsAsync(long[] ids);

        /// <summary>
        /// 获取List
        /// </summary>
        /// <param name="manuDowngradingQuery"></param>
        /// <returns></returns>
        Task<IEnumerable<ManuDowngradingEntity>> GetManuDowngradingEntitiesAsync(ManuDowngradingQuery manuDowngradingQuery);

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="manuDowngradingPagedQuery"></param>
        /// <returns></returns>
        Task<PagedInfo<ManuDowngradingEntity>> GetPagedInfoAsync(ManuDowngradingPagedQuery manuDowngradingPagedQuery);
        #endregion

        /// <summary>
        /// 根据sfcs获取数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<IEnumerable<ManuDowngradingEntity>> GetBySfcsAsync(ManuDowngradingBySfcsQuery query);

        /// <summary>
        /// 根据sfcs获取数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<IEnumerable<ManuDowngradingEntity>> GetBySFCsAsync(ManuDowngradingBySFCsQuery query);

        /// <summary>
        /// 真实删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<int> DeletesTrueByIdsAsync(long[] ids);
    }
}
