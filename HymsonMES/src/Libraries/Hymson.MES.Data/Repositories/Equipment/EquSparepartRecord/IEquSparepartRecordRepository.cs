/*
 *creator: Karl
 *
 *describe: 设备备件记录表仓储类 | 代码由框架生成
 *builder:  pengxin
 *build datetime: 2024-06-12 10:29:55
 */
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.EquSparepartRecord;
using Hymson.MES.Data.Repositories.Common.Command;

namespace Hymson.MES.Data.Repositories.EquSparepartRecord
{
    /// <summary>
    /// 设备备件记录表仓储接口
    /// </summary>
    public interface IEquSparepartRecordRepository
    {
        #region 
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="equSparepartRecordEntity"></param>
        /// <returns></returns>
        Task<int> InsertAsync(EquSparepartRecordEntity equSparepartRecordEntity);
        
        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="equSparepartRecordEntitys"></param>
        /// <returns></returns>
        Task<int> InsertsAsync(List<EquSparepartRecordEntity> equSparepartRecordEntitys);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="equSparepartRecordEntity"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(EquSparepartRecordEntity equSparepartRecordEntity);
        
        /// <summary>
        /// 批量更新 
        /// </summary>
        /// <param name="equSparepartRecordEntitys"></param>
        /// <returns></returns>
        Task<int> UpdatesAsync(List<EquSparepartRecordEntity> equSparepartRecordEntitys);

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
        Task<EquSparepartRecordEntity> GetByIdAsync(long id);
    
        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<IEnumerable<EquSparepartRecordEntity>> GetByIdsAsync(long[] ids);

        /// <summary>
        /// 获取List
        /// </summary>
        /// <param name="equSparepartRecordQuery"></param>
        /// <returns></returns>
        Task<IEnumerable<EquSparepartRecordEntity>> GetEquSparepartRecordEntitiesAsync(EquSparepartRecordQuery equSparepartRecordQuery);
        
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="equSparepartRecordPagedQuery"></param>
        /// <returns></returns>
        Task<PagedInfo<EquSparepartRecordEntity>> GetPagedInfoAsync(EquSparepartRecordPagedQuery equSparepartRecordPagedQuery);
        #endregion
    }
}
