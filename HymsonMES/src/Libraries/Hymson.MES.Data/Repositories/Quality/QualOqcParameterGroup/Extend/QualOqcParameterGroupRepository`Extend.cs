using Dapper;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Qual;


namespace Hymson.MES.Data.Repositories.Quality;

public partial class QualOqcParameterGroupRepository
{
    #region 新增

    /// <summary>
    /// 创建数据
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    public async Task<int> InsertIgnoreAsync(QualOqcParameterGroupCreateCommand command)
    {
        //using var conn = GetMESDbConnection();
        using var conn = GetMESDbConnection();
        return await conn.ExecuteAsync(InsertIgnoreSql, command);
    }

    /// <summary>
    /// 批量创建数据
    /// </summary>
    /// <param name="commands"></param>
    /// <returns></returns>
    public async Task<int> InsertIgnoreAsync(IEnumerable<QualOqcParameterGroupCreateCommand> commands)
    {
        //using var conn = GetMESDbConnection();
        using var conn = GetMESDbConnection();
        return await conn.ExecuteAsync(InsertIgnoreSql, commands);
    }

    #endregion

    #region 修改

    /// <summary>
    /// 更新数据
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    public async Task<int> UpdateAsync(QualOqcParameterGroupUpdateCommand command)
    {
        using var conn = GetMESDbConnection();
        return await conn.ExecuteAsync(UpdateSql, command);
    }

    /// <summary>
    /// 批量更新数据
    /// </summary>
    /// <param name="commands"></param>
    /// <returns></returns>
    public async Task<int> UpdateAsync(IEnumerable<QualOqcParameterGroupUpdateCommand> commands)
    {
        using var conn = GetMESDbConnection();
        return await conn.ExecuteAsync(UpdatesSql, commands);
    }

    #endregion

    #region 删除

    /// <summary>
    /// 根据ID删除数据
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    public async Task<int> DeleteAsync(DeleteCommand command)
    {
        using var conn = GetMESDbConnection();
        return await conn.ExecuteAsync(DeleteSql, command);
    }

    /// <summary>
    /// 根据ID删除多条数据
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    public async Task<int> DeleteMoreAsync(DeleteCommand command)
    {
        using var conn = GetMESDbConnection();
        return await conn.ExecuteAsync(DeletesSql, command);
    }

    #endregion


}

/// <summary>
/// <para>@层级：仓储层</para>
/// <para>@作用：扩展SQL</para>
/// <para>@描述：IQC检验项目;</para>
/// <para>@作者：Jim</para>
/// <para>@创建时间：2024-2-5</para>
/// </summary>
public partial class QualOqcParameterGroupRepository
{
    #region 新增
#if DM
    const string InsertIgnoreSql = "INSERT  INTO `qual_oqc_parameter_group` (`Id`,`Code`,`Name`,`MaterialId`,`CustomerId`,`Version`,`Status`,`Remark`,`CreatedOn`,`CreatedBy`,`UpdatedOn`,`UpdatedBy`,`SiteId`,`IsDeleted`) VALUES (@Id,@Code,@Name,@MaterialId,@CustomerId,@Version,@Status,@Remark,@CreatedOn,@CreatedBy,@UpdatedOn,@UpdatedBy,@SiteId,@IsDeleted);";
#else
    const string InsertIgnoreSql = "INSERT IGNORE INTO `qual_oqc_parameter_group` (`Id`,`Code`,`Name`,`MaterialId`,`CustomerId`,`Version`,`Status`,`Remark`,`CreatedOn`,`CreatedBy`,`UpdatedOn`,`UpdatedBy`,`SiteId`,`IsDeleted`) VALUES (@Id,@Code,@Name,@MaterialId,@CustomerId,@Version,@Status,@Remark,@CreatedOn,@CreatedBy,@UpdatedOn,@UpdatedBy,@SiteId,@IsDeleted);";

#endif
    #endregion
}