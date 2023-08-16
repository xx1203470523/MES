using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Core.Enums.Manufacture
{
    /// <summary>
    /// ManuSfcProduceBusiness 业务类型;1、返修业务  2、锁业务
    /// </summary>
    public enum SfcProduceBusinessEnum : sbyte
    {
        /// <summary>
        /// 返修业务
        /// </summary>
        [Description("返修业务")]
        Repair = 1,

        /// <summary>
        /// 锁业务
        /// </summary>
        [Description("锁业务")]
        Lock = 2
    }
}
