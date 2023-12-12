/*
 *creator: Karl
 *
 *describe: esop 文件仓储类 | 代码由框架生成
 *builder:  zhaoqing
 *build datetime: 2023-11-02 02:41:09
 */
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Data.Repositories.Common.Command;

namespace Hymson.MES.Data.Repositories.Process
{
    /// <summary>
    /// esop 文件仓储接口
    /// </summary>
    public interface IProcEsopFileRepository
    {
        #region 
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="procEsopFileEntity"></param>
        /// <returns></returns>
        Task<int> InsertAsync(ProcEsopFileEntity procEsopFileEntity);
        
        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="procEsopFileEntitys"></param>
        /// <returns></returns>
        Task<int> InsertsAsync(List<ProcEsopFileEntity> procEsopFileEntitys);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="procEsopFileEntity"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(ProcEsopFileEntity procEsopFileEntity);
        
        /// <summary>
        /// 批量更新 
        /// </summary>
        /// <param name="procEsopFileEntitys"></param>
        /// <returns></returns>
        Task<int> UpdatesAsync(List<ProcEsopFileEntity> procEsopFileEntitys);

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
        Task<ProcEsopFileEntity> GetByIdAsync(long id);
    
        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<IEnumerable<ProcEsopFileEntity>> GetByIdsAsync(long[] ids);

        /// <summary>
        /// 获取List
        /// </summary>
        /// <param name="procEsopFileQuery"></param>
        /// <returns></returns>
        Task<IEnumerable<ProcEsopFileEntity>> GetProcEsopFileEntitiesAsync(ProcEsopFileQuery procEsopFileQuery);
        
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="procEsopFilePagedQuery"></param>
        /// <returns></returns>
        Task<PagedInfo<ProcEsopFileEntity>> GetPagedInfoAsync(ProcEsopFilePagedQuery procEsopFilePagedQuery);
        #endregion
    }
}
