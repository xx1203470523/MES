using Hymson.Infrastructure;

namespace Hymson.MES.Core.Domain.Manufacture
{
    /// <summary>
    /// 数据实体（废成品入库明细）   
    /// manu_waste_products_receipt_record_detail
    /// @author User
    /// @date 2024-08-14 04:39:28
    /// </summary>
    public class ManuWasteProductsReceiptRecordDetailEntity : BaseEntity
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
        /// 计量单位(字典定义)
        /// </summary>
        public string Unit { get; set; }

       /// <summary>
        /// 入库数量
        /// </summary>
        public decimal Qty { get; set; }

       /// <summary>
        /// 类型
        /// </summary>
        public bool? Type { get; set; }

       /// <summary>
        /// 物料描述
        /// </summary>
        public string Remark { get; set; }

       /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

       
    }
}
