using Hymson.Infrastructure;

namespace Hymson.MES.Data.Repositories.Warehouse.Query
{
    /// <summary>
    /// 出货单物料详情（外部数据） 分页参数
    /// </summary>
    public class WhShipmentMaterialPagedQuery : PagerInfo
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

    }
}
