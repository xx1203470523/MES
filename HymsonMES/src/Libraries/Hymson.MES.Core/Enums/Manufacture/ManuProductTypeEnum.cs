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
        Normal = 0,
        /// <summary>
        /// 虚拟
        /// </summary>
        [Description("虚拟")]
        Fictitious = 1,
        /// <summary>
        /// 联产品
        /// </summary>
        [Description("联产品")]
        JointProducts = 2,
        /// <summary>
        /// 副产品
        /// </summary>
        [Description("副产品")]
        ByProduct = 3,
    }
}
