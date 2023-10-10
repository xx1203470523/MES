/*
 *creator: Karl
 *
 *describe: 开机配方表仓储类 | 代码由框架生成
 *builder:  wxk
 *build datetime: 2023-07-05 04:11:36
 */
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Data.Repositories.Common.Command;

namespace Hymson.MES.Data.Repositories.Process
{
    /// <summary>
    /// 开机配方表仓储接口
    /// </summary>
    public interface IProcBootuprecipeRepository
    {
        #region 
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="procBootuprecipeEntity"></param>
        /// <returns></returns>
        Task<int> InsertAsync(ProcBootuprecipeEntity procBootuprecipeEntity);
        
        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="procBootuprecipeEntitys"></param>
        /// <returns></returns>
        Task<int> InsertsAsync(List<ProcBootuprecipeEntity> procBootuprecipeEntitys);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="procBootuprecipeEntity"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(ProcBootuprecipeEntity procBootuprecipeEntity);
        
        /// <summary>
        /// 批量更新 
        /// </summary>
        /// <param name="procBootuprecipeEntitys"></param>
        /// <returns></returns>
        Task<int> UpdatesAsync(List<ProcBootuprecipeEntity> procBootuprecipeEntitys);

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
        Task<ProcBootuprecipeEntity> GetByIdAsync(long id);
        /// <summary>
        /// 根据code获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ProcBootuprecipeEntity> GetByCodeAsync(string code);

        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<IEnumerable<ProcBootuprecipeEntity>> GetByIdsAsync(long[] ids);

        /// <summary>
        /// 获取List
        /// </summary>
        /// <param name="procBootuprecipeQuery"></param>
        /// <returns></returns>
        Task<IEnumerable<ProcBootuprecipeEntity>> GetProcBootuprecipeEntitiesAsync(ProcBootuprecipeQuery procBootuprecipeQuery);
        
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="procBootuprecipePagedQuery"></param>
        /// <returns></returns>
        Task<PagedInfo<ProcBootuprecipeEntity>> GetPagedInfoAsync(ProcBootuprecipePagedQuery procBootuprecipePagedQuery);
        #endregion
    }
}
