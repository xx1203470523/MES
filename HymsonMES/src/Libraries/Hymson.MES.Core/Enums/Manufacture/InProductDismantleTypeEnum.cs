using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Core.Enums.Manufacture
{
    /// <summary>
    /// 在制品拆解类型
    /// </summary>
    public enum InProductDismantleTypeEnum:sbyte
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
        Remove = 2,
        /// <summary>
        /// 全部
        /// </summary>
        [Description("全部")]
        Whole = 3
    }

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
