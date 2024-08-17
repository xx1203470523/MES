using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;

namespace Hymson.MES.Core.Domain.Manufacture
{
    /// <summary>
    /// 数据实体（废成品入库记录）   
    /// manu_waste_products_receipt_record
    /// @author User
    /// @date 2024-08-14 04:39:04
    /// </summary>
    public class ManuWasteProductsReceiptRecordEntity : BaseEntity
    {
        /// <summary>
        /// 入库工单号
        /// </summary>
        public string WarehouseOrderCode { get; set; }

       /// <summary>
        /// 工单id
        /// </summary>
        public long? WorkOrderId { get; set; }

       /// <summary>
        /// 状态0:审批中，1：审批失败，2：审批成功3.已退料
        /// </summary>
        public ProductReceiptStatusEnum Status { get; set; }

       /// <summary>
        /// 物料描述
        /// </summary>
        public string Remark { get; set; }

       /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

       /// <summary>
        /// 完工入库单
        /// </summary>
        public string CompletionOrderCode { get; set; }

       
    }
}
