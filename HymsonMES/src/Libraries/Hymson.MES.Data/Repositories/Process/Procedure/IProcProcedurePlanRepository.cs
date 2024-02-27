
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Proc;
using Hymson.MES.Data.Repositories.Common.Command;

namespace Hymson.MES.Data.Repositories.Proc;

/// <summary>
/// <para>@层级：仓储层</para>
/// <para>@类型：接口</para>
/// <para>@描述：工序计划产能;</para>
/// <para>@作者：Jim</para>
/// <para>@创建时间：2024-2-27</para>
/// </summary>
public partial interface IProcProcedurePlanRepository
{
    #region 查询

    /// <summary>
    /// 单条数据查询
    /// </summary>
    /// <param name="query"></param>
    /// <returns></returns>
    Task<ProcProcedurePlanEntity> GetOneAsync(ProcProcedurePlanQuery query);

    /// <summary>
    /// 数据集查询
    /// </summary>
    /// <param name="query"></param>
    /// <returns></returns>
    Task<IEnumerable<ProcProcedurePlanEntity>> GetListAsync(ProcProcedurePlanQuery query);

    /// <summary>
    /// 分页查询
    /// </summary>
    /// <param name="query"></param>
    /// <returns></returns>
    Task<PagedInfo<ProcProcedurePlanEntity>> GetPagedInfoAsync(ProcProcedurePlanPagedQuery query);

    #endregion

    #region 新增

    /// <summary>
    /// 创建数据
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    Task<int> InsertAsync(ProcProcedurePlanCreateCommand command);

    /// <summary>
    /// 批量创建数据
    /// </summary>
    /// <param name="commands"></param>
    /// <returns></returns>
    Task<int> InsertAsync(IEnumerable<ProcProcedurePlanCreateCommand> commands);

    #endregion

    #region 修改

    /// <summary>
    /// 更新数据
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    Task<int> UpdateAsync(ProcProcedurePlanUpdateCommand command);

    /// <summary>
    /// 批量更新数据
    /// </summary>
    /// <param name="commands"></param>
    /// <returns></returns>
    Task<int> UpdateAsync(IEnumerable<ProcProcedurePlanUpdateCommand> commands);

    #endregion

    #region 删除

    /// <summary>
    /// 根据ID删除数据
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    Task<int> DeleteAsync(DeleteCommand command);

    /// <summary>
    /// 根据工序Id删除
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    Task<int> DeleteByProcedureIdAsync(ProcProcedurePlanDeleteCommand command);

    #endregion

    #region 扩展

    Task<int> InsertOrUpdateAsnyc(ProcProcedurePlanCreateCommand command);

    #endregion
}