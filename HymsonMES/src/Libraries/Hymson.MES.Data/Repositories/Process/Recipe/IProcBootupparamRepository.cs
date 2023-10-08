/*
 *creator: Karl
 *
 *describe: 开机参数表仓储类 | 代码由框架生成
 *builder:  wxk
 *build datetime: 2023-07-05 04:22:20
 */
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Data.Repositories.Common.Command;

namespace Hymson.MES.Data.Repositories.Process
{
    /// <summary>
    /// 开机参数表仓储接口
    /// </summary>
    public interface IProcBootupparamRepository
    {
        #region 
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="procBootupparamEntity"></param>
        /// <returns></returns>
        Task<int> InsertAsync(ProcBootupparamEntity procBootupparamEntity);
        
        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="procBootupparamEntitys"></param>
        /// <returns></returns>
        Task<int> InsertsAsync(List<ProcBootupparamEntity> procBootupparamEntitys);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="procBootupparamEntity"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(ProcBootupparamEntity procBootupparamEntity);
        
        /// <summary>
        /// 批量更新 
        /// </summary>
        /// <param name="procBootupparamEntitys"></param>
        /// <returns></returns>
        Task<int> UpdatesAsync(List<ProcBootupparamEntity> procBootupparamEntitys);

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
        Task<ProcBootupparamEntity> GetByIdAsync(long id);
    
        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<IEnumerable<ProcBootupparamEntity>> GetByIdsAsync(long[] ids);

        /// <summary>
        /// 获取List
        /// </summary>
        /// <param name="procBootupparamQuery"></param>
        /// <returns></returns>
        Task<IEnumerable<ProcBootupparamEntity>> GetProcBootupparamEntitiesAsync(ProcBootupparamQuery procBootupparamQuery);
        
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="procBootupparamPagedQuery"></param>
        /// <returns></returns>
        Task<PagedInfo<ProcBootupparamEntity>> GetPagedInfoAsync(ProcBootupparamPagedQuery procBootupparamPagedQuery);
        #endregion
    }
}
