using Hymson.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Services.Dtos.Report
{
    public record ProductionManagePanelReportDto : BaseEntityDto
    {
        /// <summary>
        /// 综合良率
        /// </summary>
        public decimal OverallYieldRate { get; set; }
        /// <summary>
        /// 计划达成率
        /// </summary>
        public decimal OverallPlanAchievingRate { get; set; }
        /// <summary>
        /// 日电芯消耗数量
        /// 计算首工序进站电芯
        /// </summary>
        public int DayConsume { get; set; }
        /// <summary>
        /// 线体名称
        /// </summary>
        public string WorkLineName { get; set; }
        /// <summary>
        /// 完成率
        /// </summary>
        public decimal CompletedRate { get; set;}

        /// <summary>
        /// true白班，false夜班
        /// </summary>
        public bool DayShift { get; set; }
        /// <summary>
        /// 工艺路线编码
        /// </summary>
        public string ProcessRouteCode { get; set; }
        /// <summary>
        /// 工艺路线名称
        /// </summary>
        public string ProcessRouteName { get; set; }
        /// <summary>
        /// 工单下达时间
        /// </summary>
        public DateTime WorkOrderDownTime { get; set; }
        /// <summary>
        /// 工单号
        /// </summary>
        public string WorkOrderCode { get; set; }
        /// <summary>
        /// 工单数量
        /// </summary>
        public int WorkOrderQty { get; set; }

        /// <summary>
        /// 完工数量
        /// </summary>
        public int CompletedQty { get; set; }

        /// <summary>
        /// 不良数量
        /// </summary>
        public int UnqualifiedQty { get; set; }

        /// <summary>
        /// 不良率
        /// </summary>
        public int UnqualifiedRate { get; set; }

        /// <summary>
        /// 产品编码
        /// </summary>
        public string ProductCode { get; set; }
        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProductName{ get; set; }
        /// <summary>
        /// 投入数量
        /// </summary>
        public int InputQty { get; set; }
    }
}
