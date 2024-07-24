using Hymson.Infrastructure;

namespace Hymson.MES.Core.Domain.WHMaterialReceipt
{
    /// <summary>
    /// 数据实体（物料收货表）   
    /// wh_material_receipt
    /// @author Jam
    /// @date 2024-03-04 02:20:06
    /// </summary>
    public class WhMaterialReceiptEntity : BaseEntity
    {
        /// <summary>
        /// 站点ID
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 收货单号
        /// </summary>
        public string ReceiptNum { get; set; }

        /// <summary>
        /// 供应商Id
        /// </summary>
        public long SupplierId { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 同步编码（WMS需要给ERP）
        /// </summary>
        public string? SyncCode { get; set; }

        /// <summary>
        /// 同步Id（WMS需要给ERP）
        /// </summary>
        public long? SyncId { get; set; }

    }
}
