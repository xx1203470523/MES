using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;

namespace Hymson.MES.Core.Domain.Equipment
{
    /// <summary>
    /// 数据实体（设备点检快照任务计划）   
    /// equ_spotcheck_task_snapshot_plan
    /// @author User
    /// @date 2024-05-20 07:40:53
    /// </summary>
    public class EquSpotcheckTaskSnapshotPlanEntity : BaseEntity
    {
        /// <summary>
        /// 点检任务ID;equ_spotcheck_task表的Id
        /// </summary>
        public long SpotCheckTaskId { get; set; }

        /// <summary>
        /// 点检计划ID;equ_spotcheck_plan表的Id
        /// </summary>
        public long SpotCheckPlanId { get; set; }

        /// <summary>
        /// 点检计划编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 点检计划名称
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
        /// 点检模板ID;equ_spotcheck_template表的Id
        /// </summary>
        public long SpotCheckTemplateId { get; set; }

        /// <summary>
        /// 点检执行人;用户中心UserId集合
        /// </summary>
        public string ExecutorIds { get; set; }

        /// <summary>
        /// 点检负责人;用户中心UserId集合
        /// </summary>
        public string LeaderIds { get; set; }

        /// <summary>
        /// 点检类型;周/日
        /// </summary>
        public EquipmentSpotcheckTypeEnum Type { get; set; }

        /// <summary>
        /// 点检计划状态
        /// </summary>
        public DisableOrEnableEnum Status { get; set; }

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
        public TrueOrFalseEnum? IsSkipHoliday { get; set; }

        /// <summary>
        /// 首次执行时间;首次执行时间需在开始-结束时间范围内
        /// </summary>
        public DateTime? FirstExecuteTime { get; set; }

        /// <summary>
        /// 周期;月、天、小时、分钟
        /// </summary>
        public int Cycle { get; set; }

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
