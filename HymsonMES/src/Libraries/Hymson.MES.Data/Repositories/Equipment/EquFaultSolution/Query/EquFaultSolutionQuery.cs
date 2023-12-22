using Hymson.MES.Data.Repositories.Common.Query;

namespace Hymson.MES.Data.Repositories.Equipment.Query
{
    /// <summary>
    /// 设备故障解决措施 查询参数
    /// </summary>
    public class EquFaultSolutionQuery : EntityBySiteIdQuery
    {
        /// <summary>
        /// 故障原因ID
        /// </summary>
        public long ReasonId { get; set; }
    }
}
