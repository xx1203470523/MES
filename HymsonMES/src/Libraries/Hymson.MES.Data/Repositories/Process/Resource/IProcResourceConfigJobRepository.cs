/*
 *creator: Karl
 *
 *describe: 资源作业配置表仓储类 | 代码由框架生成
 *builder:  zhaoqing
 *build datetime: 2023-02-10 05:26:36
 */
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Data.Repositories.Process;

namespace Hymson.MES.Data.Repositories.Process
{
    /// <summary>
    /// 资源作业配置表仓储接口
    /// </summary>
    public interface IProcResourceConfigJobRepository
    {
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="procResourceConfigJobPagedQuery"></param>
        /// <returns></returns>
        Task<PagedInfo<ProcResourceConfigJobView>> GetPagedInfoAsync(ProcResourceConfigJobPagedQuery procResourceConfigJobPagedQuery);
        
        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="procResourceConfigJobs"></param>
        /// <returns></returns>
        Task<int> InsertRangeAsync(List<ProcResourceConfigJobEntity> procResourceConfigJobs);

        /// <summary>
        /// 批量更新 
        /// </summary>
        /// <param name="procResourceConfigJobs"></param>
        /// <returns></returns>
        Task<int> UpdateRangeAsync(List<ProcResourceConfigJobEntity> procResourceConfigJobs);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<int> DeleteAsync(long id);
        
        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<int> DeletesRangeAsync(long[] ids);
    }
}
