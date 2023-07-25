/*
 *creator: Karl
 *
 *describe: 分选规则仓储类 | 代码由框架生成
 *builder:  zhaoqing
 *build datetime: 2023-07-25 03:24:54
 */
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Data.Repositories.Common.Command;

namespace Hymson.MES.Data.Repositories.Process
{
    /// <summary>
    /// 分选规则仓储接口
    /// </summary>
    public interface IProcSortingRuleRepository
    {
        #region 
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="procSortingRuleEntity"></param>
        /// <returns></returns>
        Task<int> InsertAsync(ProcSortingRuleEntity procSortingRuleEntity);
        
        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="procSortingRuleEntitys"></param>
        /// <returns></returns>
        Task<int> InsertsAsync(List<ProcSortingRuleEntity> procSortingRuleEntitys);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="procSortingRuleEntity"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(ProcSortingRuleEntity procSortingRuleEntity);
        
        /// <summary>
        /// 批量更新 
        /// </summary>
        /// <param name="procSortingRuleEntitys"></param>
        /// <returns></returns>
        Task<int> UpdatesAsync(List<ProcSortingRuleEntity> procSortingRuleEntitys);

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
        Task<ProcSortingRuleEntity> GetByIdAsync(long id);
    
        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<IEnumerable<ProcSortingRuleEntity>> GetByIdsAsync(long[] ids);

        /// <summary>
        /// 获取List
        /// </summary>
        /// <param name="procSortingRuleQuery"></param>
        /// <returns></returns>
        Task<IEnumerable<ProcSortingRuleEntity>> GetProcSortingRuleEntitiesAsync(ProcSortingRuleQuery procSortingRuleQuery);
        
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="procSortingRulePagedQuery"></param>
        /// <returns></returns>
        Task<PagedInfo<ProcSortingRuleEntity>> GetPagedInfoAsync(ProcSortingRulePagedQuery procSortingRulePagedQuery);
        #endregion
    }
}
