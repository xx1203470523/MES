/*
 *creator: Karl
 *
 *describe: 开机参数采集表仓储类 | 代码由框架生成
 *builder:  wxk
 *build datetime: 2023-07-12 04:58:46
 */
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Data.Repositories.Common.Command;

namespace Hymson.MES.Data.Repositories.Process
{
    /// <summary>
    /// 开机参数采集表仓储接口
    /// </summary>
    public interface IProcBootupparamrecordRepository
    {
        #region 
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="procBootupparamrecordEntity"></param>
        /// <returns></returns>
        Task<int> InsertAsync(ProcBootupparamrecordEntity procBootupparamrecordEntity);
        
        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="procBootupparamrecordEntitys"></param>
        /// <returns></returns>
        Task<int> InsertsAsync(List<ProcBootupparamrecordEntity> procBootupparamrecordEntitys);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="procBootupparamrecordEntity"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(ProcBootupparamrecordEntity procBootupparamrecordEntity);
        
        /// <summary>
        /// 批量更新 
        /// </summary>
        /// <param name="procBootupparamrecordEntitys"></param>
        /// <returns></returns>
        Task<int> UpdatesAsync(List<ProcBootupparamrecordEntity> procBootupparamrecordEntitys);

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
        Task<ProcBootupparamrecordEntity> GetByIdAsync(long id);
    
        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<IEnumerable<ProcBootupparamrecordEntity>> GetByIdsAsync(long[] ids);

        /// <summary>
        /// 获取List
        /// </summary>
        /// <param name="procBootupparamrecordQuery"></param>
        /// <returns></returns>
        Task<IEnumerable<ProcBootupparamrecordEntity>> GetProcBootupparamrecordEntitiesAsync(ProcBootupparamrecordQuery procBootupparamrecordQuery);
        
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="procBootupparamrecordPagedQuery"></param>
        /// <returns></returns>
        Task<PagedInfo<ProcBootupparamrecordEntity>> GetPagedInfoAsync(ProcBootupparamrecordPagedQuery procBootupparamrecordPagedQuery);
        #endregion
    }
}
