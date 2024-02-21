using Dapper;

using MySql.Data.MySqlClient;

namespace Hymson.MES.Data.Repositories.Qual;

/// <summary>
/// <para>@层级：仓储层</para>
/// <para>@作用：扩展实现</para>
/// <para>@描述：IQC检验项目;</para>
/// <para>@作者：Jim</para>
/// <para>@创建时间：2024-2-5</para>
/// </summary>
public partial class QualIqcInspectionItemRepository
{
    #region 新增

    /// <summary>
    /// 创建数据
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    public async Task<int> InsertIgnoreAsync(QualIqcInspectionItemCreateCommand command)
    {
        using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
        return await conn.ExecuteAsync(InsertIgnoreSql, command);
    }

    /// <summary>
    /// 批量创建数据
    /// </summary>
    /// <param name="commands"></param>
    /// <returns></returns>
    public async Task<int> InsertIgnoreAsync(IEnumerable<QualIqcInspectionItemCreateCommand> commands)
    {
        using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
        return await conn.ExecuteAsync(InsertIgnoreSql, commands);
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
public partial class QualIqcInspectionItemRepository
{
    #region 新增

    const string InsertIgnoreSql = "INSERT IGNORE INTO `qual_iqc_inspection_item` (`Id`,`Code`,`Name`,`MaterialId`,`SupplierId`,`Status`,`Remark`,`CreatedOn`,`CreatedBy`,`UpdatedOn`,`UpdatedBy`,`SiteId`,`IsDeleted`) VALUES (@Id,@Code,@Name,@MaterialId,@SupplierId,@Status,@Remark,@CreatedOn,@CreatedBy,@UpdatedOn,@UpdatedBy,@SiteId,@IsDeleted);";

    #endregion
}