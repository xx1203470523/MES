using Dapper;


namespace Hymson.MES.Data.Repositories.Qual;

/// <summary>
/// <para>@层级：仓储层</para>
/// <para>@作用：扩展实现</para>
/// <para>@描述：IQC检验项目详情;</para>
/// <para>@作者：Jim</para>
/// <para>@创建时间：2024-2-5</para>
/// </summary>
public partial class QualIqcInspectionItemDetailRepository
{
    /// <summary>
    /// 根据ID删除数据
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    public async Task<int> DeleteByQualIqcInspectionItemIdAsync(long qualIqcInspectionItemId)
    {
        using var conn = GetMESDbConnection();
        return await conn.ExecuteAsync(DeleteByQualIqcInspectionItemIdSql, new { QualIqcInspectionItemId = qualIqcInspectionItemId });
    }

    public async Task<int> DeleteByQualIqcInspectionItemIdsAsync(IEnumerable<long> qualIqcInspectionItemIds)
    {
        using var conn = GetMESDbConnection();
        return await conn.ExecuteAsync(DeleteByQualIqcInspectionItemIdsSql, new { QualIqcInspectionItemIds = qualIqcInspectionItemIds });
    }
}

/// <summary>
/// <para>@层级：仓储层</para>
/// <para>@作用：扩展SQL</para>
/// <para>@描述：IQC检验项目详情;</para>
/// <para>@作者：Jim</para>
/// <para>@创建时间：2024-2-5</para>
/// </summary>
public partial class QualIqcInspectionItemDetailRepository
{
    #region 删除

    const string DeleteByQualIqcInspectionItemIdSql = "UPDATE `qual_iqc_inspection_item_detail` SET IsDeleted = Id WHERE QualIqcInspectionItemId = @QualIqcInspectionItemId;";

    const string DeleteByQualIqcInspectionItemIdsSql = "UPDATE `qual_iqc_inspection_item_detail` SET IsDeleted = Id WHERE QualIqcInspectionItemId IN @QualIqcInspectionItemIds;";

    #endregion
}