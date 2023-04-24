using System.ComponentModel;

namespace Hymson.MES.Core.Enums
{
    /// <summary>
    /// 物料库存来源类型
    /// </summary>
    public enum WhMaterialInventorySourceEnum : sbyte
    {
        /// <summary>
        /// 手动录入
        /// </summary>
        [Description("手动录入")]
        ManualEntry = 1,
        /// <summary>
        /// WMS
        /// </summary>
        [Description("WMS")]
        WMS = 2,
        /// <summary>
        /// 上料点编号
        /// </summary>
        [Description("上料点")]
        LoadingPoint = 3,

        /// <summary>
        /// 生产完成
        /// </summary>
        [Description("生产完成")]
        manuComplete = 4
    }
}
