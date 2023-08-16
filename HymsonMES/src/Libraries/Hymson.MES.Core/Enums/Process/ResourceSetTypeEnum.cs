using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Core.Enums.Process
{
    /// <summary>
    /// 资源设置
    /// </summary>
    public enum ResourceSetTypeEnum : sbyte
    {
        /// <summary>
        /// 工单
        /// </summary>
        [Description("工单")]
        Workorder = 1,
        /// <summary>
        /// 物料
        /// </summary>
        [Description("物料")]
        Material = 2,
        /// <summary>
        /// 工具
        /// </summary>
        [Description("工具")]
        Tool = 3
    }
}
