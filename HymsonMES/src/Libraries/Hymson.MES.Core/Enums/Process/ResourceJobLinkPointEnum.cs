using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Core.Enums
{
    /// <summary>
    /// 资源作业设置关联点
    /// </summary>
    public enum ResourceJobLinkPointEnum:sbyte
    {
        /// <summary>
        /// 开始
        /// </summary>
        [Description("开始前")]
        BeforeStart = 1,
        /// <summary>
        /// 开始
        /// </summary>
        [Description("开始后")]
        AfterStart = 2,
        /// <summary>
        /// 完成前
        /// </summary>
        [Description("完成前")]
        BeforeFinish = 3,
        /// <summary>
        /// 完成后
        /// </summary>
        [Description("完成后")]
        AfterFinish = 4
    }
}
