using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Core.Enums.Manufacture
{
     /// <summary>
     /// 步骤控制
     /// </summary>
    public enum SfcStepControlEnum : sbyte
    {
        /// <summary>
        /// 排队中
        /// </summary>
        [Description("排队中")]
        lineUp = 1,

        /// <summary>
        /// 活动中
        /// </summary>
        [Description("活动中")]
        Activity = 2,

        /// <summary>
        /// 完成
        /// </summary>
        [Description("完成")]
        InProductionComplete = 3,

        /// <summary>
        /// 已完成
        /// </summary>
        [Description("已完成")]
        Complete = 4,
    }
}
