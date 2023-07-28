/*
 *creator: Karl
 *
 *describe: 烘烤执行表仓储类 | 代码由框架生成
 *builder:  wxk
 *build datetime: 2023-07-28 05:42:41
 */
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Data.Repositories.Common.Command;

namespace Hymson.MES.Data.Repositories.Manufacture
{
    /// <summary>
    /// 烘烤执行表仓储接口
    /// </summary>
    public interface IManuBakingRecordRepository
    {
        #region 
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="manuBakingRecordEntity"></param>
        /// <returns></returns>
        Task<int> InsertAsync(ManuBakingRecordEntity manuBakingRecordEntity);
        
        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="manuBakingRecordEntitys"></param>
        /// <returns></returns>
        Task<int> InsertsAsync(List<ManuBakingRecordEntity> manuBakingRecordEntitys);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="manuBakingRecordEntity"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(ManuBakingRecordEntity manuBakingRecordEntity);
        
        /// <summary>
        /// 批量更新 
        /// </summary>
        /// <param name="manuBakingRecordEntitys"></param>
        /// <returns></returns>
        Task<int> UpdatesAsync(List<ManuBakingRecordEntity> manuBakingRecordEntitys);

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
        Task<ManuBakingRecordEntity> GetByIdAsync(long id);
    
        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<IEnumerable<ManuBakingRecordEntity>> GetByIdsAsync(long[] ids);

        /// <summary>
        /// 获取List
        /// </summary>
        /// <param name="manuBakingRecordQuery"></param>
        /// <returns></returns>
        Task<IEnumerable<ManuBakingRecordEntity>> GetManuBakingRecordEntitiesAsync(ManuBakingRecordQuery manuBakingRecordQuery);
        
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="manuBakingRecordPagedQuery"></param>
        /// <returns></returns>
        Task<PagedInfo<ManuBakingRecordEntity>> GetPagedInfoAsync(ManuBakingRecordPagedQuery manuBakingRecordPagedQuery);
        #endregion
    }
}
