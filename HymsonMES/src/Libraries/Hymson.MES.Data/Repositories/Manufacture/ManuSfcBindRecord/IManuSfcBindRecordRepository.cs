/*
 *creator: Karl
 *
 *describe: 条码绑定解绑记录表仓储类 | 代码由框架生成
 *builder:  chenjianxiong
 *build datetime: 2023-05-17 10:09:25
 */
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Data.Repositories.Common.Command;

namespace Hymson.MES.Data.Repositories.Manufacture
{
    /// <summary>
    /// 条码绑定解绑记录表仓储接口
    /// </summary>
    public interface IManuSfcBindRecordRepository
    {
        #region 
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="manuSfcBindRecordEntity"></param>
        /// <returns></returns>
        Task<int> InsertAsync(ManuSfcBindRecordEntity manuSfcBindRecordEntity);
        
        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="manuSfcBindRecordEntitys"></param>
        /// <returns></returns>
        Task<int> InsertsAsync(List<ManuSfcBindRecordEntity> manuSfcBindRecordEntitys);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="manuSfcBindRecordEntity"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(ManuSfcBindRecordEntity manuSfcBindRecordEntity);
        
        /// <summary>
        /// 批量更新 
        /// </summary>
        /// <param name="manuSfcBindRecordEntitys"></param>
        /// <returns></returns>
        Task<int> UpdatesAsync(List<ManuSfcBindRecordEntity> manuSfcBindRecordEntitys);

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
        Task<ManuSfcBindRecordEntity> GetByIdAsync(long id);
    
        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<IEnumerable<ManuSfcBindRecordEntity>> GetByIdsAsync(long[] ids);

        /// <summary>
        /// 获取List
        /// </summary>
        /// <param name="manuSfcBindRecordQuery"></param>
        /// <returns></returns>
        Task<IEnumerable<ManuSfcBindRecordEntity>> GetManuSfcBindRecordEntitiesAsync(ManuSfcBindRecordQuery manuSfcBindRecordQuery);
        
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="manuSfcBindRecordPagedQuery"></param>
        /// <returns></returns>
        Task<PagedInfo<ManuSfcBindRecordEntity>> GetPagedInfoAsync(ManuSfcBindRecordPagedQuery manuSfcBindRecordPagedQuery);
        #endregion
    }
}
