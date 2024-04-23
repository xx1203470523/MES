using Confluent.Kafka;
using Hymson.Excel.Abstractions.Attributes;
using Hymson.Infrastructure;
using Mysqlx.Crud;
using OfficeOpenXml.Attributes;

namespace Hymson.MES.Services.Dtos.QualificationRateReport
{
    /// <summary>
    /// 合格率报表Dto
    /// </summary>
    public record QualificationRateReportDto : BaseEntityDto
    {
       /// <summary>
        /// 工单编码
        /// </summary>
        public string OrderCode { get; set; }

       /// <summary>
        /// 产品信息
        /// </summary>
        public string MaterialName { get; set; }

       /// <summary>
        /// 工序id
        /// </summary>
        public string ProcedureName { get; set; }

       /// <summary>
        /// 开始时间
        /// </summary>
        public string StartOn { get; set; }

       /// <summary>
        /// 结束时间
        /// </summary>
        public string EndOn { get; set; }

       /// <summary>
        /// 合格数
        /// </summary>
        public decimal QualifiedQuantity { get; set; }

        /// <summary>
        /// 一次合格数
        /// </summary>
        public decimal? OneQualifiedQuantity { get; set; }

        /// <summary>
        /// 不合格数
        /// </summary>
        public decimal UnQualifiedQuantity { get; set; }

        /// <summary>
        /// 合格率
        /// </summary>
        public decimal QualifiedRate { get; set; }

        /// <summary>
        /// 合格率
        /// </summary>
        public decimal? OneQualifiedRate { get; set; }
    }

    /// <summary>
    /// 合格率报表分页Dto
    /// </summary>
    public class QualificationRateReportPagedQueryDto : PagerInfo 
    {
        /// <summary>
        /// 工单号
        /// </summary>
        public string? OrderCode { get; set;}

        /// <summary>
        /// 工序Ids
        /// </summary>
        public long[]? ProcedureIds { get; set;}

        /// <summary>
        /// 查询日期类型（日月年）
        /// </summary>
        public int? Type { get; set; }

        /// <summary>
        /// 日期
        /// </summary>
        public DateTime[]? Date { get; set;}
    }

    /// <summary>
    /// 合格率报表导出
    /// </summary>
    [SheetDescription("合格率报表")]
    public record QualificationRateExportDto : BaseExcelDto
    {
        /// <summary>
        /// 工单号
        /// </summary>
        [EpplusTableColumn(Header = "工单号", Order = 1)]
        public string? OrderCode { get; set; }

        /// <summary>
        /// 物料名称
        /// </summary>
        [EpplusTableColumn(Header = "物料名称", Order = 2)]
        public string? MaterialName { get; set; }

        /// <summary>
        /// 物料名称
        /// </summary>
        [EpplusTableColumn(Header = "工序名称", Order = 3)]
        public string? ProcedureName { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        [EpplusTableColumn(Header = "开始时间", Order = 4)]
        public string? StartOn { get; set; }

        /// <summary>
        /// 截至时间
        /// </summary>
        [EpplusTableColumn(Header = "结束时间", Order = 5)]
        public string? EndOn { get; set; }

        /// <summary>
        /// 合格数
        /// </summary>
        [EpplusTableColumn(Header = "合格数", Order = 6)]
        public decimal? QualifiedQuantity { get; set; }

        /// <summary>
        /// 一次合格数
        /// </summary>
        [EpplusTableColumn(Header = "一次合格数", Order = 6)]
        public decimal? OneQualifiedQuantity { get; set; }

        /// <summary>
        /// 不合格数
        /// </summary>
        [EpplusTableColumn(Header = "不合格数", Order = 7)]
        public decimal? UnQualifiedQuantity { get; set; }

        /// <summary>
        /// 合格率
        /// </summary>
        [EpplusTableColumn(Header = "合格率", Order = 8)]
        public decimal? QualifiedRate { get; set; }

        /// <summary>
        /// 一次合格率
        /// </summary>
        [EpplusTableColumn(Header = "一次合格率", Order = 9)]
        public decimal? OneQualifiedRate { get; set; }

    }
}
