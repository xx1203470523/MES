using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;

namespace Hymson.MES.Data.Repositories.WhWarehouseRegion.Query
{
    /// <summary>
    /// 库区 分页参数
    /// </summary>
    public class WhWarehouseRegionPagedQuery : PagerInfo
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 库区编码
        /// </summary>
        public string? Code { get; set; }

        /// <summary>
        /// 库区名称
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// 仓库Ids
        /// </summary>
        public IEnumerable<long>? WareHouseIds { get; set; }

        /// <summary>
        /// 状态;1、启用  2未启用
        /// </summary>
        public DisableOrEnableEnum? Status { get; set; }
    }
}
