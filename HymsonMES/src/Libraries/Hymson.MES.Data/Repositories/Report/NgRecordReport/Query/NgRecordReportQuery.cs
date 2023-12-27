using Hymson.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Data.Repositories.Report;

public class NgRecordReportQuery
{
    /// <summary>
    /// 设备编码
    /// </summary>
    public string? EquipmentCode { get; set; }

    /// <summary>
    /// 工序
    /// </summary>
    public string? ProdureId { get; set; }

    /// <summary>
    /// 产品条码
    /// </summary>
    public string? Sfc { get; set; }

    /// <summary>
    /// 日期
    /// </summary>
    public IEnumerable<DateTime>? DateList { get; set; }

    /// <summary>
    /// 起始时间
    /// </summary>
    public DateTime? BeginTime { get; set; }

    /// <summary>
    /// 截止日期
    /// </summary>
    public DateTime? EndTime { get; set; }
}

public class NgRecordReportPageQuery : PagerInfo
{

}