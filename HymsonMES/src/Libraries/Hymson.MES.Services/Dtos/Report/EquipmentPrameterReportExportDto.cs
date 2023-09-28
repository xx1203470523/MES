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
    /// 设备参数导出
    /// </summary>
    [SheetDescriptionAttribute("设备参数")]
    public record EquipmentPrameterReportExportDto : BaseExcelDto
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
        /// 资源编码
        /// </summary>
        [EpplusTableColumn(Header = "资源编码", Order = 3)]
        public string ResourceCode { get; set; }
        /// <summary>
        /// 资源名称
        /// </summary>
        [EpplusTableColumn(Header = "资源名称", Order = 4)]
        public string ResourceName { get; set; }
        /// <summary>
        /// 设备编码
        /// </summary>
        [EpplusTableColumn(Header = "设备编码", Order = 5)]
        public string EquipmentCode { get; set; }
        /// <summary>
        /// 设备名称
        /// </summary>
        [EpplusTableColumn(Header = "设备名称", Order = 6)]
        public string EquipmentName { get; set; }
        /// <summary>
        /// 参数编码
        /// </summary>
        [EpplusTableColumn(Header = "参数编码", Order = 7)]
        public string ParameterCode { get; set; }
        /// <summary>
        /// 参数名称
        /// </summary>
        [EpplusTableColumn(Header = "参数名称", Order = 8)]
        public string ParameterName { get; set; }
        /// <summary>
        /// 参数值
        /// </summary>
        [EpplusTableColumn(Header = "参数值", Order = 9)]
        public string ParameterValue { get; set; }
        /// <summary>
        /// 上报时间
        /// </summary>
        [EpplusTableColumn(Header = "上报时间", Order = 10)]
        public DateTime CreatedOn { get; set; }
    }
}
