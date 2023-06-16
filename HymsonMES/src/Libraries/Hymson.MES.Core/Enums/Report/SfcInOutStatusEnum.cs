using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Core.Enums
{
    /// <summary>
    /// 包装报告查询类型
    /// </summary>
    public enum SfcInOutStatusEnum : sbyte
    {
        /// <summary>
        /// 完成
        /// </summary>
        [Description("完成")]
        Finished = 1,
        /// <summary>
        /// 活动
        /// </summary>
        [Description("活动")]
        Activity = 2,
    }
}
