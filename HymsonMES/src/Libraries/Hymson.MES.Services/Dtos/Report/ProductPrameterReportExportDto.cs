using AutoMapper.Execution;
using Confluent.Kafka;
using Google.Protobuf.WellKnownTypes;
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
    /// 产品参数导出
    /// </summary>
    [SheetDescriptionAttribute("产品参数")]
    public record ProductPrameterReportExportDto : BaseExcelDto
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
        /// 采集时间
        /// </summary>
        [EpplusTableColumn(Header = "采集时间", Order = 10)]
        public DateTime CreatedOn { get; set; }
        /// <summary>
        /// 采集人
        /// </summary>
        [EpplusTableColumn(Header = "采集人", Order = 11)]
        public string CreatedBy { get; set; }
    }
}