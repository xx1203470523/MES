using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;

namespace Hymson.MES.Data.Repositories.Equipment.Query
{
    /// <summary>
    /// 工具类型管理 分页参数
    /// </summary>
    public class EquToolsTypePagedQuery : PagerInfo
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 工具类型编码
        /// </summary>
        public string? Code { get; set; }

        /// <summary>
        /// 备件名称
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// 更新时间  时间范围  数组
        /// </summary>
        public DateTime[]? UpdatedOn { get; set; }

        /// <summary>
        /// 状态;0 禁用 1、启用
        /// </summary>
        public DisableOrEnableEnum? Status { get; set; }

    }
}
