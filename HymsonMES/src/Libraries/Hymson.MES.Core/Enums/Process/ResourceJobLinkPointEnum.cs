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
        [Description("开始")]
        Start = 1,
        /// <summary>
        /// 停止
        /// </summary>
        [Description("停止")]
        Stop = 2,
        /// <summary>
        /// 完成
        /// </summary>
        [Description("完成")]
        Finish = 3
    }
}
