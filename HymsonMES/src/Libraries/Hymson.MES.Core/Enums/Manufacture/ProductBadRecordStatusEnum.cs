using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Core.Enums.Manufacture
{
    /// <summary>
    ///1.3.21 manu_sfc_repair_detail 维修详情表 IsClose
    /// </summary>
    public enum ManuSfcRepairDetailIsCloseEnum : sbyte
    {
        /// <summary>
        /// 开启
        /// </summary>
        [Description("开启")]
        Open = 1,
        /// <summary>
        /// 关闭
        /// </summary>
        [Description("关闭")]
        Close = 2,
    }
}
