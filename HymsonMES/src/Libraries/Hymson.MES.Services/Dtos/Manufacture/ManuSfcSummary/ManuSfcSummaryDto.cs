
using Hymson.Infrastructure;

namespace Hymson.MES.Services.Dtos.Manu;

/// <summary>
/// <para>@描述：生产汇总表; 基础数据传输对象</para>
/// <para>@作者：Jim</para>
/// <para>@创建时间：2024-1-25</para>
/// <para><seealso cref="BaseEntityDto">点击查看享元对象</seealso></para>
/// </summary>
public record ManuSfcSummaryDto : BaseEntityDto
{
    /// <summary>
    /// 站点ID
    /// </summary>
    public long? SiteId { get; set; }

    /// <summary>
    /// 当前工序id
    /// </summary>
    public long? ProcedureId { get; set; }

    /// <summary>
    /// 资源id
    /// </summary>
    public long? ResourceId { get; set; }

    /// <summary>
    /// 设备Id
    /// </summary>
    public long? EquipmentId { get; set; }

    /// <summary>
    /// 条码
    /// </summary>
    public string? SFC { get; set; }

    /// <summary>
    /// 工单id
    /// </summary>
    public long? WorkOrderId { get; set; }

    /// <summary>
    /// 产品id
    /// </summary>
    public long? ProductId { get; set; }

    /// <summary>
    /// 投入时间;投入时间
    /// </summary>
    public DateTime? BeginTime { get; set; }

    /// <summary>
    /// 产出时间;产出时间
    /// </summary>
    public DateTime? EndTime { get; set; }

    /// <summary>
    /// 复投次数;复投次数
    /// </summary>
    public long? RepeatedCount { get; set; }

    /// <summary>
    /// 产出数量;产出数量
    /// </summary>
    public decimal? Qty { get; set; }

    /// <summary>
    /// 复投时NG次数;复投时NG次数
    /// </summary>
    public long? NgNum { get; set; }

    /// <summary>
    /// 第一次的品质状态;1 第一次合格，0 第一次不合格
    /// </summary>
    public long? FirstQualityStatus { get; set; }

    /// <summary>
    /// 最终品质状态;1 合格，0 不合格
    /// </summary>
    public long? QualityStatus { get; set; }

    /// <summary>
    /// 0未补料,1已补料
    /// </summary>
    public long? IsReplenish { get; set; }

}

/// <summary>
/// <para>@描述：生产汇总表; 用于更新数据的数据传输对象</para>
/// <para>@作者：Jim</para>
/// <para>@创建时间：2024-1-25</para>
/// <para><seealso cref="ManuSfcSummaryDto">点击查看享元对象</seealso></para>
/// </summary>
public record ManuSfcSummaryUpdateDto : ManuSfcSummaryDto
{
    /// <summary>
    /// Id
    /// </summary>
    public long? Id { get; set; }
}

/// <summary>
/// <para>@描述：生产汇总表; 用于页面展示的数据传输对象</para>
/// <para>@作者：Jim</para>
/// <para>@创建时间：2024-1-25</para>
/// <para><seealso cref="ManuSfcSummaryUpdateDto">点击查看享元对象</seealso></para>
/// </summary>
public record ManuSfcSummaryOutputDto : ManuSfcSummaryUpdateDto
{
    /// <summary>
    /// 创建人
    /// </summary>
    public string? CreatedBy { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime? CreatedOn { get; set; }

    /// <summary>
    /// 更新人
    /// </summary>
    public string? UpdatedBy { get; set; }

    /// <summary>
    /// 更新时间
    /// </summary>
    public DateTime? UpdatedOn { get; set; }

}

/// <summary>
/// <para>@描述：生产汇总表; 用于删除数据的数据传输对象</para>
/// <para>@作者：Jim</para>
/// <para>@创建时间：2024-1-25</para>
/// </summary>
public record ManuSfcSummaryDeleteDto : ManuSfcSummaryDto
{
    /// <summary>
    /// 要删除的组
    /// </summary>
    public IEnumerable<long> Ids { get; set; }
}

/// <summary>
/// <para>@描述：生产汇总表; 用于条件查询（分页）的数据传输对象</para>
/// <para>@作者：Jim</para>
/// <para>@创建时间：2024-1-25</para>
/// <para><seealso cref="PagerInfo">点击查看享元对象</seealso></para>
/// </summary>
public class ManuSfcSummaryPagedQueryDto : PagerInfo
{
}

/// <summary>
/// <para>@描述：生产汇总表; 用于条件查询（分页）的数据传输对象</para>
/// <para>@作者：Jim</para>
/// <para>@创建时间：2024-1-25</para>
/// <para><seealso cref="PagerInfo">点击查看享元对象</seealso></para>
/// </summary>
public class ManuSfcSummaryQueryDto
{
}