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
        /// 物料卸载
        /// </summary>
        [Description("物料卸载")]
        MaterialUnloading = 4,

        /// <summary>
        /// 库存维护   (仅仅做记录使用)
        /// </summary>
        [Description("库存维护")]
        InventoryModify = 5,

        /// <summary>
        /// 物料拆分
        /// </summary>
        [Description("物料拆分")]
        MaterialBarCodeSplit = 6,

        /// <summary>
        /// 物料合并
        /// </summary>
        [Description("物料合并")]
        MaterialBarCodeMerge = 7,

        /// <summary>
        /// 物料锁定
        /// </summary>
        [Description("物料锁定")]
        MaterialBarCodeLock = 8,

        /// <summary>
        /// 物料解锁
        /// </summary>
        [Description("物料解锁")]
        MaterialBarCodeUnLock = 9,

        /// <summary>
        /// 物料不良录入
        /// </summary>
        [Description("物料不良录入")]
        BadEntry = 10,

        /// <summary>
        /// 不良处置
        /// </summary>
        [Description("不良处置")]
        BadDisposal = 11,

        /// <summary>
        /// 生产完成
        /// </summary>
        [Description("生产完成")]
        ManuComplete = 12,

        /// <summary>
        /// 步骤控制
        /// </summary>
        [Description("步骤控制")]
        StepControl = 13,

        // <summary>
        /// 物料报废
        /// </summary>
        [Description("物料报废")]
        MaterialScrapping = 14,

        /// <summary>
        /// 取消报废
        /// </summary>
        [Description("取消报废")]
        CancelScrapping = 15,

        /// <summary>
        /// 离脱
        /// </summary>
        [Description("离脱")]
        Detachment = 16
    }
}
