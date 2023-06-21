using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Services.Dtos.Report
{
    /// <summary>
    /// 工单产量报表
    /// </summary>
    public record PlanWorkOrderProductionReportViewDto : BaseEntityDto
    {
        /// <summary>
        /// 工单号
        /// </summary>
        public string OrderCode { get; set; }
        /// <summary>
        /// 工单类型
        /// </summary>
        public PlanWorkOrderTypeEnum Type { get; set; }
        /// <summary>
        /// 工单数量
        /// </summary>
        public decimal Qty { get; set; }
        /// <summary>
        /// 工作中心名称
        /// </summary>
        public string WorkCentName { get; set; }
        /// <summary>
        /// 工作中心编码
        /// </summary>
        public string WorkCentCode { get; set; }
        /// <summary>
        /// 物料编码
        /// </summary>
        public string MaterialCode { get; set; }
        /// <summary>
        /// 物料名称
        /// </summary>
        public string MaterialName { get; set; }
        /// <summary>
        /// 计划开始时间
        /// </summary>
        public DateTime? PlanStartTime { get; set; }
        /// <summary>
        /// 计划结束时间
        /// </summary>
        public DateTime? PlanEndTime { get; set; }
        /// <summary>
        /// 实际开始时间
        /// </summary>
        public DateTime? RealStart { get; set; }
        /// <summary>
        /// 实际开始时间
        /// </summary>
        public DateTime? RealEnd { get; set; }
        /// <summary>
        /// 投入数量
        /// </summary>
        public decimal InputQty { get; set; }
        /// <summary>
        /// 手工录入不合格数量
        /// </summary>
        public decimal UnqualifiedQuantity { get; set; }
        /// <summary>
        /// 完工数量
        /// </summary>
        public decimal FinishProductQuantity { get; set; }
        /// <summary>
        /// 下达数量
        /// </summary>
        public decimal PassDownQuantity { get; set; }
        /// <summary>
        /// NG数量
        /// </summary>
        public decimal NGQty { get; set; }
        /// <summary>
        /// 合格率
        /// </summary>
        public decimal PassRate { get; set; }
        /// <summary>
        /// 合格数量
        /// </summary>
        public decimal PassQty { get; set; }
        /// <summary>
        /// 不合格数量 NgQty + UnqualifiedQuantity
        /// </summary>
        public decimal NoPassQty { get; set; }
    }

    /// <summary>
    /// 工单产量报表 分页参数
    /// </summary>
    public class PlanWorkOrderProductionReportPagedQueryDto : PagerInfo
    {
        /// <summary>
        /// 工单类型
        /// </summary>
        public PlanWorkOrderTypeEnum? OrderType { get; set; }

        /// <summary>
        /// 站点id
        /// </summary>
        public long? SiteId { get; set; }

        /// <summary>
        /// 物料编码
        /// </summary>
        public string? MaterialCode { get; set; }

        /// <summary>
        /// 工单编码
        /// </summary>
        public string? OrderCode { get; set; }

        /// <summary>
        /// 产线编码
        /// </summary>
        public string? WorkCenterCode { get; set; }

        /// <summary>
        /// 实际完成时间
        /// </summary>
        public DateTime[]? RealEnd { get; set; }
    }

}
