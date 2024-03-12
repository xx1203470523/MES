
using Hymson.Infrastructure;

namespace Hymson.WMS.Core.Domain.Quality;

/// <summary>
/// <para>@描述：不合格代码关联不合格组表;</para>
/// <para>@作者：Jim</para>
/// <para>@创建时间：2024-3-12</para>
/// <para><seealso cref="BaseEntity">点击查看享元对象</seealso></para>
/// </summary>
public class QualUnqualifiedCodeGroupRelationEntity : BaseEntity
{

    /// <summary>
    /// 所属站点代码
    /// </summary>
    public long SiteId { get; set; }


    /// <summary>
    /// 所属不合格代码ID
    /// </summary>
    public long UnqualifiedCodeId { get; set; }


    /// <summary>
    /// 所属不合格组ID
    /// </summary>
    public long UnqualifiedGroupId { get; set; }


    /// <summary>
    /// 说明
    /// </summary>
    public string? Remark { get; set; }

}