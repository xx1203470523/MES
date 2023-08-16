/*
 *creator: Karl
 *
 *describe: 档位详情仓储类 | 代码由框架生成
 *builder:  zhaoqing
 *build datetime: 2023-07-25 03:34:23
 */
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Data.Repositories.Common.Command;

namespace Hymson.MES.Data.Repositories.Process
{
    /// <summary>
    /// 档位详情仓储接口
    /// </summary>
    public interface IProcSortingRuleGradeDetailsRepository
    {
        #region 
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="procSortingRuleGradeDetailsEntity"></param>
        /// <returns></returns>
        Task<int> InsertAsync(ProcSortingRuleGradeDetailsEntity procSortingRuleGradeDetailsEntity);
        
        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="procSortingRuleGradeDetailsEntitys"></param>
        /// <returns></returns>
        Task<int> InsertsAsync(List<ProcSortingRuleGradeDetailsEntity> procSortingRuleGradeDetailsEntitys);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="procSortingRuleGradeDetailsEntity"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(ProcSortingRuleGradeDetailsEntity procSortingRuleGradeDetailsEntity);
        
        /// <summary>
        /// 批量更新 
        /// </summary>
        /// <param name="procSortingRuleGradeDetailsEntitys"></param>
        /// <returns></returns>
        Task<int> UpdatesAsync(List<ProcSortingRuleGradeDetailsEntity> procSortingRuleGradeDetailsEntitys);

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
        Task<ProcSortingRuleGradeDetailsEntity> GetByIdAsync(long id);
    
        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<IEnumerable<ProcSortingRuleGradeDetailsEntity>> GetByIdsAsync(long[] ids);

        /// <summary>
        /// 获取List
        /// </summary>
        /// <param name="procSortingRuleGradeDetailsQuery"></param>
        /// <returns></returns>
        Task<IEnumerable<ProcSortingRuleGradeDetailsEntity>> GetProcSortingRuleGradeDetailsEntitiesAsync(ProcSortingRuleGradeDetailsQuery procSortingRuleGradeDetailsQuery);
        
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="procSortingRuleGradeDetailsPagedQuery"></param>
        /// <returns></returns>
        Task<PagedInfo<ProcSortingRuleGradeDetailsEntity>> GetPagedInfoAsync(ProcSortingRuleGradeDetailsPagedQuery procSortingRuleGradeDetailsPagedQuery);

        /// <summary>
        /// 删除（物理删除）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<int> DeleteSortingRuleGradeByIdAsync(long id);

        /// <summary>
        /// 根据分选规则id获取档位信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<IEnumerable<ProcSortingRuleGradeDetailsEntity>> GetSortingRuleGradeeDetailsByIdAsync(long sortingRuleId);
        #endregion
    }
}
