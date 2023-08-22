using Hymson.Infrastructure;

namespace Hymson.MES.Data.Repositories.Integrated.Query
{
    /// <summary>
    /// 消息管理 分页参数
    /// </summary>
    public class InteMessageManagePagedQuery : PagerInfo
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 消息单号
        /// </summary>
        public string? Code { get; set; }

        /// <summary>
        /// 事件类型名称
        /// </summary>
        public string? EventTypeName { get; set; }

        /// <summary>
        /// 车间id
        /// </summary>
        public long? WorkShopId { get; set; }

        /// <summary>
        /// 线体id
        /// </summary>
        public long? LineId { get; set; }

        /// <summary>
        /// 资源名称
        /// </summary>
        public string? ResourceName { get; set; }

        /// <summary>
        /// 更新时间  时间范围  数组
        /// </summary>
        public DateTime[]? UpdatedOn { get; set; }

    }
}
