
using Hymson.Infrastructure;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.WMS.Core.Domain.Quality;

namespace Hymson.MES.Data.Repositories.Quality;

/// <summary>
/// <para>@层级：仓储层</para>
/// <para>@类型：接口</para>
/// <para>@描述：不合格代码关联不合格组表;</para>
/// <para>@作者：Jim</para>
/// <para>@创建时间：2024-3-12</para>
/// </summary>
public partial interface IQualUnqualifiedCodeGroupRelationRepository
{
    #region 查询

    /// <summary>
    /// 单条数据查询
    /// </summary>
    /// <param name="query"></param>
    /// <returns></returns>
    Task<QualUnqualifiedCodeGroupRelationEntity> GetOneAsync(QualUnqualifiedCodeGroupRelationQuery query);

    /// <summary>
    /// 数据集查询
    /// </summary>
    /// <param name="query"></param>
    /// <returns></returns>
    Task<IEnumerable<QualUnqualifiedCodeGroupRelationEntity>> GetListAsync(QualUnqualifiedCodeGroupRelationQuery query);

    /// <summary>
    /// 分页查询
    /// </summary>
    /// <param name="query"></param>
    /// <returns></returns>
    Task<PagedInfo<QualUnqualifiedCodeGroupRelationEntity>> GetPagedInfoAsync(QualUnqualifiedCodeGroupRelationPagedQuery query);

    #endregion

    #region 新增

    /// <summary>
    /// 创建数据
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    Task<int> InsertAsync(QualUnqualifiedCodeGroupRelationCreateCommand command);

    /// <summary>
    /// 批量创建数据
    /// </summary>
    /// <param name="commands"></param>
    /// <returns></returns>
    Task<int> InsertAsync(IEnumerable<QualUnqualifiedCodeGroupRelationCreateCommand> commands);

    #endregion

    #region 修改

    /// <summary>
    /// 更新数据
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    Task<int> UpdateAsync(QualUnqualifiedCodeGroupRelationUpdateCommand command);

    /// <summary>
    /// 批量更新数据
    /// </summary>
    /// <param name="commands"></param>
    /// <returns></returns>
    Task<int> UpdateAsync(IEnumerable<QualUnqualifiedCodeGroupRelationUpdateCommand> commands);

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

    #region 扩展


    #endregion
}