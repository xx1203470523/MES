﻿using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Manufacture;
using OfficeOpenXml.Attributes;

namespace Hymson.MES.Services.Dtos.Report
{
    /// <summary>
    /// 
    /// </summary>
    public record WorkOrderControlReportViewDto : BaseEntityDto
    {
        /// <summary>
        /// 工单ID
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 物料编码/版本
        /// </summary>
        public string MaterialCode { get; set; }

        /// <summary>
        /// 工单Id
        /// </summary>
        public long WorkOrderId { get; set; }

        /// <summary>
        /// 工单编码
        /// </summary>
        public string OrderCode { get; set; }

        /// <summary>
        /// 工作中心
        /// </summary>
        public string WorkCenterId { get; set; }

        /// <summary>
        /// 工单类型
        /// </summary>
        public PlanWorkOrderTypeEnum? Type { get; set; }

        /// <summary>
        /// 工单类型
        /// </summary>
        public PlanWorkOrderStatusEnum? Status { get; set; }

        /// <summary>
        /// 条码下达数量
        /// </summary>
        public decimal PassDownQuantity { get; set; }

        /// <summary>
        /// 在制数量
        /// </summary>
        public decimal ProcessDownQuantity { get; set; }

        /// <summary>
        /// 报废数量
        /// </summary>
        public decimal UnQualifiedQuantity { get; set; }

        /// <summary>
        /// 完成数量
        /// </summary>
        public decimal FinishProductQuantity { get; set; }

        /// <summary>
        /// 工单数量
        /// </summary>
        public decimal Qty { get; set; }
    }

    public record WorkOrderControlReportViewExportDto : BaseExcelDto
    {

        /// <summary>
        /// 物料编码/版本
        /// </summary>
        [EpplusTableColumn(Header = "物料编码", Order = 1)]
        public string MaterialCode { get; set; }

      
        /// <summary>
        /// 工单编码
        /// </summary>
        [EpplusTableColumn(Header = "工单号", Order = 2)]
        public string OrderCode { get; set; }

        /// <summary>
        /// 工作中心
        /// </summary>
        [EpplusTableColumn(Header = "工作中心", Order = 3)]
        public string WorkCenterId { get; set; }

        /// <summary>
        /// 工单类型
        /// </summary>
        [EpplusTableColumn(Header = "工单类型", Order = 4)]
        public string Type { get; set; }
        //public PlanWorkOrderTypeEnum? Type { get; set; }

        /// <summary>
        /// 工单类型
        /// </summary>
        [EpplusTableColumn(Header = "工单状态", Order = 5)]
        public string? Status { get; set; }
        //public PlanWorkOrderStatusEnum? Status { get; set; }

        /// <summary>
        /// 条码下达数量
        /// </summary>
        [EpplusTableColumn(Header = "条码下达数量", Order = 6)]
        public decimal PassDownQuantity { get; set; }

        /// <summary>
        /// 在制数量
        /// </summary>
        [EpplusTableColumn(Header = "排队/在制数量", Order = 7)]
        public decimal ProcessDownQuantity { get; set; }

        /// <summary>
        /// 报废数量
        /// </summary>
        [EpplusTableColumn(Header = "报废数量", Order = 7)]
        public decimal UnQualifiedQuantity { get; set; }

        /// <summary>
        /// 完成数量
        /// </summary>
        [EpplusTableColumn(Header = "完成数量", Order = 8)]
        public decimal FinishProductQuantity { get; set; }

        /// <summary>
        /// 工单数量
        /// </summary>
        [EpplusTableColumn(Header = "工单数量", Order = 9)]
        public decimal Qty { get; set; }


    }

        /// <summary>
        /// 工单报告 分页参数
        /// </summary>
        public class WorkOrderControlReportPagedQueryDto : PagerInfo
    {
        /// <summary>
        /// 站点id
        /// </summary>
        public long? SiteId { get; set; }

        /// <summary>
        /// 物料编码/版本
        /// </summary>
        public string MaterialCode { get; set; }

        /// <summary>
        /// 工单编码
        /// </summary>
        public string OrderCode { get; set; }

        /// <summary>
        /// 工作中心
        /// </summary>
        public string WorkCenterId { get; set; }

        /// <summary>
        /// 工单类型
        /// </summary>
        public PlanWorkOrderTypeEnum? Type { get; set; }

        /// <summary>
        /// 工单类型
        /// </summary>
        public PlanWorkOrderStatusEnum? Status { get; set; }

        /// <summary>
        /// 条码下达数量
        /// </summary>
        public decimal PassDownQuantity { get; set; }

        /// <summary>
        /// 在制数量
        /// </summary>
        public decimal ProcessDownQuantity { get; set; }

        /// <summary>
        /// 报废数量
        /// </summary>
        public decimal UnQualifiedQuantity { get; set; }

        /// <summary>
        /// 完成数量
        /// </summary>
        public decimal FinishProductQuantity { get; set; }

        /// <summary>
        /// 工单数量
        /// </summary>
        public decimal Qty { get; set; }
    }

    /// <summary>
    /// 工单报告 分页参数
    /// </summary>
    public class WorkOrderControlReportOptimizePagedQueryDto : PagerInfo
    {
        /// <summary>
        /// 物料编码
        /// </summary>
        public string? MaterialCode { get; set; }

        /// <summary>
        /// 工单编码
        /// </summary>
        public string? OrderCode { get; set; }

        /// <summary>
        /// 工单类型
        /// </summary>
        public PlanWorkOrderTypeEnum? Type { get; set; }

        /// <summary>
        /// 工单状态
        /// </summary>
        public PlanWorkOrderStatusEnum? Status { get; set; }

        /// <summary>
        /// 创建时间  数组 ：时间范围 
        /// </summary>
        public DateTime[]? CreatedOn { get; set; }
    }
}
