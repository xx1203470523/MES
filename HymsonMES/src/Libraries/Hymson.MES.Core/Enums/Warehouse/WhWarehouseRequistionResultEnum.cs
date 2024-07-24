using System.ComponentModel;

namespace Hymson.MES.Core.Enums.Warehouse
{
    /// <summary>
    /// 退料单结果
    /// </summary>
    public enum WhWarehouseRequistionResultEnum : sbyte
    {
        /// <summary>
        /// 退料中
        /// </summary>
        [Description("退料中")]
        Receiving = 1,

        /// <summary>
        /// 退料完成
        /// </summary>
        [Description("退料完成")]
        Completed = 2,

        /// <summary>
        /// 取消收料
        /// </summary>
        [Description("取消收料")]
        CancelMaterialReceipt = 3
    }
}
