using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Data.Repositories.Report;

public class ProductionDetailsReportQuery : PagerInfo
{

    /// <summary>
    /// 产品条码
    /// </summary>
    public string SFC { get; set; }

    /// <summary>
    /// 设备编码
    /// </summary>
    public string? EquipmentCode { get; set; }

    /// <summary>
    /// 设备Id
    /// </summary>
    public long? EquipmentId { get; set; }

    /// <summary>
    /// 设备Ids
    /// </summary>
    public IEnumerable<long>? EquipmentIds { get; set; }

    /// <summary>
    /// 工序Ids
    /// </summary>
    public IEnumerable<long>? ProcedureId { get; set; }

    /// <summary>
    /// 资源Id
    /// </summary>
    public long? ResourceId { get; set; }

    /// <summary>
    /// 资源Ids
    /// </summary>
    public IEnumerable<long>? ResourceIds { get; set; }

    /// <summary>
    /// 开始时间
    /// </summary>
    public DateTime? BeginTime { get; set; }

    /// <summary>
    /// 截至时间
    /// </summary>
    public DateTime? EndTime { get; set; }

    /// <summary>
    /// 日期
    /// </summary>
    public DateTime[]? DateList { get; set; }

    /// <summary>
    /// 是否合格状态
    /// </summary>
    public TrueOrFalseEnum? QualityStatus { get; set; }

}

public class ProductionDetailsReportPageQuery : PagerInfo
{
    /// <summary>
    /// 产品条码
    /// </summary>
    public string SFC { get; set; }

    /// <summary>
    /// 设备编码
    /// </summary>
    public string? EquipmentCode { get; set; }

    /// <summary>
    /// 设备Id
    /// </summary>
    public long? EquipmentId { get; set; }

    /// <summary>
    /// 设备Ids
    /// </summary>
    public IEnumerable<long>? EquipmentIds { get; set; }

    /// <summary>
    /// 工序Ids
    /// </summary>
    public IEnumerable<long>? ProcedureId { get; set; }

    /// <summary>
    /// 资源Id
    /// </summary>
    public long? ResourceId { get; set; }

    /// <summary>
    /// 资源Ids
    /// </summary>
    public IEnumerable<long>? ResourceIds { get; set; }

    /// <summary>
    /// 开始时间
    /// </summary>
    public DateTime? BeginTime { get; set; }

    /// <summary>
    /// 截至时间
    /// </summary>
    public DateTime? EndTime { get; set; }

    /// <summary>
    /// 日期
    /// </summary>
    public DateTime[]? DateList { get; set; }

    /// <summary>
    /// 是否合格状态
    /// </summary>
    public TrueOrFalseEnum? QualityStatus { get; set; }
}