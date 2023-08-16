/*
 *creator: Karl
 *
 *describe: CCS盖板NG记录仓储类 | 代码由框架生成
 *builder:  chenjianxiong
 *build datetime: 2023-08-15 05:15:40
 */
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Data.Repositories.Common.Command;

namespace Hymson.MES.Data.Repositories.Manufacture
{
    /// <summary>
    /// CCS盖板NG记录仓储接口
    /// </summary>
    public interface IManuSfcCcsNgRecordRepository
    {
        #region 
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="manuSfcCcsNgRecordEntity"></param>
        /// <returns></returns>
        Task<int> InsertAsync(ManuSfcCcsNgRecordEntity manuSfcCcsNgRecordEntity);
        
        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="manuSfcCcsNgRecordEntitys"></param>
        /// <returns></returns>
        Task<int> InsertsAsync(List<ManuSfcCcsNgRecordEntity> manuSfcCcsNgRecordEntitys);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="manuSfcCcsNgRecordEntity"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(ManuSfcCcsNgRecordEntity manuSfcCcsNgRecordEntity);
        
        /// <summary>
        /// 批量更新 
        /// </summary>
        /// <param name="manuSfcCcsNgRecordEntitys"></param>
        /// <returns></returns>
        Task<int> UpdatesAsync(List<ManuSfcCcsNgRecordEntity> manuSfcCcsNgRecordEntitys);

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
        Task<ManuSfcCcsNgRecordEntity> GetByIdAsync(long id);
    
        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<IEnumerable<ManuSfcCcsNgRecordEntity>> GetByIdsAsync(long[] ids);

        /// <summary>
        /// 获取List
        /// </summary>
        /// <param name="manuSfcCcsNgRecordQuery"></param>
        /// <returns></returns>
        Task<IEnumerable<ManuSfcCcsNgRecordEntity>> GetManuSfcCcsNgRecordEntitiesAsync(ManuSfcCcsNgRecordQuery manuSfcCcsNgRecordQuery);
        
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="manuSfcCcsNgRecordPagedQuery"></param>
        /// <returns></returns>
        Task<PagedInfo<ManuSfcCcsNgRecordEntity>> GetPagedInfoAsync(ManuSfcCcsNgRecordPagedQuery manuSfcCcsNgRecordPagedQuery);
        #endregion
    }
}
