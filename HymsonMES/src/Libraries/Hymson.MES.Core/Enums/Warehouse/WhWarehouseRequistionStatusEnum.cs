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
        Approvaling = 1,

        /// <summary>
        /// WMS审批失败
        /// </summary>
        [Description("WMS审批失败")]
        ApprovalingFailed = 2,

        /// <summary>
        /// WMS审批成功
        /// </summary>
        [Description("WMS审批成功")]
        ApprovalingSuccess = 3,

        /// <summary>
        /// 已收料
        /// </summary>
        [Description("已收料")]
        Picked = 4,

        /// <summary>
        /// 取消领料
        /// </summary>
        [Description("取消领料")]
        Cancel = 5

    }
}
