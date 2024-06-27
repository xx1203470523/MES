using Hymson.Infrastructure;

namespace Hymson.MES.Core.Domain.Manufacture
{
    /// <summary>
    /// 数据实体（生产退料单明细）   
    /// manu_return_order_detail
    /// @author wxk
    /// @date 2024-06-22 02:25:15
    /// </summary>
    public class ManuReturnOrderDetailEntity : BaseEntity
    {
        /// <summary>
        /// 所属退料单Id
        /// </summary>
        public long RequistionOrderId { get; set; }

       /// <summary>
        /// 物料编码
        /// </summary>
        public string MaterialCode { get; set; }

       /// <summary>
        /// 物料版本
        /// </summary>
        public string Version { get; set; }

       /// <summary>
        /// 物料条码
        /// </summary>
        public string MaterialBarCode { get; set; }

       /// <summary>
        /// 物料批次
        /// </summary>
        public string Batch { get; set; }

       /// <summary>
        /// 退料数量
        /// </summary>
        public decimal Qty { get; set; }

       /// <summary>
        /// 仓库id
        /// </summary>
        public long? WarehouseId { get; set; }

       /// <summary>
        /// 供应商编码
        /// </summary>
        public string SupplierCode { get; set; }

       /// <summary>
        /// 物料的有效期（过期时间)
        /// </summary>
        public DateTime ExpirationDate { get; set; }

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