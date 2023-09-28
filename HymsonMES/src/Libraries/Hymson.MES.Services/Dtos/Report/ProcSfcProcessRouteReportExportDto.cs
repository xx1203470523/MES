using Confluent.Kafka;
using Hymson.Excel.Abstractions.Attributes;
using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;
using Mysqlx.Crud;
using OfficeOpenXml.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Services.Dtos.Report
{
    /// <summary>
    /// 生产工艺导出参数
    /// </summary>
    [SheetDescriptionAttribute("生产工艺")]
    public record ProcSfcProcessRouteReportExportDto : BaseExcelDto
    {
        /// <summary>
        /// 工序编码
        /// </summary>
        [EpplusTableColumn(Header = "工序编码", Order = 1)]
        public string ProcedureCode { get; set; }
        /// <summary>
        /// 工序名称
        /// </summary>
        [EpplusTableColumn(Header = "工序名称", Order = 2)]
        public string ProcedureName { get; set; }
        /// <summary>
        /// 当前状态;1：排队；2：激活；3：完工；
        /// </summary>
        [EpplusTableColumn(Header = "状态", Order = 3)]
        public string? CurrentStatusStr { get; set; }
        /// <summary>
        /// 当前数量
        /// </summary>
        [EpplusTableColumn(Header = "当前数量", Order = 4)]
        public decimal? Qty { get; set; }
        /// <summary>
        /// 是否合格
        /// </summary>
        [EpplusTableColumn(Header = "合格状态", Order = 5)]
        public string? Passed { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        [EpplusTableColumn(Header = "操作人", Order = 6)]
        public string CreatedBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        [EpplusTableColumn(Header = "操作时间", Order = 7)]
        public DateTime? CreatedOn { get; set; }
     
    }
}
