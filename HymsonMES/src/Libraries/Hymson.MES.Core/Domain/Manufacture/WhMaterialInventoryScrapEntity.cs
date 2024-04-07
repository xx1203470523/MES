using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Manufacture;

namespace Hymson.MES.Core.Domain.Manufacture
{
    /// <summary>
    /// 物料报废，数据实体对象   
    /// wh_material_inventory_scrap
    /// @author luoxichang
    /// @date 2023-08-14 14:37:05
    /// </summary>
    public class WhMaterialInventoryScrapEntity : BaseEntity
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

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
        public string Batch { get; set; }

        /// <summary>
        /// 台账id
        /// </summary>
        public long MaterialStandingbookId { get; set; }

        /// <summary>
        /// 报废数量
        /// </summary>
        public decimal ScrapQty { get; set; }

        /// <summary>
        /// 是否取消
        /// </summary>
        public TrueOrFalseEnum IsCancellation { get; set; }

        /// <summary>
        /// 物料领料工单id
        /// </summary>
        public long WorkOrderId { get; set; }

        /// <summary>
        /// 报废工序id
        /// </summary>
        public long ProcedureId { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 取消物料报废台账id
        /// </summary>
        public long CancelMaterialStandingbookId { get; set; }

        /// <summary>
        /// 报废类型
        /// </summary>
        public InventoryScrapTypeEnum? ScrapType { get; set; }
    }
}
