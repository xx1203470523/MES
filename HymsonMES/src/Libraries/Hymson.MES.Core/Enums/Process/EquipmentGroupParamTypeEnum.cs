using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Core.Enums
{
    /// <summary>
    /// 设备参数类型
    /// </summary>
    public enum EquipmentGroupParamTypeEnum : sbyte
    {
        /// <summary>
        /// 开机参数
        /// </summary>
        [Description("开机参数")]
        OpenParam = 1,

        /// <summary>
        /// 设备过程参数
        /// </summary>
        [Description("设备过程参数")]
        ProcessParam = 2
    }
}
