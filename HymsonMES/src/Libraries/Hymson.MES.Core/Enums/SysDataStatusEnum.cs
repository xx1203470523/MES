using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Core.Enums
{
    public enum SysDataStatusEnum:short
    {
        /// <summary>
        /// 新建
        /// </summary>
        Build = 0,
        /// <summary>
        /// 启用
        /// </summary>
        Enable = 1,
        /// <summary>
        /// 保留
        /// </summary>
        Retain = 2,
        /// <summary>
        /// 废除
        /// </summary>
        Abolish = 3,
    }
}
