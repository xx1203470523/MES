using System.ComponentModel;

namespace Hymson.MES.Core.Enums
{
    /// <summary>
    /// 物料库存来源类型
    /// </summary>
    public enum WhMaterialInventoryTypeEnum : sbyte
    {
        /// <summary>
        /// 物料接收
        /// </summary>
        [Description("物料接收")]
        MaterialReceiving = 1,
        /// <summary>
        /// 物料退料
        /// </summary>
        [Description("物料退料")]
        MaterialReturn = 2,
        /// <summary>
        /// 物料加载
        /// </summary>
        [Description("物料加载")]
        MaterialLoading = 3,

        /// <summary>
        /// 生产完成
        /// </summary>
        [Description("生产完成")]
        ManuComplete = 4,

        /// <summary>
        /// 步骤控制
        /// </summary>
        [Description("步骤控制")]
        StepControl = 5,

        /// <summary>
        /// 库存维护   (仅仅做记录使用)
        /// </summary>
        [Description("库存维护")]
        InventoryModify = 6,
        /// <summary>
        /// 条码拆分
        /// </summary>
        [Description("条码拆分")]
        Split = 7,
        /// <summary>
        /// 条码合并
        /// </summary>
        [Description("条码合并")]
        Merge = 8,
        /// <summary>
        /// 离脱
        /// </summary>
        [Description("离脱")]
        Detachment = 9,
        // <summary>
        /// 物料报废
        /// </summary>
        [Description("物料报废")]
        MaterialScrapping = 10,
        /// <summary>
        /// 取消报废
        /// </summary>
        [Description("取消报废")]
        CancelScrapping = 11
    }
}
