using System.ComponentModel;

namespace Hymson.MES.Core.Enums.Manufacture
{
    /// <summary>
    /// 业务类型
    /// </summary>
    public  enum ManuSfcProduceBusinessType
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
