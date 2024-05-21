using System.ComponentModel;

namespace Hymson.MES.Core.Enums
{
    /// <summary>
    /// 质量锁定操作状态
    /// </summary>
    public enum QualityLockEnum : sbyte
    {
        /// <summary>
        /// 取消锁定，未锁定
        /// </summary>
        [Description("取消锁定")]
        Unlock = 1,
        /// <summary>
        /// 即时锁
        /// </summary>
        [Description("即时锁")]
        InstantLock = 2,
        /// <summary>
        /// 将来锁
        /// </summary>
        [Description("将来锁")]
        FutureLock = 3,
    }



    /// <summary>
    /// 质量锁定操作状态
    /// </summary>
    public enum QualityLockMaterialEnum : sbyte
    {
        /// <summary>
        /// 取消锁定，未锁定
        /// </summary>
        [Description("取消锁定")]
        Unlock = 1,
        /// <summary>
        /// 即时锁
        /// </summary>
        [Description("即时锁")]
        InstantLock = 2,
    }
}
