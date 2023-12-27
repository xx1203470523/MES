
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Integrated;
using Hymson.MES.Data.Repositories.Common.Command;

namespace Hymson.MES.Data.Repositories.Inte;

/// <summary>
/// <para>@层级：仓储层</para>
/// <para>@类型：接口</para>
/// <para>@描述：容器装载维护;</para>
/// <para>@作者：Jim</para>
/// <para>@创建时间：2023-12-13</para>
/// </summary>
public partial interface IInteContainerFreightRepository
{
    #region 查询

    /// <summary>
    /// 单条数据查询
    /// </summary>
    /// <param name="query"></param>
    /// <returns></returns>
    Task<InteContainerFreightEntity> GetOneAsync(InteContainerFreightQuery query);

    /// <summary>
    /// 数据集查询
    /// </summary>
    /// <param name="query"></param>
    /// <returns></returns>
    Task<IEnumerable<InteContainerFreightEntity>> GetListAsync(InteContainerFreightQuery query);

    /// <summary>
    /// 分页查询
    /// </summary>
    /// <param name="query"></param>
    /// <returns></returns>
    Task<PagedInfo<InteContainerFreightEntity>> GetPagedInfoAsync(InteContainerFreightPagedQuery query);

    #endregion

    #region 新增

    /// <summary>
    /// 创建数据
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    Task<int> InsertAsync(InteContainerFreightCreateCommand command);

    /// <summary>
    /// 批量创建数据
    /// </summary>
    /// <param name="commands"></param>
    /// <returns></returns>
    Task<int> InsertAsync(IEnumerable<InteContainerFreightCreateCommand> commands);

    #endregion

    #region 修改

    /// <summary>
    /// 更新数据
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    Task<int> UpdateAsync(InteContainerFreightUpdateCommand command);

    /// <summary>
    /// 批量更新数据
    /// </summary>
    /// <param name="commands"></param>
    /// <returns></returns>
    Task<int> UpdateAsync(IEnumerable<InteContainerFreightUpdateCommand> commands);

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
    Task<int> DeleteMoreAsync(DeleteMoreCommand commands);

    #endregion

    #region 扩展


    #endregion
}