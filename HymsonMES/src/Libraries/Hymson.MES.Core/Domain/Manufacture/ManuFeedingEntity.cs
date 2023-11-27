using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Warehouse;

namespace Hymson.MES.Core.Domain.Manufacture
{
    /// <summary>
    /// 物料加载，数据实体对象   
    /// manu_feeding
    /// @author Czhipu
    /// @date 2023-03-25 09:56:47
    /// </summary>
    public class ManuFeedingEntity : BaseEntity
    {
        /// <summary>
        /// 资源Id
        /// </summary>
        public long ResourceId { get; set; }

        /// <summary>
        /// 上料点
        /// </summary>
        public long? FeedingPointId { get; set; }

        /// <summary>
        /// 上料产品Id（主物料ID）
        /// </summary>
        public long ProductId { get; set; }

        /// <summary>
        /// 供应商ID
        /// </summary>
        public long SupplierId { get; set; }

        /// <summary>
        /// 上料条码
        /// </summary>
        public string BarCode { get; set; }

        /// <summary>
        /// 条码对应的物料ID
        /// </summary>
        public long MaterialId { get; set; }

        /// <summary>
        /// 初始数量
        /// </summary>
        public decimal InitQty { get; set; }

        /// <summary>
        /// 上料数量/卸料数量
        /// </summary>
        public decimal Qty { get; set; }

        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        ///物料类型
        /// </summary>
        public MaterialInventoryMaterialTypeEnum MaterialType { get; set; }

        /// <summary>
        /// 工单id
        /// </summary>
        public long? WorkOrderId { get; set; }

        /// <summary>
        /// 物料加载来源
        /// </summary>
        public ManuSFCFeedingSourceEnum? LoadSource { get; set; }

    }
}
