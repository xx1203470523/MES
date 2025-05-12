using Confluent.Kafka;
using Hymson.Excel.Abstractions.Attributes;
using Hymson.Infrastructure;
using Hymson.MES.Core.Enums.Manufacture;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
    /// 条码履历导出信息
    /// </summary>
    [SheetDescriptionAttribute("条码履历")]
    public record ProductTracePagedReportExportDto : BaseExcelDto
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
        /// 条码
        /// </summary>
        [EpplusTableColumn(Header = "条码", Order = 3)]
        public string SFC { get; set; }
        /// <summary>
        /// 资源编码
        /// </summary>
        [EpplusTableColumn(Header = "资源编码", Order = 4)]
        public string ResourceCode { get; set; }
        /// <summary>
        /// 资源名称
        /// </summary>
        [EpplusTableColumn(Header = "资源名称", Order = 5)]
        public string ResourceName { get; set; }
        /// <summary>
        /// 设备编码
        /// </summary>
        [EpplusTableColumn(Header = "设备编码", Order = 6)]
        public string EquipmentCode { get; set; }
        /// <summary>
        /// 设备名称
        /// </summary>
        [EpplusTableColumn(Header = "设备名称", Order = 7)]
        public string EquipmentName { get; set; }
        /// <summary>
        /// 内阻
        /// </summary>
        [EpplusTableColumn(Header = "内阻", Order = 7)]
        public string ParameterValue1 { get; set; }
        /// <summary>
        /// 电压
        /// </summary>
        [EpplusTableColumn(Header = "电压", Order = 8)]
        public string ParameterValue2 { get; set; }
        /// <summary>
        /// 批次号
        /// </summary>
        [EpplusTableColumn(Header = "批次号", Order = 9)]
        public string BatchNo { get; set; }
        /// <summary>
        /// 步骤类型
        /// </summary>
        [EpplusTableColumn(Header = "操作类型", Order = 10)]
        public string OperatetypeStr { get; set; }

        /// <summary>
        /// 操作时间
        /// </summary>
        [EpplusTableColumn(Header = "操作时间", Order = 11)]
        public DateTime CreatedOn { get; set; }
    }

    /// <summary>
    /// 条码履历导入模板
    /// </summary>
    public record ManuSfcStepImportDto : BaseExcelDto
    {
        /// <summary>
        /// 条码
        /// </summary>
        [EpplusTableColumn(Header = "条码", Order = 1)]
        public string SFC { get; set; }

        /// <summary>
        /// 工序编码
        /// </summary>
        [EpplusTableColumn(Header = "工序编码", Order = 2)]
        public string ProcedureCode { get; set; }

        /// <summary>
        /// 资源编码
        /// </summary>
        [EpplusTableColumn(Header = "资源编码", Order = 3)]
        public string ResourceCode { get; set; }

        /// <summary>
        /// 设备编码
        /// </summary>
        [EpplusTableColumn(Header = "设备编码", Order = 4)]
        public string EquipmentCode { get; set; }

        /// <summary>
        /// 进出/出站
        /// </summary>
        [EpplusTableColumn(Header = "进出/出站", Order = 5)]
        public string Status { get; set; }

        /// <summary>
        /// 操作时间（默认导入时间）
        /// </summary>
        [EpplusTableColumn(Header = "操作时间（默认导入时间）", Order = 6)]
        public string OpertiaonDate { get; set; }
    }

    /// <summary>
    /// 条码履历导入
    /// </summary>
    public class UploadManuSfcStepDto
    {
        [FromForm(Name = "file")]
        public IFormFile File { get; set; }
    }
}
