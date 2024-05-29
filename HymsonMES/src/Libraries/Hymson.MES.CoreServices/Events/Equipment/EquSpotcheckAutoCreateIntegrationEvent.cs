using Hymson.EventBus.Abstractions;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Quality;
using Hymson.MES.CoreServices.Bos.Quality;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.CoreServices.Events.Quality
{
    /// <summary>
    ///
    /// </summary>
    public record EquSpotcheckAutoCreateIntegrationEvent : IntegrationEvent
    {
        /// <summary>
        /// 站点 
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; } = "";

        /// <summary>
        /// 计划ID
        /// </summary>
        public long SpotCheckPlanId { get; set; }

        /// <summary>
        /// 计划类型 手动 1 自动 0（自动不用传默认0）
        /// </summary>
        public int? ExecType { get; set; } = 0;
    }
}
