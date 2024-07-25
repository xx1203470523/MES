using System.ComponentModel;

namespace Hymson.MES.Core.Domain.Manufacture
{
    /// <summary>
    /// 领料类型
    /// </summary>
    public enum ManuRequistionTypeEnum : sbyte
    {
        /// <summary>
        /// 实仓领料
        /// </summary>
        [Description("实仓领料")]
        WorkOrderPicking = 1,
        /// <summary>
        /// 虚仓领料
        /// </summary>
        [Description("虚仓领料")]
        WorkOrderReplenishment = 2
    }

    /// <summary>
    /// 
    /// </summary>
    public enum ManuReturnTypeEnum : sbyte
    {
        /// <summary>
        /// 实仓退料
        /// </summary>
        [Description("实仓退料")]
        WorkOrderReturn = 1,
        /// <summary>
        /// 虚仓退料
        /// </summary>
        [Description("虚仓退料")]
        WorkOrderBorrow = 2
    }

    /// <summary>
    /// 
    /// </summary>
    public enum ManuMaterialFormResponseEnum : sbyte
    {
        /// <summary>
        /// 创建成功
        /// </summary>
        [Description("创建成功")]
        Created = 1,
        /// <summary>
        /// 创建失败
        /// </summary>
        [Description("创建失败")]
        Failed = 2,
        /// <summary>
        /// WMS审批成功
        /// </summary>
        [Description("WMS审批成功")]
        ApprovalingSuccess = 3,
        /// <summary>
        /// WMS审批失败
        /// </summary>
        [Description("WMS审批失败")]
        ApprovalingFailed = 4,
        /// <summary>
        /// 取消成功
        /// </summary>
        [Description("取消成功")]
        CancelSuccess = 5,
        /// <summary>
        /// 取消失败
        /// </summary>
        [Description("取消失败")]
        CancelFailed = 6

    }

}
