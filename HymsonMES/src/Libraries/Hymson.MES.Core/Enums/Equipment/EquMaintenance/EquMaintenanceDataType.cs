using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Core.Enums.Equipment.EquMaintenance
{
    public enum EquMaintenanceDataType : sbyte
    {
        /// <summary>
        /// 文本
        /// </summary>
        [Description("文本")]
        Text = 1,
        /// <summary>
        /// 数值
        /// </summary>
        [Description("数值")]
        Numeric = 2,
    }

}
