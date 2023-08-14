/*
 *creator: Karl
 *
 *describe: 分选规则详情仓储类 | 代码由框架生成
 *builder:  zhaoqing
 *build datetime: 2023-07-25 03:25:19
 */
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Data.Repositories.Common.Command;

namespace Hymson.MES.Data.Repositories.Process
{
    /// <summary>
    /// 分选规则详情仓储接口
    /// </summary>
    public interface IProcSortingRuleDetailRepository
    {
        #region 
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="procSortingRuleDetailEntity"></param>
        /// <returns></returns>
        Task<int> InsertAsync(ProcSortingRuleDetailEntity procSortingRuleDetailEntity);

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="procSortingRuleDetailEntitys"></param>
        /// <returns></returns>
        Task<int> InsertsAsync(IEnumerable<ProcSortingRuleDetailEntity> procSortingRuleDetailEntitys);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="procSortingRuleDetailEntity"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(ProcSortingRuleDetailEntity procSortingRuleDetailEntity);

        /// <summary>
        /// 批量更新 
        /// </summary>
        /// <param name="procSortingRuleDetailEntitys"></param>
        /// <returns></returns>
        Task<int> UpdatesAsync(IEnumerable<ProcSortingRuleDetailEntity> procSortingRuleDetailEntitys);

        /// <summary>
        /// 删除（物理删除）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<int> DeleteSortingRuleDetailByIdAsync(long sortingRuleId);

        /// <summary>
        /// 根据分选规则id获取参数信息
        /// </summary>
        /// <param name="sortingRuleId"></param>
        /// <returns></returns>
        Task<IEnumerable<ProcSortingRuleDetailEntity>> GetSortingRuleDetailByIdAsync(long sortingRuleId);

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
        Task<ProcSortingRuleDetailEntity> GetByIdAsync(long id);

        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<IEnumerable<ProcSortingRuleDetailEntity>> GetByIdsAsync(long[] ids);

        /// <summary>
        /// 获取List
        /// </summary>
        /// <param name="procSortingRuleDetailQuery"></param>
        /// <returns></returns>
        Task<IEnumerable<ProcSortingRuleDetailEntity>> GetProcSortingRuleDetailEntitiesAsync(ProcSortingRuleDetailQuery procSortingRuleDetailQuery);

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="procSortingRuleDetailPagedQuery"></param>
        /// <returns></returns>
        Task<PagedInfo<ProcSortingRuleDetailEntity>> GetPagedInfoAsync(ProcSortingRuleDetailPagedQuery procSortingRuleDetailPagedQuery);
        #endregion
    }
}
