using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;
using OfficeOpenXml.Attributes;

namespace Hymson.MES.Services.Dtos.Report
{
    /// <summary>
    /// 降级品明细报表Dto
    /// </summary>
    public record ManuDowngradingDetailReportDto : BaseEntityDto
    {
        /// <summary>
        /// 工作中心Code
        /// </summary>
        public string? WorkCenterCode { get; set; }

        /// <summary>
        /// 条码
        /// </summary>
        public string SFC { get; set; }

        /// <summary>
        /// 产品编码
        /// </summary>
        public string ProductCode { get; set; }

        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// 降级编码
        /// </summary>
        public string? DowngradingCode { get; set; }

        /// <summary>
        /// 降级名称
        /// </summary>
        public string? DowngradingName { get; set; }

        /// <summary>
        /// 降级描述
        /// </summary>
        public string? DowngradingRemark { get; set; }

        /// <summary>
        /// 录入人员
        /// </summary>
        public string? UpdatedBy { get; set; }

        /// <summary>
        /// 录入时间
        /// </summary>
        public DateTime? UpdatedOn { get; set; }

        /// <summary>
        /// 工单编号
        /// </summary>
        public string OrderCode { get; set; }

        /// <summary>
        /// 工单类型
        /// </summary>
        public PlanWorkOrderTypeEnum OrderType { get; set; }

        /// <summary>
        /// 工序编码
        /// </summary>
        public string? ProcedureCode { get; set; }

        /// <summary>
        /// 工序名称
        /// </summary>
        public string? ProcedureName { get; set; }


    }

    /// <summary>
    /// 降级品明细报表分页Dto
    /// </summary>
    public class ManuDowngradingDetailReportPagedQueryDto : PagerInfo
    {
        /// <summary>
        /// 工作中心Id
        /// </summary>
        public string? WorkCenterId { get; set; }

        /// <summary>
        /// 工单Id
        /// </summary>
        public string? OrderId { get; set; }

        /// <summary>
        /// 产品Id
        /// </summary>
        public string? ProductId { get; set; }

        /// <summary>
        /// 工序Id
        /// </summary>
        public string? ProcedureId { get; set; }

        /// <summary>
        /// 产品条码
        /// </summary>
        public string? SFC { get; set; }

        /// <summary>
        /// 降级等级
        /// </summary>
        public string? DowngradingCode { get; set; }

        /// <summary>
        /// 录入时间
        /// </summary>
        public DateTime[]? CreatedOn { get; set; }

    }

    public class ManuDowngradingDetailExportResultDto
    {
        public string Path { get; set; }

        public string FileName { get; set; }
    }

    /// <summary>
    /// 降级品明细报表Dto
    /// </summary>
    public record ManuDowngradingDetailExportDto : BaseExcelDto
    {
        /// <summary>
        /// 工作中心Code
        /// </summary>
        [EpplusTableColumn(Header = "工作中心", Order = 1)]
        public string? WorkCenterCode { get; set; }

        /// <summary>
        /// 产品条码
        /// </summary>
        [EpplusTableColumn(Header = "产品条码", Order = 2)]
        public string SFC { get; set; }

        /// <summary>
        /// 产品编码
        /// </summary>
        [EpplusTableColumn(Header = "产品编码", Order = 3)]
        public string ProductCode { get; set; }

        /// <summary>
        /// 产品名称
        /// </summary>
        [EpplusTableColumn(Header = "产品名称", Order = 4)]
        public string ProductName { get; set; }

        /// <summary>
        /// 降级编码
        /// </summary>
        [EpplusTableColumn(Header = "降级编码", Order = 5)]
        public string? DowngradingCode { get; set; }

        /// <summary>
        /// 降级名称
        /// </summary>
        [EpplusTableColumn(Header = "降级名称", Order = 6)]
        public string? DowngradingName { get; set; }

        /// <summary>
        /// 降级描述
        /// </summary>
        [EpplusTableColumn(Header = "降级描述", Order = 7)]
        public string? DowngradingRemark { get; set; }

        /// <summary>
        /// 录入人员
        /// </summary>
        [EpplusTableColumn(Header = "录入人员", Order = 8)]
        public string? UpdatedBy { get; set; }

        /// <summary>
        /// 录入时间
        /// </summary>
        [EpplusTableColumn(Header = "录入时间", Order = 9)]
        public DateTime? UpdatedOn { get; set; }

        /// <summary>
        /// 工单号
        /// </summary>
        [EpplusTableColumn(Header = "工单号", Order = 10)]
        public string OrderCode { get; set; }

        /// <summary>
        /// 工单类型PlanWorkOrderTypeEnum
        /// </summary>
        [EpplusTableColumn(Header = "工单类型", Order = 11)]
        public string? OrderType { get; set; }

        /// <summary>
        /// 工序编码
        /// </summary>
        [EpplusTableColumn(Header = "工序编码", Order = 12)]
        public string? ProcedureCode { get; set; }

        /// <summary>
        /// 工序名称
        /// </summary>
        [EpplusTableColumn(Header = "工序名称", Order = 13)]
        public string? ProcedureName { get; set; }


    }

}
