using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Core.Enums.Manufacture
{
    /// <summary>
    /// 条码类型
    /// </summary>
    public  enum SfcTypeEnum : sbyte
    {
        /// <summary>
        /// 生产
        /// </summary>
        [Description("生产")]
        Produce = 1,

        /// <summary>
        /// 非生产
        /// </summary>
        [Description("非生产")]
        NoProduce = 2
    }
}
