using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Core.Enums.Mavel
{
    /// <summary>
    /// NIO推送参数中的分类
    /// </summary>
    public enum Category01Enum
    {
        /// <summary>
        /// 未维护
        /// </summary>
        [Description("未维护")]
        No = 0,
        /// <summary>
        /// 工艺参数
        /// </summary>
        [Description("工艺参数")]
        Process = 1,
        /// <summary>
        /// 测试项
        /// </summary>
        [Description("测试项")]
        TestItem = 2
    }
}
