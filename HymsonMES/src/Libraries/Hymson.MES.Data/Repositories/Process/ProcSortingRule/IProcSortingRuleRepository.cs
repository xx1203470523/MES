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
using Hymson.MES.Data.Repositories.Process.ProcSortingRule.Query;
using Hymson.MES.Data.Repositories.Process.ProcSortingRule.View;

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

        /// <summary>
        ///根据编码和版本获取分选规则
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        Task<ProcSortingRuleEntity> GetByCodeAndVersion(ProcSortingRuleByCodeAndVersionQuery param);

        /// <summary>
        /// 根据编码和物料获取分选规则
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        Task<ProcSortingRuleEntity> GetByCodeAndMaterialId(ProcSortingRuleCodeAndMaterialIdQuery param);


        /// <summary>
        ///更具编码获取当前版本的分选规则
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        Task<ProcSortingRuleEntity> GetByDefaultVersion(ProcSortingRuleByDefaultVersionQuery param);
        #endregion

        /// <summary>
        /// 更新状态
        /// </summary>
        /// <param name="procMaterialEntitys"></param>
        /// <returns></returns>
        Task<int> UpdateStatusAsync(ChangeStatusCommand command);

        #region 顷刻

        /// <summary>
        /// 根据产品id获取分选规则详情
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public Task<IEnumerable<ProcSortRuleDetailEquView>> GetSortRuleDetailAsync(ProcSortRuleDetailEquQuery param);

        #endregion
    }
}
