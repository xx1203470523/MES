using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;

namespace Hymson.MES.Data.Repositories.Process
{
    /// <summary>
    /// 跨工序时间管控 分页参数
    /// </summary>
    public class ProcProcedureTimeControlPagedQuery : PagerInfo
    {
        /// <summary>
        /// 站点
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 管控标识
        /// </summary>
        public string? Code { get; set; }

        /// <summary>
        /// 管控名称
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public SysDataStatusEnum? Status { get; set; }

        /// <summary>
        /// 产品ID
        /// </summary>
        public IEnumerable<long>? ProductIds { get; set; }

        /// <summary>
        /// 产品版本
        /// </summary>
        public string? Version { get; set; }

        /// <summary>
        /// 起始工序
        /// </summary>
        public IEnumerable<long>? FromProcedureIds { get; set; }

        /// <summary>
        /// 到达工序
        /// </summary>
        public IEnumerable<long>? ToProcedureIds { get; set; }

    }
}
