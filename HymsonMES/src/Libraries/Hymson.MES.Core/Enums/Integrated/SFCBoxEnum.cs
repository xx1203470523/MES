using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Core.Enums.Integrated
{
    public enum SFCBoxEnum : sbyte
    {
        /// <summary>
        /// 开启
        /// </summary>
        [Description("开启")]
        Start = 1,
        /// <summary>
        /// 恢复
        /// </summary>
        [Description("恢复")]
        Middle = 2,
    }
}
