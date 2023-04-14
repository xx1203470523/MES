using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Manufacture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Services.Bos.Manufacture
{
    /// <summary>
    /// 在制品锁业务
    /// </summary>
    public class SfcProduceLockBo
    {
        /// <summary>
        /// 锁1：即时锁；2：将来锁；
        /// </summary>
        public QualityLockEnum Lock { get; set; }

        /// <summary>
        /// 未来锁工序id
        /// </summary>
        public long? LockProductionId { get; set; }
    }
}
