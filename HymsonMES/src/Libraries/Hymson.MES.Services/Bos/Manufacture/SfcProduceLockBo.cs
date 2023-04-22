using Hymson.MES.Core.Enums;

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
