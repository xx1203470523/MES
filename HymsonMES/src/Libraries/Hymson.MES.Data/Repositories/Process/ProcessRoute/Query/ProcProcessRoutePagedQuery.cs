using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;

namespace Hymson.MES.Data.Repositories.Process
{
    /// <summary>
    /// 工艺路线表 分页参数
    /// </summary>
    public class ProcProcessRoutePagedQuery : PagerInfo
    {
        /// <summary>
        /// 工艺路线代码
        /// </summary>
        public string? Code { get; set; }

        /// <summary>
        /// 工艺路线名称
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public SysDataStatusEnum? Status { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        public int? Type { get; set; } 

        /// <summary>
        /// 版本
        /// </summary>
        public string? Version { get; set; }

        /// <summary>
        /// 物料名称
        /// </summary>
        public int IsCurrentVersion { get; set; }
        /// <summary>
        /// 站点id
        /// </summary>
        public long SiteId { get; set; } = 0;
    }
}
