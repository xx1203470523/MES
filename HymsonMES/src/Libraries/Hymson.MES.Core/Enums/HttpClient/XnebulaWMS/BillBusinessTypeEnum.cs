﻿using System.ComponentModel;

/// <summary>
/// 单据业务类型
/// </summary>
public enum BillBusinessTypeEnum : short
{
    /// <summary>
    /// 到货单
    /// </summary>
    [Description("到货单")]
    ArrivalNotice = 101,

    /// <summary>
    /// 销售退货单
    /// </summary>
    [Description("销售退货单")]
    SalesReturnReceipt = 102,

    /// <summary>
    /// 其他入库单
    /// </summary>
    [Description("其他入库单")]
    OtherReceipt = 103,

    /// <summary>
    /// 退料单
    /// </summary>
    [Description("退料单")]
    MaterialReturnForm = 104,

    /// <summary>
    /// 成品入库单
    /// </summary>
    [Description("成品入库单")]
    InboundOrder = 105,

    /// <summary>
    /// 采购退货单
    /// </summary>
    [Description("采购退货单")]
    PurchaseReturnReceipt = 301,

    /// <summary>
    /// 采购退货单
    /// </summary>
    [Description("销售订单")]
    SalesOrder = 302,

    /// <summary>
    /// 其他出库单
    /// </summary>
    [Description("其他出库单")]
    OtherDispatch = 303,

    /// <summary>
    /// 材料申请单
    /// </summary>
    [Description("材料申请单")]
    MaterialRequestForm = 304,

    /// <summary>
    /// 物料盘点单
    /// </summary>
    [Description("物料盘点单")]
    MaterialStocktakeForm = 501
}