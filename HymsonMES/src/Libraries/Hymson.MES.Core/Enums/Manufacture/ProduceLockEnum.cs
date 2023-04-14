using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Core.Enums.Manufacture
{
    /// <summary>
    /// 锁类型
    /// </summary>
    public enum ProduceLockEnum
    {
        /// <summary>
        /// 即时锁
        /// </summary>
        [Description("即时锁")]
        InstantLock = 1,
        /// <summary>
        /// 将来锁
        /// </summary>
        [Description("将来锁")]
        FutureLock = 2,
    }
}
