using Hymson.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Data.Repositories.Report;

public class NgRecordReportQuery : PagerInfo
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
    /// 工序Id
    /// </summary>
    public long? ProcedureId { get; set; }

    /// <summary>
    /// 工序Ids
    /// </summary>
    public IEnumerable<long>? ProcedureIds { get; set; }

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

}

public class NgRecordReportPageQuery : PagerInfo
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
    /// 工序Id
    /// </summary>
    public long? ProcedureId { get; set; }

    /// <summary>
    /// 工序Ids
    /// </summary>
    public IEnumerable<long>? ProcedureIds { get; set; }

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
}