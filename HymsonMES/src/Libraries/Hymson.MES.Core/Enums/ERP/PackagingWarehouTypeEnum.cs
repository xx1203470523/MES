using System.ComponentModel;

namespace Hymson.MES.Core.Enums
{
    /// <summary>
    /// 包装入库类型枚举
    /// </summary>
    public enum PackagingWarehouTypeEnum : sbyte
    {
        /// <summary>
        /// 胶框
        /// </summary>
        [Description("胶框")]
        Vehicle = 1,

        /// <summary>
        /// 包装箱
        /// </summary>
        [Description("包装箱")]
        Packing = 2
    }
}
