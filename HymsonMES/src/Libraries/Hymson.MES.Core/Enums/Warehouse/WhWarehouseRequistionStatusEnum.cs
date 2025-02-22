﻿using System.ComponentModel;

namespace Hymson.MES.Core.Enums
{
    /// <summary>
    /// 领料申请状态0:单据申请，1创建成功,2创建失败,3：审批成功4：WMS审批失败5.取消领料6.已收料
    /// </summary>
    public enum WhWarehouseRequistionStatusEnum : byte
    {
        /// <summary>
        /// 单据申请
        /// </summary>
        [Description("单据申请")]
        Approvaling = 0,
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
        /// 取消领料
        /// </summary>
        [Description("取消领料")]
        Cancel = 5,
        /// <summary>
        /// 已收料
        /// </summary>
        [Description("已收料")]
        Picked = 6

    }
    /// <summary>
    /// 退料申请状态 1、申请成功待检验 2、检验中 3、检验完成待入库 4、退料入库中 5、退料入库完成 6、取消退料
    /// </summary>
    public enum WhWarehouseReturnStatusEnum : byte
    {
        /// <summary>
        /// 单据申请
        /// </summary>
        [Description("单据申请")]
        Approvaling = 0,
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
        /// 取消退料
        /// </summary>
        [Description("取消退料")]
        Cancel = 5,
        /// <summary>
        /// 已退料
        /// </summary>
        [Description("已退料")]
        Return = 6
    }

    /// <summary>
    /// 入库申请状态0:单据申请，1创建成功,2创建失败,3：审批成功4：WMS审批失败5.取消退料6.已退料
    /// </summary>
    public enum ProductReceiptStatusEnum : byte
    {
        /// <summary>
        /// 单据申请
        /// </summary>
        [Description("单据申请")]
        Approvaling = 0,
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
        /// 取消入库
        /// </summary>
        [Description("取消入库")]
        Cancel = 5,
        /// <summary>
        /// 已入库
        /// </summary>
        [Description("已入库")]
        Receipt = 6,

        /// <summary>
        /// 取消申请
        /// </summary>
        [Description("取消申请")]
        CancelApply = 7
    }
}
