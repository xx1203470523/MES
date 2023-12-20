using Hymson.Infrastructure;

namespace Hymson.MES.Core.Domain.Equipment
{
    /// <summary>
    /// 数据实体（设备故障解决措施原因关系）   
    /// equ_fault_reason_solution_relation
    /// @author Czhipu
    /// @date 2023-12-19 07:11:14
    /// </summary>
    public class EquFaultReasonSolutionRelationEntity : BaseEntity
    {
        /// <summary>
        /// 设备故障原因id
        /// </summary>
        public long FaultReasonId { get; set; }

       /// <summary>
        /// 设备故障解决措施id
        /// </summary>
        public long FaultSolutionId { get; set; }

       /// <summary>
        /// 设备故障先
        /// </summary>
        public string Remark { get; set; }

       /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

       
    }
}
