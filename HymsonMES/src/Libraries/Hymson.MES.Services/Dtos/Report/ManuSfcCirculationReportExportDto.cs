using Confluent.Kafka;
using Hymson.Excel.Abstractions.Attributes;
using Hymson.Infrastructure;
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
    /// 产品追朔导出信息
    /// </summary>
    [SheetDescriptionAttribute("产品追朔")]
    public record ManuSfcCirculationReportExportDto : BaseExcelDto
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
        /// 流转前条码
        /// </summary>
        [EpplusTableColumn(Header = "条码", Order = 3)]
        public string SFC { get; set; }
        /// <summary>
        /// 流转后条码信息
        /// </summary>
        [EpplusTableColumn(Header = "流转后条码", Order = 4)]
        public string CirculationBarCode { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        [EpplusTableColumn(Header = "操作人", Order = 5)]
        public string CreatedBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        [EpplusTableColumn(Header = "操作时间", Order = 6)]
        public DateTime? CreatedOn { get; set; }
    }
}
