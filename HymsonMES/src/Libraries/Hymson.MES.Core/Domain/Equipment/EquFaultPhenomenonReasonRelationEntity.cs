using Hymson.Infrastructure;

namespace Hymson.MES.Core.Domain.Equipment
{
    /// <summary>
    /// 数据实体（设备故障原因现象关系）   
    /// equ_fault_phenomenon_reason_relation
    /// @author Czhipu
    /// @date 2023-12-19 07:11:14
    /// </summary>
    public class EquFaultPhenomenonReasonRelationEntity : BaseEntity
    {
        /// <summary>
        /// 设备故障现象id
        /// </summary>
        public long FaultPhenomenonId { get; set; }

        /// <summary>
        /// 设备故障原因id
        /// </summary>
        public long FaultReasonId { get; set; }

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
