using Hymson.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Data.Repositories.Report;


public class ProductDetailReportView : BaseEntity
{
    /// <summary>
    /// 工单Id
    /// </summary>
    public long? WorkOrderId { get; set; }

    /// <summary>
    /// 工单号
    /// </summary>
    public string? OrderCode { get; set; }

    /// <summary>
    /// 产品Id
    /// </summary>
    public long ProductId { get; set; }

    /// <summary>
    /// 工序Id
    /// </summary>
    public long ProcedureId { get; set; }

    /// <summary>
    /// 工序名称
    /// </summary>
    public string? ProcedureName { get; set; }


    /// <summary>
    /// 物料编码
    /// </summary>
    public string? MaterialCode { get; set; }

    /// <summary>
    /// 物料名称
    /// </summary>
    public string? MaterialName { get; set; }

    /// <summary>
    /// 查询类型
    /// </summary>
    public string? Type { get; set; }

    /// <summary>
    /// 查询日期
    /// </summary>
    public string? SearchDate { get; set; }

    /// <summary>
    /// 开始时间
    /// </summary>
    public string? StartDate { get; set; }

    /// <summary>
    /// 截至时间
    /// </summary>
    public string? EndDate { get; set; }

    /// <summary>
    /// 投入
    /// </summary>
    public decimal? FeedingQty { get; set; }

    /// <summary>
    /// 产出
    /// </summary>
    public decimal? OutputQty { get; set; }
}
