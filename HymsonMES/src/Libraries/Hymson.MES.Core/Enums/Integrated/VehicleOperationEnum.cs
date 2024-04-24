using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Core.Enums.Integrated
{
    public enum VehicleOperationEnum
    {
        /// <summary>
        /// 绑盘
        /// </summary>
        [Description("绑盘")]
        Bind = 0,
        /// <summary>
        /// 解盘
        /// </summary>
        [Description("解盘")]
        Unbind = 1,
        /// <summary>
        /// 清盘
        /// </summary>
        [Description("清盘")]
        Clear = 2,
        /// <summary>
        /// 报废解盘
        /// </summary>
        [Description("报废解盘")]
        NgUnbind = 3
    }
}
