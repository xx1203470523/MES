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
        /// 新增
        /// </summary>
        /// <param name="procResourceConfigJobEntity"></param>
        /// <returns></returns>
        Task<int> InsertAsync(ProcResourceConfigJobEntity procResourceConfigJobEntity);
        
        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="procResourceConfigJobEntitys"></param>
        /// <returns></returns>
        Task<int> InsertsAsync(List<ProcResourceConfigJobEntity> procResourceConfigJobEntitys);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="procResourceConfigJobEntity"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(ProcResourceConfigJobEntity procResourceConfigJobEntity);
        
        /// <summary>
        /// 批量更新 
        /// </summary>
        /// <param name="procResourceConfigJobEntitys"></param>
        /// <returns></returns>
        Task<int> UpdatesAsync(List<ProcResourceConfigJobEntity> procResourceConfigJobEntitys);

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
        Task<int> DeletesAsync(long[] ids);

        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ProcResourceConfigJobEntity> GetByIdAsync(long id);
    
        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<IEnumerable<ProcResourceConfigJobEntity>> GetByIdsAsync(long[] ids);

        /// <summary>
        /// 获取List
        /// </summary>
        /// <param name="procResourceConfigJobQuery"></param>
        /// <returns></returns>
        Task<IEnumerable<ProcResourceConfigJobEntity>> GetProcResourceConfigJobEntitiesAsync(ProcResourceConfigJobQuery procResourceConfigJobQuery);
        
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="procResourceConfigJobPagedQuery"></param>
        /// <returns></returns>
        Task<PagedInfo<ProcResourceConfigJobView>> GetPagedInfoAsync(ProcResourceConfigJobPagedQuery procResourceConfigJobPagedQuery);
    }
}
