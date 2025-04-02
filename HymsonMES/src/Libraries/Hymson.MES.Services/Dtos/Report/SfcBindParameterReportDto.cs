using Hymson.Excel.Abstractions.Attributes;
using Hymson.Infrastructure;
using OfficeOpenXml.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Services.Dtos.Report
{
    public class SfcBindParameterReportDto
    {

    }

    /// <summary>
    /// 查询参数
    /// </summary>
    public class SfcBindParameterReportPagedQueryDto : PagerInfo
    {
        /// <summary>
        /// 工单号
        /// </summary>
        public string OrderCode { get; set; }
    }

    /// <summary>
    /// 查询结果
    /// </summary>
    public record SfcBindParameterReportPagedDataDto : BaseEntityDto
    {
        /// <summary>
        /// 工单号
        /// </summary>
        public string OrderCode { get; set; }

        /// <summary>
        /// 条码
        /// </summary>
        public string Sfc { get; set; }

        /// <summary>
        /// 流转条码
        /// </summary>
        public string CirculationBarcode { get; set; }

        /// <summary>
        /// 工序编码
        /// </summary>
        public string ProcedureCode { get; set; }

        /// <summary>
        /// 工序名称
        /// </summary>
        public string ProcedureName { get; set; }

        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProductCode { get; set; }

        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// TORQUE1
        /// </summary>
        public decimal Column1 { get; set; }

        /// <summary>
        /// TORQUE2
        /// </summary>
        public decimal Column2 { get; set; }

        /// <summary>
        /// TORQUE3
        /// </summary>
        public decimal Column3 { get; set; }

        /// <summary>
        /// TORQUE4
        /// </summary>
        public decimal Column4 { get; set; }

        /// <summary>
        /// TORQUE5
        /// </summary>
        public decimal Column5 { get; set; }

        /// <summary>
        /// TORQUE6
        /// </summary>
        public decimal Column6 { get; set; }

        /// <summary>
        /// TORQUE7
        /// </summary>
        public decimal Column7 { get; set; }

        /// <summary>
        /// TORQUE8
        /// </summary>
        public decimal Column8 { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public string CreatedBy { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedOn { get; set; }
    }

    /// <summary>
    /// 报表导出
    /// </summary>
    [SheetDescriptionAttribute("条码绑定关系扭矩参数报表")]
    public record SfcBindParameterReportExportDto : BaseExcelDto
    {
        /// <summary>
        /// 工单号
        /// </summary>
        [EpplusTableColumn(Header = "工单号", Order = 1)]
        public string OrderCode { get; set; }

        /// <summary>
        /// 条码
        /// </summary>
        [EpplusTableColumn(Header = "条码", Order = 2)]
        public string Sfc { get; set; }

        /// <summary>
        /// 流转条码
        /// </summary>
        [EpplusTableColumn(Header = "流转条码", Order = 3)]
        public string CirculationBarcode { get; set; }

        /// <summary>
        /// 工序编码
        /// </summary>
        [EpplusTableColumn(Header = "工序编码", Order = 3)]
        public string ProcedureCode { get; set; }

        /// <summary>
        /// 工序名称
        /// </summary>
        [EpplusTableColumn(Header = "工序名称", Order = 4)]
        public string ProcedureName { get; set; }

        /// <summary>
        /// 产品名称
        /// </summary>
        [EpplusTableColumn(Header = "产品名称", Order = 5)]
        public string ProductCode { get; set; }

        /// <summary>
        /// 产品名称
        /// </summary>
        [EpplusTableColumn(Header = "产品名称", Order = 6)]
        public string ProductName { get; set; }

        /// <summary>
        /// TORQUE1
        /// </summary>
        [EpplusTableColumn(Header = "TORQUE1", Order = 7)]
        public decimal Column1 { get; set; }

        /// <summary>
        /// TORQUE2
        /// </summary>
        [EpplusTableColumn(Header = "TORQUE2", Order = 8)]
        public decimal Column2 { get; set; }

        /// <summary>
        /// TORQUE3
        /// </summary>
        [EpplusTableColumn(Header = "TORQUE3", Order = 9)]
        public decimal Column3 { get; set; }

        /// <summary>
        /// TORQUE4
        /// </summary>
        [EpplusTableColumn(Header = "TORQUE4", Order = 10)]
        public decimal Column4 { get; set; }

        /// <summary>
        /// TORQUE5
        /// </summary>
        [EpplusTableColumn(Header = "TORQUE5", Order = 11)]
        public decimal Column5 { get; set; }

        /// <summary>
        /// TORQUE6
        /// </summary>
        [EpplusTableColumn(Header = "TORQUE6", Order = 12)]
        public decimal Column6 { get; set; }

        /// <summary>
        /// TORQUE7
        /// </summary>
        [EpplusTableColumn(Header = "TORQUE7", Order = 13)]
        public decimal Column7 { get; set; }

        /// <summary>
        /// TORQUE8
        /// </summary>
        [EpplusTableColumn(Header = "TORQUE8", Order = 14)]
        public decimal Column8 { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        [EpplusTableColumn(Header = "创建人", Order = 15)]
        public string CreatedBy { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [EpplusTableColumn(Header = "创建时间", Order = 16)]
        public DateTime CreatedOn { get; set; }
    }
}
