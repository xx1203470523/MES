/*
 *creator: Karl
 *
 *describe: 设备维修记录故障详情    实体类 | 代码由框架生成  如果数据库字段发生变化,则手动调整
 *builder:  pengxin
 *build datetime: 2024-06-12 10:56:30
 */
using Hymson.Infrastructure;

namespace Hymson.MES.Core.Domain.EquRepairOrderFault
{
    /// <summary>
    /// 设备维修记录故障详情，数据实体对象   
    /// equ_repair_order_fault
    /// @author pengxin
    /// @date 2024-06-12 10:56:30
    /// </summary>
    public class EquRepairOrderFaultEntity : BaseEntity
    {
        /// <summary>
        /// 站点ID
        /// </summary>
        public long SiteId { get; set; }

       /// <summary>
        /// 维修单idequ_repair_orderid
        /// </summary>
        public string RepairOrderId { get; set; }

       /// <summary>
        /// 故障id equ_fault_phenomenon id
        /// </summary>
        public long? FaultPhenomenonId { get; set; }

       /// <summary>
        /// 故障现象
        /// </summary>
        public string FaultPhenomenon { get; set; }

       /// <summary>
        /// 故障原因id equ_fault_reasonId
        /// </summary>
        public long? FaultReasonId { get; set; }

       /// <summary>
        /// 故障原因
        /// </summary>
        public string FaultReason { get; set; }

       /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

       
    }
}
