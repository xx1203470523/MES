/*
 *creator: Karl
 *
 *describe: 分选规则    服务接口 | 代码由框架生成
 *builder:  zhaoqing
 *build datetime: 2023-07-25 03:24:54
 */
using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Process;

namespace Hymson.MES.Services.Services.Process
{
    /// <summary>
    /// 分选规则 service接口
    /// </summary>
    public interface IProcSortingRuleService
    {
        /// <summary>
        /// 获取分页List
        /// </summary>
        /// <param name="procSortingRulePagedQueryDto"></param>
        /// <returns></returns>
        Task<PagedInfo<ProcSortingRuleDto>> GetPagedListAsync(ProcSortingRulePagedQueryDto procSortingRulePagedQueryDto);

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="procSortingRuleCreateDto"></param>
        /// <returns></returns>
        Task CreateProcSortingRuleAsync(ProcSortingRuleCreateDto procSortingRuleCreateDto);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="procSortingRuleModifyDto"></param>
        /// <returns></returns>
        Task ModifyProcSortingRuleAsync(ProcSortingRuleModifyDto procSortingRuleModifyDto);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task DeleteProcSortingRuleAsync(long id);

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<int> DeletesProcSortingRuleAsync(long[] ids);

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ProcSortingRuleDto> QueryProcSortingRuleByIdAsync(long id);
    }
}
