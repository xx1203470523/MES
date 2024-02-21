namespace Hymson.MES.Data.Repositories.Qual;

/// <summary>
/// <para>@层级：仓储层</para>
/// <para>@类型：接口</para>
/// <para>@描述：IQC检验项目详情; 扩展</para>
/// <para>@作者：Jim</para>
/// <para>@创建时间：2024-2-5</para>
/// </summary>
public partial interface IQualIqcInspectionItemDetailRepository
{
    /// <summary>
    /// 根据检验项目ID来删除明细
    /// </summary>
    /// <param name="qualIqcInspectionItemId"></param>
    /// <returns></returns>
    Task<int> DeleteByQualIqcInspectionItemIdAsync(long qualIqcInspectionItemId);

    /// <summary>
    /// 根据多个检验项目ID来删除明细
    /// </summary>
    /// <param name="qualIqcInspectionItemIds"></param>
    /// <returns></returns>
    Task<int> DeleteByQualIqcInspectionItemIdsAsync(IEnumerable<long> qualIqcInspectionItemIds);
}