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
    /// 工单信息导出Dto
    /// </summary>
    [SheetDescriptionAttribute("工单信息")]
    public record ProductTracePlanWorkOrderReportExportDto : BaseExcelDto
    {
        /// <summary>
        /// 工单号
        /// </summary>
        [EpplusTableColumn(Header = "工单号", Order = 1)]
        public string OrderCode { get; set; }
        /// <summary>
        /// 工单类型
        /// </summary>
        [EpplusTableColumn(Header = "工单类型", Order = 2)]
        public PlanWorkOrderTypeEnum Type { get; set; }
        /// <summary>
        /// 工单状态;1：未开始；2：下达；3：生产中；4：完成；5：锁定；6：暂停中；
        /// </summary>
        [EpplusTableColumn(Header = "工单状态", Order = 3)]
        public PlanWorkOrderStatusEnum Status { get; set; }
        /// <summary>
        /// 工单数量
        /// </summary>
        [EpplusTableColumn(Header = "工单数量", Order = 4)]

        public decimal Qty { get; set; }
        
        /// <summary>
        /// 物料编码
        /// </summary>
        [EpplusTableColumn(Header = "物料编码", Order = 5)]
        public string MaterialCode { get; set; }
        /// <summary>
        /// 物料名称
        /// </summary>
        [EpplusTableColumn(Header = "物料名称", Order = 6)]
        public string MaterialName { get; set; }
        /// <summary>
        /// 实际开始时间
        /// </summary>
        [EpplusTableColumn(Header = "实际开始时间", Order = 7)]
        public DateTime? RealStart { get; set; }
        /// <summary>
        /// 实际结束时间
        /// </summary>
        [EpplusTableColumn(Header = "实际结束时间", Order = 8)]
        public DateTime? RealEnd { get; set; }
    }
}
