using System.ComponentModel;

namespace Hymson.MES.Core.Enums
{
    /// <summary>
    /// 库存来源类型
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
        Locked = 3
    }
}
