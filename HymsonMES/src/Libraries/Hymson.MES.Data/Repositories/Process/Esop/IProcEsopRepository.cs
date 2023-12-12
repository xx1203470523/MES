/*
 *creator: Karl
 *
 *describe: ESOP仓储类 | 代码由框架生成
 *builder:  zhaoqing
 *build datetime: 2023-11-02 02:39:53
 */
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Data.Repositories.Common.Command;

namespace Hymson.MES.Data.Repositories.Process
{
    /// <summary>
    /// ESOP仓储接口
    /// </summary>
    public interface IProcEsopRepository
    {
        #region 
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="procEsopEntity"></param>
        /// <returns></returns>
        Task<int> InsertAsync(ProcEsopEntity procEsopEntity);
        
        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="procEsopEntitys"></param>
        /// <returns></returns>
        Task<int> InsertsAsync(List<ProcEsopEntity> procEsopEntitys);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="procEsopEntity"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(ProcEsopEntity procEsopEntity);
        
        /// <summary>
        /// 批量更新 
        /// </summary>
        /// <param name="procEsopEntitys"></param>
        /// <returns></returns>
        Task<int> UpdatesAsync(List<ProcEsopEntity> procEsopEntitys);

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
        Task<ProcEsopEntity> GetByIdAsync(long id);
    
        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<IEnumerable<ProcEsopEntity>> GetByIdsAsync(long[] ids);

        /// <summary>
        /// 获取List
        /// </summary>
        /// <param name="procEsopQuery"></param>
        /// <returns></returns>
        Task<IEnumerable<ProcEsopEntity>> GetProcEsopEntitiesAsync(ProcEsopQuery procEsopQuery);
        
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="procEsopPagedQuery"></param>
        /// <returns></returns>
        Task<PagedInfo<ProcEsopView>> GetPagedInfoAsync(ProcEsopPagedQuery procEsopPagedQuery);
        #endregion
    }
}
