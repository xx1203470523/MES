using System.ComponentModel;

namespace Hymson.MES.Core.Enums
{
    /// <summary>
    /// 库存状态
    /// </summary>
    public enum WhMaterialInventoryStatusEnum : sbyte
    {
        /// <summary>
        /// 待使用
        /// </summary>
        [Description("待使用")]
        ToBeUsed = 1,
        /// <summary>
        /// 使用中
        /// </summary>
        [Description("使用中")]
        InUse = 2,
        /// <summary>
        /// 锁定
        /// </summary>
        [Description("锁定")]
        Locked = 3,
        /// <summary>
        /// 报废
        /// </summary>
        [Description("报废")]
        Scrap = 4,

        /// <summary>
        /// 无效
        /// </summary>
        [Description("无效")]
        Invalid = 5
    }
}
