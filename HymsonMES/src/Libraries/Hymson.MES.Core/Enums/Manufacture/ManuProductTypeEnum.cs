using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Core.Enums.Manufacture
{
    /// <summary>
    /// Bom类型
    /// </summary>
    public enum ManuProductTypeEnum : sbyte
    {
        /// <summary>
        /// 正常
        /// </summary>
        [Description("正常")]
        Normal = 1,
        /// <summary>
        /// 虚拟
        /// </summary>
        [Description("虚拟")]
        Fictitious = 2,
        /// <summary>
        /// 联产品
        /// </summary>
        [Description("联产品")]
        JointProducts = 3,
        /// <summary>
        /// 副产品x
        /// </summary>
        [Description("副产品")]
        ByProduct = 4,
    }
}
