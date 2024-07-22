using Hymson.Infrastructure;

namespace Hymson.MES.Core.Domain.Manufacture
{
    /// <summary>
    /// 数据实体（工单完工入库明细）   
    /// manu_product_receipt_order_detail
    /// @author User
    /// @date 2024-07-17 10:29:18
    /// </summary>
    public class ManuProductReceiptOrderDetailEntity : BaseEntity
    {
        /// <summary>
        /// 入库单主表Id
        /// </summary>
        public long ProductReceiptId { get; set; }

       /// <summary>
        /// 物料编码
        /// </summary>
        public string MaterialCode { get; set; }

       /// <summary>
        /// 物料名称
        /// </summary>
        public string MaterialName { get; set; }

       /// <summary>
        /// 条码
        /// </summary>
        public string Sfc { get; set; }

       /// <summary>
        /// 计量单位(字典定义)
        /// </summary>
        public string Unit { get; set; }

       /// <summary>
        /// 入库数量
        /// </summary>
        public decimal Qty { get; set; }

       /// <summary>
        /// 仓库id
        /// </summary>
        public long? WarehouseId { get; set; }

       /// <summary>
        /// 品检状态
        /// </summary>
        public bool? Status { get; set; }

       /// <summary>
        /// 物料描述
        /// </summary>
        public string Remark { get; set; }

       /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 箱号编码
        /// </summary>
        public string ContaineCode { get; set; }
    }
}
