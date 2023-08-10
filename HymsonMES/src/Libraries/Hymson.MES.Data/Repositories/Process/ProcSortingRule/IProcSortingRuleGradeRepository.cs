/*
 *creator: Karl
 *
 *describe: 档次仓储类 | 代码由框架生成
 *builder:  zhaoqing
 *build datetime: 2023-07-25 03:34:14
 */
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Data.Repositories.Common.Command;

namespace Hymson.MES.Data.Repositories.Process
{
    /// <summary>
    /// 档次仓储接口
    /// </summary>
    public interface IProcSortingRuleGradeRepository
    {
        #region 
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="procSortingRuleGradeEntity"></param>
        /// <returns></returns>
        Task<int> InsertAsync(ProcSortingRuleGradeEntity procSortingRuleGradeEntity);

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="procSortingRuleGradeEntitys"></param>
        /// <returns></returns>
        Task<int> InsertsAsync(List<ProcSortingRuleGradeEntity> procSortingRuleGradeEntitys);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="procSortingRuleGradeEntity"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(ProcSortingRuleGradeEntity procSortingRuleGradeEntity);

        /// <summary>
        /// 批量更新 
        /// </summary>
        /// <param name="procSortingRuleGradeEntitys"></param>
        /// <returns></returns>
        Task<int> UpdatesAsync(List<ProcSortingRuleGradeEntity> procSortingRuleGradeEntitys);

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
        Task<ProcSortingRuleGradeEntity> GetByIdAsync(long id);

        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<IEnumerable<ProcSortingRuleGradeEntity>> GetByIdsAsync(long[] ids);

        /// <summary>
        /// 获取List
        /// </summary>
        /// <param name="procSortingRuleGradeQuery"></param>
        /// <returns></returns>
        Task<IEnumerable<ProcSortingRuleGradeEntity>> GetProcSortingRuleGradeEntitiesAsync(ProcSortingRuleGradeQuery procSortingRuleGradeQuery);

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="procSortingRuleGradePagedQuery"></param>
        /// <returns></returns>
        Task<PagedInfo<ProcSortingRuleGradeEntity>> GetPagedInfoAsync(ProcSortingRuleGradePagedQuery procSortingRuleGradePagedQuery);

        /// <summary>
        /// 删除（物理删除）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<int> DeleteSortingRuleGradeByIdAsync(long id);
        #endregion
    }
}
