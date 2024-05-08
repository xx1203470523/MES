using System.ComponentModel;

namespace Hymson.MES.Core.Enums.Quality
{
    /// <summary>
    /// 质量锁定对象（条码、物料）
    /// </summary>
    public enum QualityLockObjectEnum : sbyte
    {
        /// <summary>
        /// 产品序列码
        /// </summary>
        [Description("产品序列码")]
        BarCode = 1,

        /// <summary>
        /// 物料
        /// </summary>
        [Description("物料")]
        Material = 2
    }
}
