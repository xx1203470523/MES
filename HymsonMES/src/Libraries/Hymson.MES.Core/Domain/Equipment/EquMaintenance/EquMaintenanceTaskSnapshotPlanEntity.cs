using Hymson.Infrastructure;

namespace Hymson.MES.Core.Domain.Equipment
{
    /// <summary>
    /// 数据实体（设备保养快照任务计划）   
    /// equ_maintenance_task_snapshot_plan
    /// @author JAM
    /// @date 2024-05-23 03:21:55
    /// </summary>
    public class EquMaintenanceTaskSnapshotPlanEntity : BaseEntity
    {
        /// <summary>
        /// 任务ID;equ_maintenance_task表的Id
        /// </summary>
        public long MaintenanceTaskId { get; set; }

        /// <summary>
        /// 计划ID;equ_maintenance_plan表的Id
        /// </summary>
        public long MaintenancePlanId { get; set; }

        /// <summary>
        /// 计划编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 计划名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 版本
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// 设备ID;equ_equipment表的Id
        /// </summary>
        public long EquipmentId { get; set; }

        /// <summary>
        /// 保养模板ID;equ_maintenance_template表的Id
        /// </summary>
        public long maintenanceTemplateId { get; set; }

        /// <summary>
        /// 执行人
        /// </summary>
        public string ExecutorIds { get; set; }

        /// <summary>
        /// 负责人
        /// </summary>
        public string LeaderIds { get; set; }

        /// <summary>
        /// 类型;周/日
        /// </summary>
        public bool Type { get; set; }

        /// <summary>
        /// 计划状态
        /// </summary>
        public bool Status { get; set; }

        /// <summary>
        /// 开始时间（计划）
        /// </summary>
        public DateTime? BeginTime { get; set; }

        /// <summary>
        /// 结束时间（计划）
        /// </summary>
        public DateTime? EndTime { get; set; }

        /// <summary>
        /// 是否跳过节假日
        /// </summary>
        public bool? IsSkipHoliday { get; set; }

        /// <summary>
        /// 首次执行时间;首次执行时间需在开始-结束时间范围内
        /// </summary>
        public DateTime? FirstExecuteTime { get; set; }

        /// <summary>
        /// 周期;月、天、小时、分钟
        /// </summary>
        public bool Cycle { get; set; }

        /// <summary>
        /// 完成时间（小时）
        /// </summary>
        public int? CompletionHour { get; set; }

        /// <summary>
        /// 完成时间（分钟）;不超过60
        /// </summary>
        public int? CompletionMinute { get; set; }

        /// <summary>
        /// 提前生成时间（分钟）
        /// </summary>
        public int? PreGeneratedMinute { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

        
    }
}
