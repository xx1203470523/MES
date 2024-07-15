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


        /// <summary>
        /// 库存维护   (仅仅做记录使用)
        /// </summary>
        [Description("库存维护")]


        /// <summary>
        /// 物料拆分
        /// </summary>
        [Description("物料拆分")]
        MaterialBarCodeSplit = 12,

        /// <summary>
        /// 物料合并
        /// </summary>
        [Description("物料合并")]
        MaterialBarCodeMerge = 13,

        /// <summary>
        /// 物料锁定
        /// </summary>
        [Description("物料锁定")]

        /// <summary>
        /// 物料解锁
        /// </summary>
        [Description("物料解锁")]
        MaterialBarCodeUnLock = 16 ,

        /// <summary>
        /// 物料不良录入
        /// </summary>
        [Description("物料不良录入")]
        BadEntry = 17,

        /// <summary>
        /// 不良处置
        /// </summary>
        [Description("不良处置")]
        BadDisposal = 18,

        /// <summary>
        /// 物料拆分新增
        /// </summary>
        [Description("物料拆分新增")]
        SplitAdd = 19,

        /// <summary>
        /// 物料合并新增
        /// </summary>
        [Description("物料合并新增")]
        CombinedAdd = 20,

 // <summary>
        /// 物料报废
        /// </summary>
        [Description("物料报废")]
        MaterialScrapping = 21,

        /// <summary>
        /// 取消报废
        /// </summary>
        [Description("取消报废")]
        CancelScrapping = 22,

        /// <summary>
        /// 离脱
        /// </summary>
        [Description("离脱")]
        Detachment = 23,
        /// <summary>
        /// 物料卸载
        /// </summary>        [Description("物料卸载")]
        MaterialUnloading = 24    }
}
