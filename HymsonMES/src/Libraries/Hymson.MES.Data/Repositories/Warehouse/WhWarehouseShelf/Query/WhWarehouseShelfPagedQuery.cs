using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;

namespace Hymson.MES.Data.Repositories.WhWarehouseShelf.Query
{
    /// <summary>
    /// 货架 分页参数
    /// </summary>
    public class WhWarehouseShelfPagedQuery : PagerInfo
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 货架编码
        /// </summary>
        public string? Code { get; set; }

        /// <summary>
        /// 货架名称
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// 状态;1、启用  2、未启用
        /// </summary>
        public DisableOrEnableEnum? Status { get; set; }

        /// <summary>
        /// 仓库Ids
        /// </summary>
        public IEnumerable<long>? WarehouseIds { get; set; }

        /// <summary>
        /// 库区Ids
        /// </summary>
        public IEnumerable<long>? WarehouseRegionIds { get; set; }

    }
}
