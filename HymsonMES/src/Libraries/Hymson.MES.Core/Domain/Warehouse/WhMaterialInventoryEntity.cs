/*
 *creator: Karl
 *
 *describe: 物料库存    实体类 | 代码由框架生成  如果数据库字段发生变化,则手动调整
 *builder:  pengxin
 *build datetime: 2023-03-06 03:27:59
 */
using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Warehouse;

namespace Hymson.MES.Core.Domain.Warehouse
{
    /// <summary>
    /// 物料库存，数据实体对象   
    /// wh_material_inventory
    /// @author pengxin
    /// @date 2023-03-06 03:27:59
    /// </summary>
    public class WhMaterialInventoryEntity : BaseEntity
    {
        /// <summary>
        /// 供应商ID
        /// </summary>
        public long SupplierId { get; set; }

        /// <summary>
        /// 物料ID
        /// </summary>
        public long MaterialId { get; set; }

        /// <summary>
        /// 物料条码
        /// </summary>
        public string MaterialBarCode { get; set; }

        /// <summary>
        /// 批次
        /// </summary>
        public int? Batch { get; set; }

        /// <summary>
        /// 数量（剩余 不包含报废数量）
        /// </summary>
        public decimal QuantityResidue { get; set; }

        /// <summary>
        /// 报废数量
        /// </summary>
        public decimal? ScrapQty { get; set; }

        /// <summary>
        /// 接收数量 (一开始的数量)
        /// </summary>
        public decimal ReceivedQty { get; set; }

        /// <summary>
        /// 状态;待使用/使用中/锁定
        /// </summary>
        public WhMaterialInventoryStatusEnum Status { get; set; }

        /// <summary>
        /// 有效期/到期日
        /// </summary>
        public DateTime? DueDate { get; set; }

        /// <summary>
        /// 来源/目标;手动录入/WMS/上料点编号
        /// </summary>
        public MaterialInventorySourceEnum Source { get; set; }

        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 物料类型
        /// </summary>
        public MaterialInventoryMaterialTypeEnum MaterialType { get; set; }

        /// <summary>
        /// 工单id
        /// </summary>
        public long? WorkOrderId { get; set; }

    }
}
