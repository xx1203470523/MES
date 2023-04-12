/*
 *creator: Karl
 *
 *describe: 容器装载记录仓储类 | 代码由框架生成
 *builder:  wxk
 *build datetime: 2023-04-12 02:32:21
 */
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Data.Repositories.Common.Command;

namespace Hymson.MES.Data.Repositories.Manufacture
{
    /// <summary>
    /// 容器装载记录仓储接口
    /// </summary>
    public interface IManuContainerPackRecordRepository
    {
        #region 
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="manuContainerPackRecordEntity"></param>
        /// <returns></returns>
        Task<int> InsertAsync(ManuContainerPackRecordEntity manuContainerPackRecordEntity);
        
        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="manuContainerPackRecordEntitys"></param>
        /// <returns></returns>
        Task<int> InsertsAsync(List<ManuContainerPackRecordEntity> manuContainerPackRecordEntitys);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="manuContainerPackRecordEntity"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(ManuContainerPackRecordEntity manuContainerPackRecordEntity);
        
        /// <summary>
        /// 批量更新 
        /// </summary>
        /// <param name="manuContainerPackRecordEntitys"></param>
        /// <returns></returns>
        Task<int> UpdatesAsync(List<ManuContainerPackRecordEntity> manuContainerPackRecordEntitys);

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
        Task<ManuContainerPackRecordEntity> GetByIdAsync(long id);
    
        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<IEnumerable<ManuContainerPackRecordEntity>> GetByIdsAsync(long[] ids);

        /// <summary>
        /// 获取List
        /// </summary>
        /// <param name="manuContainerPackRecordQuery"></param>
        /// <returns></returns>
        Task<IEnumerable<ManuContainerPackRecordEntity>> GetManuContainerPackRecordEntitiesAsync(ManuContainerPackRecordQuery manuContainerPackRecordQuery);
        
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="manuContainerPackRecordPagedQuery"></param>
        /// <returns></returns>
        Task<PagedInfo<ManuContainerPackRecordEntity>> GetPagedInfoAsync(ManuContainerPackRecordPagedQuery manuContainerPackRecordPagedQuery);
        #endregion
    }
}
