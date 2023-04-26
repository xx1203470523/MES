using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Core.Enums.Report
{
    public enum OriginalSummaryReportTypeEnum : sbyte
    {
        /// <summary>
        /// 活动
        /// </summary>
        [Description("活动")]
        Activity = 1,
        /// <summary>
        /// 移除
        /// </summary>
        [Description("移除")]
        Remove = 2
    }
}
