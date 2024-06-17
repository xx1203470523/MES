using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Services.Dtos.Report;

/// <summary>
/// Ng记录报表查询参数
/// </summary>
public class ProductionDetailsReportQueryDto : PagerInfo
{
    /// <summary>
    /// 设备编码
    /// </summary>
    public string? EquipmentCode{ get; set; }

    /// <summary>
    /// 工序
    /// </summary>
    public IEnumerable<long>? ProcedureId { get; set; }

    /// <summary>
    /// 产品条码
    /// </summary>
    public string? Sfc { get; set; }

    /// <summary>
    /// 日期
    /// </summary>
    public DateTime[]? DateList { get; set; }

    /// <summary>
    /// 起始时间
    /// </summary>
    public DateTime? BeginTime { get; set; }

    /// <summary>
    /// 截止日期
    /// </summary>
    public DateTime? EndTime { get; set; }

    /// <summary>
    /// 是否合格状态
    /// </summary>
    public TrueOrFalseEnum? QualityStatus { get; set; }
}

/// <summary>
/// Ng记录报表分页查询参数
/// </summary>
public class ProductionDetailsReportPageQueryDto : PagerInfo
{
    /// <summary>
    /// 设备编码
    /// </summary>
    public string? EquipmentCode { get; set; }

    /// <summary>
    /// 工序
    /// </summary>
    public long[]? ProcedureId { get; set; }

    /// <summary>
    /// 产品条码
    /// </summary>
    public string? SFC { get; set; }

    /// <summary>
    /// 日期
    /// </summary>
    public DateTime[]? DateList { get; set; }

    /// <summary>
    /// 起始时间
    /// </summary>
    public DateTime? BeginTime { get; set; }

    /// <summary>
    /// 截止日期
    /// </summary>
    public DateTime? EndTime { get; set; }

    /// <summary>
    /// 是否合格状态
    /// </summary>
    public TrueOrFalseEnum? QualityStatus { get; set; }
}