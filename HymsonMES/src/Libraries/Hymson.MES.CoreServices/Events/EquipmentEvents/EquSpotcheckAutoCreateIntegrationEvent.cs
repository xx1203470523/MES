using Hymson.EventBus.Abstractions;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Quality;
using Hymson.MES.CoreServices.Bos.Quality;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.CoreServices.Events.Equipment
{
    /// <summary>
    /// 开始任务参数
    /// </summary>
    public record EquSpotcheckAutoCreateIntegrationEvent : IntegrationEvent
    {
        /// <summary>
        /// 计划ID
        /// </summary>
        public long SpotCheckPlanId { get; set; }

        /// <summary>
        /// 站点 
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; } = "";

        /// <summary>
        /// 计划类型 手动 1 自动 0（自动不用传默认0）
        /// </summary>
        public int? ExecType { get; set; } = 0;

        /// <summary>
        /// 表达式
        /// </summary>
        public string CornExpression { get; set; }

        /// <summary>
        /// 首次执行时间
        /// </summary>
        public DateTime FirstExecuteTime { get; set; }

        /// <summary>
        /// 任务结束时间
        /// </summary>
        public DateTime EndTime { get; set; }
    }

    /// <summary>
    /// 停止任务参数
    /// </summary>
    public record EquSpotcheckAutoStopIntegrationEvent : IntegrationEvent
    {
        /// <summary>
        /// 计划ID 
        /// </summary>
        public long SpotCheckPlanId { get; set; }
    }
}
