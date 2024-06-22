using System.ComponentModel;

namespace Hymson.MES.Core.Enums
{
    /// <summary>
    /// 领料申请状态0:审批中，1：审批失败，2：审批成功3.已收料4.取消领料
    /// </summary>
    public enum WhWarehouseRequistionStatusEnum : byte
    {
        /// <summary>
        /// 审批中
        /// </summary>
        [Description("审批中")]
        Approvaling = 0,

        /// <summary>
        /// WMS审批失败
        /// </summary>
        [Description("WMS审批失败")]
        ApprovalingFailed = 1,

        /// <summary>
        /// WMS审批成功
        /// </summary>
        [Description("WMS审批成功")]
        ApprovalingSuccess = 2,
        /// <summary>
        /// 已收料
        /// </summary>
        [Description("已收料")]
        Picked = 3,
        /// <summary>
        /// 取消领料
        /// </summary>
        [Description("取消领料")]
        Cancel = 4
    }
    public enum WhWarehouseReturnStatusEnum : byte
    {
        /// <summary>
        /// 审批中
        /// </summary>
        [Description("审批中")]
        Approvaling = 0,

        /// <summary>
        /// WMS审批失败
        /// </summary>
        [Description("WMS审批失败")]
        ApprovalingFailed = 1,

        /// <summary>
        /// WMS审批成功
        /// </summary>
        [Description("WMS审批成功")]
        ApprovalingSuccess = 2,
        /// <summary>
        /// 已收料
        /// </summary>
        [Description("已退料")]
        Picked = 3,
        /// <summary>
        /// 取消领料
        /// </summary>
        [Description("取消退料")]
        Cancel = 4
    }
}
