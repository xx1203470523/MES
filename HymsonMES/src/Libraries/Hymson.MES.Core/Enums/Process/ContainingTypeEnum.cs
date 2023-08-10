using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Core.Enums.Process
{
    /// <summary>
    /// 包含类型
    /// </summary>
    public enum ContainingTypeEnum
    {
        /// <summary>
        /// 小于
        /// </summary>
        [Description("<")]
        Lt = 1,

        /// <summary>
        /// 小于等于
        /// </summary>
        [Description("≤")]
        LtOrE = 2,
    }
}
