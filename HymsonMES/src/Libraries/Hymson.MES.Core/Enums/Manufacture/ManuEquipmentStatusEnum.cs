using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Core.Enums.Manufacture
{
    /// <summary>
    /// manu_equipment_status_time表CurrentStatus
    /// </summary>
    public enum ManuEquipmentStatusEnum : sbyte
    {
        /// <summary>
        /// 运行
        /// </summary>
        [Description("运行")]
        Working = 1,
        /// <summary>
        /// 故障
        /// </summary>
        [Description("故障")]
        Fault= 2,
        /// <summary>
        /// 待机
        /// </summary>
        [Description("待机")]
        Stand = 3
    }
}
