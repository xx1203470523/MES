using System.ComponentModel;

namespace Hymson.MES.Core.Enums.Warehouse
{
    /// <summary>
    /// 领料单结果
    /// </summary>
    public enum WhMaterialPickingReceiveResultEnum : sbyte
    {
        /// <summary>
        /// 发料中
        /// </summary>
        [Description("发料中")]
        Receiving = 1,

        /// <summary>
        /// 发料完成
        /// </summary>
        [Description("发料完成")]
        Completed = 2,

        /// <summary>
        /// 取消发料
        /// </summary>
        [Description("取消发料")]
        CancelMaterialReceipt = 3
    }
}
