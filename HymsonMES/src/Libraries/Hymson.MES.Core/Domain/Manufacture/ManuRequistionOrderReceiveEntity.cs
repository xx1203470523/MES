using Hymson.Infrastructure;

namespace Hymson.MES.Core.Domain.Manufacture
{
    /// <summary>
    /// 数据实体（生产领料单明细）   
    /// manu_requistion_order_receive
    /// @author User
    /// @date 2024-07-25 07:11:44
    /// </summary>
    public class ManuRequistionOrderReceiveEntity : BaseEntity
    {
        /// <summary>
        /// 所属领料单Id
        /// </summary>
        public long RequistionOrderId { get; set; }

        /// <summary>
        /// 物料编码
        /// </summary>
        public long MaterialId { get; set; }

        /// <summary>
        /// 物料条码
        /// </summary>
        public string MaterialBarCode { get; set; }

        /// <summary>
        /// 物料批次
        /// </summary>
        public string Batch { get; set; }

        /// <summary>
        /// 领料数量
        /// </summary>
        public decimal Qty { get; set; }

        /// <summary>
        /// 仓库id
        /// </summary>
        public long? WarehouseId { get; set; }

        /// <summary>
        /// 供应商编码
        /// </summary>
        public long? SupplierId { get; set; }

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
