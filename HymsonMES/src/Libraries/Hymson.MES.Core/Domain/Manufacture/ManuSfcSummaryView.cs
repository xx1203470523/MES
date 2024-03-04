using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Core.Domain.Manufacture;

/// <summary>
/// ManuSfcSummary汇总数据
/// </summary>
public class ManuSfcSummaryView
{
    /// <summary>
    /// 工单Id
    /// </summary>
    public long? WorkOrderId { get; set; }

    /// <summary>
    /// 产出数
    /// </summary>
    public decimal? OutputQty { get; set; }

    /// <summary>
    /// 良品数量
    /// </summary>
    public decimal? QualifiedQty { get; set; }

    /// <summary>
    /// 一次良品数量
    /// </summary>
    public decimal? OneQuanlifiedQty { get; set; }
}
