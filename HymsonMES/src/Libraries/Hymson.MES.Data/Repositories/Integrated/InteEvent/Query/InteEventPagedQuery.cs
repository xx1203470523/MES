using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;

namespace Hymson.MES.Data.Repositories.Integrated.Query
{
    /// <summary>
    /// 事件维护 分页参数
    /// </summary>
    public class InteEventPagedQuery : PagerInfo
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 参数集编码
        /// </summary>
        public string? Code { get; set; }

        /// <summary>
        /// 参数集名称
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public DisableOrEnableEnum? Status { get; set; }

        /// <summary>
        /// 事件类型Id
        /// </summary>
        public long? EventTypeId { get; set; }

        /// <summary>
        /// 事件类型名称
        /// </summary>
        public string? EventTypeName { get; set; }

    }
}
