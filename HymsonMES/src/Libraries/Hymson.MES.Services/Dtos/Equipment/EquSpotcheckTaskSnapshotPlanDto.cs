using Hymson.Infrastructure;

namespace Hymson.MES.Services.Dtos.Equipment
{
    /// <summary>
    /// 设备点检快照任务计划新增/更新Dto
    /// </summary>
    public record EquSpotcheckTaskSnapshotPlanSaveDto : BaseEntityDto
    {
        /// <summary>
        /// 
        /// </summary>
        public long Id { get; set; }

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
        public bool Type { get; set; }

       /// <summary>
        /// 点检计划状态
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
        /// 创建人
        /// </summary>
        public string CreatedBy { get; set; }

       /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedOn { get; set; }

       /// <summary>
        /// 最后修改人
        /// </summary>
        public string UpdatedBy { get; set; }

       /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? UpdatedOn { get; set; }

       /// <summary>
        /// 是否逻辑删除
        /// </summary>
        public long IsDeleted { get; set; }

       /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

       
    }

    /// <summary>
    /// 设备点检快照任务计划Dto
    /// </summary>
    public record EquSpotcheckTaskSnapshotPlanDto : BaseEntityDto
    {
        /// <summary>
        /// 
        /// </summary>
        public long Id { get; set; }

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
        public string PlanCode { get; set; }

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
        public bool PlanType { get; set; }

       /// <summary>
        /// 点检计划状态
        /// </summary>
        public bool PlanStatus { get; set; }

       /// <summary>
        /// 开始时间（计划）
        /// </summary>
        public DateTime? PlanBeginTime { get; set; }

       /// <summary>
        /// 结束时间（计划）
        /// </summary>
        public DateTime? PlanEndTime { get; set; }

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
        /// 创建人
        /// </summary>
        public string CreatedBy { get; set; }

       /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedOn { get; set; }

       /// <summary>
        /// 最后修改人
        /// </summary>
        public string UpdatedBy { get; set; }

       /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? UpdatedOn { get; set; }

       /// <summary>
        /// 是否逻辑删除
        /// </summary>
        public long IsDeleted { get; set; }

       /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

       
    }

    /// <summary>
    /// 设备点检快照任务计划分页Dto
    /// </summary>
    public class EquSpotcheckTaskSnapshotPlanPagedQueryDto : PagerInfo { }

}
