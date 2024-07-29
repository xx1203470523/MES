using Hymson.MES.Core.Enums.Warehouse;

namespace Hymson.MES.SystemServices.Dtos.Warehouse
{
    /// <summary>
    /// 
    /// </summary>
    public class WhMaterialReturnConfirmDto
    {
        /// <summary>
        /// 退料单号
        /// </summary>
        public string ReturnOrderCode { set; get; } = "";

        /// <summary>
        /// 退料单结果
        /// </summary>
        public WhWarehouseRequistionResultEnum ReceiptResult { set; get; }

        /// <summary>
        /// 退料单仓库收料详情
        /// </summary>
        public IEnumerable<MaterialReturnReceiptDetailDto>? Details { set; get; }

        /// <summary>
        /// 备注
        /// </summary>
        public string? Remark { set; get; }

        /// <summary>
        /// 操作人
        /// </summary>
        public string OperateBy { set; get; } = "";
    }

    /// <summary>
    /// 退料
    /// </summary>
    public class MaterialReturnReceiptDetailDto
    {
        /// <summary>
        /// 物料条码
        /// </summary>
        public string? MaterialBarCode { set; get; } = "";

        /// <summary>
        /// 物料编码
        /// </summary>
        public string MaterialCode { set; get; } = "";

        /// <summary>
        /// 数量
        /// </summary>
        public decimal Qty { set; get; }
    }
}
