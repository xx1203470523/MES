namespace Hymson.MES.Data.Repositories.Qual;

/// <summary>
/// <para>@层级：仓储层</para>
/// <para>@类型：接口</para>
/// <para>@描述：IQC检验项目;</para>
/// <para>@作者：Jim</para>
/// <para>@创建时间：2024-2-5</para>
/// </summary>
public partial interface IQualIqcInspectionItemRepository
{
    /// <summary>
    /// 数据插入（过滤出错行）
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    Task<int> InsertIgnoreAsync(QualIqcInspectionItemCreateCommand command);
    
    /// <summary>
    /// 数据批量插入（过滤出错行）
    /// </summary>
    /// <param name="commands"></param>
    /// <returns></returns>
    Task<int> InsertIgnoreAsync(IEnumerable<QualIqcInspectionItemCreateCommand> commands);
}