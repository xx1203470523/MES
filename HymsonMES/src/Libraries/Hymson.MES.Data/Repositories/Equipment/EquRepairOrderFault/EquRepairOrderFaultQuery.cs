/*
 *creator: Karl
 *
 *describe: 设备维修记录故障详情 查询类 | 代码由框架生成
 *builder:  pengxin
 *build datetime: 2024-06-12 10:56:30
 */

namespace Hymson.MES.Data.Repositories.EquRepairOrderFault
{
    /// <summary>
    /// 设备维修记录故障详情 查询参数
    /// </summary>
    public class EquRepairOrderFaultQuery
    {
    }

    /// <summary>
    /// 修改故障原因
    /// </summary>
    public class UpdateFaultReasonsQuery
    {
        /// <summary>
        ///Id
        /// </summary>
        public long? Id { get; set; }
        /// <summary>
        /// 故障原因id equ_fault_reasonId
        /// </summary>
        public long? FaultReasonId { get; set; }

        /// <summary>
        /// 故障原因
        /// </summary>
        public string FaultReason { get; set; }

        /// <summary>
        /// 更新人
        /// </summary>
        public string? UpdatedBy { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime? UpdatedOn { get; set; }
    }
}
