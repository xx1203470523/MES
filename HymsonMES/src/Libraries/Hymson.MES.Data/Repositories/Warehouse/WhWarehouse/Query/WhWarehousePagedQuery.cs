using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;

namespace Hymson.MES.Data.Repositories.WhWareHouse.Query
{
    /// <summary>
    /// 仓库 分页参数
    /// </summary>
    public class WhWarehousePagedQuery : PagerInfo
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 仓库编码
        /// </summary>
        public string? Code { get; set; }

        /// <summary>
        /// 仓库名称
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public DisableOrEnableEnum? Status { get; set; }
    }
}
