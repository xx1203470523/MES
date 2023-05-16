/*
 *creator: Karl
 *
 *describe: 托盘条码记录表仓储类 | 代码由框架生成
 *builder:  chenjianxiong
 *build datetime: 2023-05-16 11:11:02
 */
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Data.Repositories.Common.Command;

namespace Hymson.MES.Data.Repositories.Manufacture
{
    /// <summary>
    /// 托盘条码记录表仓储接口
    /// </summary>
    public interface IManuTraySfcRecordRepository
    {
        #region 
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="manuTraySfcRecordEntity"></param>
        /// <returns></returns>
        Task<int> InsertAsync(ManuTraySfcRecordEntity manuTraySfcRecordEntity);
        
        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="manuTraySfcRecordEntitys"></param>
        /// <returns></returns>
        Task<int> InsertsAsync(List<ManuTraySfcRecordEntity> manuTraySfcRecordEntitys);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="manuTraySfcRecordEntity"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(ManuTraySfcRecordEntity manuTraySfcRecordEntity);
        
        /// <summary>
        /// 批量更新 
        /// </summary>
        /// <param name="manuTraySfcRecordEntitys"></param>
        /// <returns></returns>
        Task<int> UpdatesAsync(List<ManuTraySfcRecordEntity> manuTraySfcRecordEntitys);

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
        Task<ManuTraySfcRecordEntity> GetByIdAsync(long id);
    
        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<IEnumerable<ManuTraySfcRecordEntity>> GetByIdsAsync(long[] ids);

        /// <summary>
        /// 获取List
        /// </summary>
        /// <param name="manuTraySfcRecordQuery"></param>
        /// <returns></returns>
        Task<IEnumerable<ManuTraySfcRecordEntity>> GetManuTraySfcRecordEntitiesAsync(ManuTraySfcRecordQuery manuTraySfcRecordQuery);
        
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="manuTraySfcRecordPagedQuery"></param>
        /// <returns></returns>
        Task<PagedInfo<ManuTraySfcRecordEntity>> GetPagedInfoAsync(ManuTraySfcRecordPagedQuery manuTraySfcRecordPagedQuery);
        #endregion
    }
}
