using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Core.Enums
{
    /// <summary>
    /// manu_sfc_info条码状态枚举
    /// </summary>
    public enum SfcStatusEnum : sbyte
    {
        /// <summary>
        /// 在制
        /// </summary>
        [Description("在制")]
        InProcess = 1,
        /// <summary>
        /// 完成
        /// </summary>
        [Description("完成")]
        Complete = 2,
        /// <summary>
        /// 已入库
        /// </summary>
        [Description("已入库")]
        Received = 3,
        /// <summary>
        /// 报废
        /// </summary>
        [Description("报废")]
        Scrapping = 4
    }
}
