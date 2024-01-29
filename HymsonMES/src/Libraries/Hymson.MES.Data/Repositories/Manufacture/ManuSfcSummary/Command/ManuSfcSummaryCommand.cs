
using Hymson.MES.Data.Repositories.Common;
using Hymson.MES.Data.Repositories.Common.Command;

namespace Hymson.WMS.Data.Repositories.ManuManufacture;

/// <summary>
/// <para>@层级：仓储层</para>
/// <para>@类型：创建指令</para>
/// <para>@描述：生产汇总表;标准创建对象</para>
/// <para>@作者：Jim</para>
/// <para>@创建时间：2024-1-25</para>
/// </summary>
public class ManuSfcSummaryCreateCommand : BaseCommand
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
/// <para>@层级：仓储层</para>
/// <para>@类型：更新指令</para>
/// <para>@描述：生产汇总表;标准更新对象</para>
/// <para>@作者：Jim</para>
/// <para>@创建时间：2024-1-25</para>
/// </summary>
public class ManuSfcSummaryUpdateCommand : UpdateCommand
{
    
    /// <summary>
    /// 主键
    /// </summary>
    public long? Id { get; set; }
    
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