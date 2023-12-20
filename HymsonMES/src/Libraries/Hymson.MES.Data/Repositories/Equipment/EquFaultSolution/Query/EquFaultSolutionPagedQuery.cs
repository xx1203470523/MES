using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;

namespace Hymson.MES.Data.Repositories.Equipment.Query
{
    /// <summary>
    /// 设备故障解决措施 分页参数
    /// </summary>
    public class EquFaultSolutionPagedQuery : PagerInfo
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 编码（设备故障原因）
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 名称（设备故障原因）
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 故障原因状态（字典定义）
        /// </summary>
        public SysDataStatusEnum? Status { get; set; }

    }
}
