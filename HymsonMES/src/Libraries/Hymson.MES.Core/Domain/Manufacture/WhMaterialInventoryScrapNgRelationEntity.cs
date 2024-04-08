using Hymson.Infrastructure;

namespace Hymson.MES.Core.Domain.Manufacture
{
    /// <summary>
    /// 物料报废ng信息，数据实体对象   
    /// wh_material_inventory_scrap_ng_relation
    /// @author luoxichang
    /// @date 2023-08-14 14:37:05
    /// </summary>
    public class WhMaterialInventoryScrapNgRelationEntity : BaseEntity
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 物料报废id
        /// </summary>
        public long MaterialInventoryScrapId { get; set; }

        /// <summary>
        /// 不合格代码id
        /// </summary>
        public long UnqualifiedCodeId { get; set; }

    }
}
