/*
 *creator: Karl
 *
 *describe: 降级品录入记录仓储类 | 代码由框架生成
 *builder:  Karl
 *build datetime: 2023-08-10 10:15:49
 */
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Data.Repositories.Common.Command;

namespace Hymson.MES.Data.Repositories.Manufacture
{
    /// <summary>
    /// 降级品录入记录仓储接口
    /// </summary>
    public interface IManuDowngradingRecordRepository
    {
        #region 
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="manuDowngradingRecordEntity"></param>
        /// <returns></returns>
        Task<int> InsertAsync(ManuDowngradingRecordEntity manuDowngradingRecordEntity);
        
        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="manuDowngradingRecordEntitys"></param>
        /// <returns></returns>
        Task<int> InsertsAsync(IEnumerable<ManuDowngradingRecordEntity>? manuDowngradingRecordEntitys);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="manuDowngradingRecordEntity"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(ManuDowngradingRecordEntity manuDowngradingRecordEntity);
        
        /// <summary>
        /// 批量更新 
        /// </summary>
        /// <param name="manuDowngradingRecordEntitys"></param>
        /// <returns></returns>
        Task<int> UpdatesAsync(List<ManuDowngradingRecordEntity> manuDowngradingRecordEntitys);

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
        Task<ManuDowngradingRecordEntity> GetByIdAsync(long id);
    
        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<IEnumerable<ManuDowngradingRecordEntity>> GetByIdsAsync(long[] ids);

        /// <summary>
        /// 获取List
        /// </summary>
        /// <param name="manuDowngradingRecordQuery"></param>
        /// <returns></returns>
        Task<IEnumerable<ManuDowngradingRecordEntity>> GetManuDowngradingRecordEntitiesAsync(ManuDowngradingRecordQuery manuDowngradingRecordQuery);
        
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="manuDowngradingRecordPagedQuery"></param>
        /// <returns></returns>
        Task<PagedInfo<ManuDowngradingRecordEntity>> GetPagedInfoAsync(ManuDowngradingRecordPagedQuery manuDowngradingRecordPagedQuery);
        #endregion
    }
}
