

using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Plan;
using Hymson.MES.Data.Repositories.Common.Command;

namespace Hymson.MES.Data.Repositories.Plan;

/// <summary>
/// <para>@层级：仓储层</para>
/// <para>@类型：接口</para>
/// <para>@描述：生产日历;</para>
/// <para>@作者：Jim</para>
/// <para>@创建时间：2024-1-31</para>
/// </summary>
public partial interface IPlanCalendarRepository
{
    #region 查询

    /// <summary>
    /// 单条数据查询
    /// </summary>
    /// <param name="query"></param>
    /// <returns></returns>
    Task<PlanCalendarEntity> GetOneAsync(PlanCalendarQuery query);

    /// <summary>
    /// 数据集查询
    /// </summary>
    /// <param name="query"></param>
    /// <returns></returns>
    Task<IEnumerable<PlanCalendarEntity>> GetListAsync(PlanCalendarQuery query);

    /// <summary>
    /// 分页查询
    /// </summary>
    /// <param name="query"></param>
    /// <returns></returns>
    Task<PagedInfo<PlanCalendarEntity>> GetPagedInfoAsync(PlanCalendarPagedQuery query);

    #endregion

    #region 新增

    /// <summary>
    /// 创建数据
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    Task<int> InsertAsync(PlanCalendarCreateCommand command);

    /// <summary>
    /// 批量创建数据
    /// </summary>
    /// <param name="commands"></param>
    /// <returns></returns>
    Task<int> InsertAsync(IEnumerable<PlanCalendarCreateCommand> commands);

    #endregion

    #region 修改

    /// <summary>
    /// 更新数据
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    Task<int> UpdateAsync(PlanCalendarUpdateCommand command);

    /// <summary>
    /// 批量更新数据
    /// </summary>
    /// <param name="commands"></param>
    /// <returns></returns>
    Task<int> UpdateAsync(IEnumerable<PlanCalendarUpdateCommand> commands);

    #endregion

    #region 删除

    /// <summary>
    /// 根据ID删除数据
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    Task<int> DeleteAsync(DeleteCommand command);

    /// <summary>
    /// 根据ID删除多条数据
    /// </summary>
    /// <param name="commands"></param>
    /// <returns></returns>
    Task<int> DeleteMoreAsync(DeleteCommand commands);

    #endregion

}

/// <summary>
/// <para>@层级：仓储层</para>
/// <para>@类型：接口</para>
/// <para>@描述：生产日历; 扩展</para>
/// <para>@作者：Jim</para>
/// <para>@创建时间：2024-1-31</para>
/// </summary>
public partial interface IPlanCalendarRepository
{
}