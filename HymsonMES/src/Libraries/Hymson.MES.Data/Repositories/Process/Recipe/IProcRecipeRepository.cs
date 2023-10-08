/*
 *creator: Karl
 *
 *describe: 配方表仓储类 | 代码由框架生成
 *builder:  wxk
 *build datetime: 2023-07-04 03:02:39
 */
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Data.Repositories.Common.Command;

namespace Hymson.MES.Data.Repositories.Process
{
    /// <summary>
    /// 配方表仓储接口
    /// </summary>
    public interface IProcRecipeRepository
    {
        #region 
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="procRecipeEntity"></param>
        /// <returns></returns>
        Task<int> InsertAsync(ProcRecipeEntity procRecipeEntity);
        
        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="procRecipeEntitys"></param>
        /// <returns></returns>
        Task<int> InsertsAsync(List<ProcRecipeEntity> procRecipeEntitys);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="procRecipeEntity"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(ProcRecipeEntity procRecipeEntity);
        
        /// <summary>
        /// 批量更新 
        /// </summary>
        /// <param name="procRecipeEntitys"></param>
        /// <returns></returns>
        Task<int> UpdatesAsync(List<ProcRecipeEntity> procRecipeEntitys);

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
        Task<ProcRecipeEntity> GetByIdAsync(long id);
    
        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<IEnumerable<ProcRecipeEntity>> GetByIdsAsync(long[] ids);

        /// <summary>
        /// 获取List
        /// </summary>
        /// <param name="procRecipeQuery"></param>
        /// <returns></returns>
        Task<IEnumerable<ProcRecipeEntity>> GetProcRecipeEntitiesAsync(ProcRecipeQuery procRecipeQuery);
        
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="procRecipePagedQuery"></param>
        /// <returns></returns>
        Task<PagedInfo<ProcRecipeEntity>> GetPagedInfoAsync(ProcRecipePagedQuery procRecipePagedQuery);
        #endregion
    }
}
