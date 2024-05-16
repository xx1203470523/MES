using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;

namespace Hymson.MES.Core.Domain.Warehouse
{
    /// <summary>
    /// 物料台账，数据实体对象   
    /// wh_material_standingbook
    /// @author pengxin
    /// @date 2023-03-13 10:03:28
    /// </summary>
    public class WhMaterialStandingbookEntity : BaseEntity
    {
        /// <summary>
        /// 物料编码
        /// </summary>
        public string MaterialCode { get; set; }

        /// <summary>
        /// 物料名称（D）
        /// </summary>
        public string MaterialName { get; set; }

        /// <summary>
        /// 物料版本（D）
        /// </summary>
        public string MaterialVersion { get; set; }

        /// <summary>
        /// 物料条码
        /// </summary>
        public string MaterialBarCode { get; set; }

        /// <summary>
        /// 批次号
        /// </summary>
        public string Batch { get; set; } = "";

        /// <summary>
        /// 数量
        /// </summary>
        public decimal Quantity { get; set; }

        /// <summary>
        /// 单位;来自物料模块的计量单位
        /// </summary>
        public string Unit { get; set; }

        /// <summary>
        /// 流转类型;物料接收/物料退料/物料加载
        /// </summary>
        public WhMaterialInventoryTypeEnum Type { get; set; }

        /// <summary>
        /// 来源/目标;手动录入/WMS/上料点编号
        /// </summary>
        public MaterialInventorySourceEnum Source { get; set; }

        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 供应商ID
        /// </summary>
        public long SupplierId { get; set; }
    }
}
