using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Core.Enums
{
    public enum SysDataStatusEnum : short
    {
        /// <summary>
        /// 新建
        /// </summary>
        [Description("新建")]
        Build = 0,
        /// <summary>
        /// 启用
        /// </summary>
        [Description("启用")]
        Enable = 1,
        /// <summary>
        /// 保留
        /// </summary>
        [Description("保留")]
        Retain = 2,
        /// <summary>
        /// 废除
        /// </summary>
        [Description("废除")]
        Abolish = 3,
    }
}
