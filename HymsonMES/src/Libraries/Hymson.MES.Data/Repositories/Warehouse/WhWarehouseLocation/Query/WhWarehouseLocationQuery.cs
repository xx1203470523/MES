using Hymson.MES.Core.Enums;

namespace Hymson.MES.Data.Repositories.WhWarehouseLocation.Query
{
    /// <summary>
    /// 库位 查询参数
    /// </summary>
    public class WhWarehouseLocationQuery
    {
        /// <summary>
        /// Id
        /// </summary>
        public long? Id { get; set; }

        /// <summary>
        /// 库位编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 货架Id
        /// </summary>
        public long? WarehouseShelfId { get; set; }

        /// <summary>
        /// 站点Id
        /// </summary>
        public long? SiteId { get; set; }

        /// <summary>
        /// 货架Ids
        /// </summary>
        public IEnumerable<long>? WarehouseShelfIds { get; set; }

        /// <summary>
        /// 库位编码模糊查询
        /// </summary>
        public string? CodeLike { get; set; }

        /// <summary>
        /// 状态;1、启用  2、未启用
        /// </summary>
        public DisableOrEnableEnum? Status { get; set; }
    }
}
