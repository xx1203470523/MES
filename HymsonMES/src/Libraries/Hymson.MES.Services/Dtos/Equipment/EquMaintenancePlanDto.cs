/*
 *creator: Karl
 *
 *describe: 设备点检计划    Dto | 代码由框架生成
 *builder:  pengxin
 *build datetime: 2024-05-17 09:36:24
 */

using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;

namespace Hymson.MES.Services.Dtos.EquMaintenancePlan
{
    /// <summary>
    /// 设备点检计划Dto
    /// </summary>
    public record EquMaintenancePlanDto : BaseEntityDto
    {
        /// <summary>
        /// 
        /// </summary>
        public long Id { get; set; }

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
        /// 点检执行人;用户中心UserId集合
        /// </summary>
        public string? ExecutorIds { get; set; }

        /// <summary>
        /// 点检负责人;用户中心UserId集合
        /// </summary>
        public string? LeaderIds { get; set; }


        /// <summary>
        /// 点检类型;天/小时
        /// </summary>
        public EquipmentMaintenanceTypeEnum? Type { get; set; }

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
        /// 是否跳过节假日;日历中的休息日
        /// </summary>
        public TrueOrFalseEnum? IsSkipHoliday { get; set; }

        /// <summary>
        /// 首次执行时间;首次执行时间需在开始-结束时间范围内
        /// </summary>
        public DateTime? FirstExecuteTime { get; set; }

        /// <summary>
        /// 循环周期
        /// </summary>
        public int? Cycle { get; set; }

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
        /// Corn表达式
        /// </summary>
        public string CornExpression { get; set; }

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
    /// 设备点检计划新增Dto
    /// </summary>
    public record EquMaintenancePlanCreateDto : BaseEntityDto
    {

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
        /// 点检类型;天/小时
        /// </summary>
        public EquipmentMaintenanceTypeEnum Type { get; set; }

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
        /// 是否跳过节假日;日历中的休息日
        /// </summary>
        public TrueOrFalseEnum? IsSkipHoliday { get; set; }

        /// <summary>
        /// 首次执行时间;首次执行时间需在开始-结束时间范围内
        /// </summary>
        public DateTime? FirstExecuteTime { get; set; }

        /// <summary>
        /// 循环周期
        /// </summary>
        public int? Cycle { get; set; }

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
        /// 关联设备信息
        /// </summary>
        public IEnumerable<EquRelatedDto> RelationDto { get; set; }
    }

    /// <summary>
    /// 设备点检计划更新Dto
    /// </summary>
    public record EquMaintenancePlanModifyDto : BaseEntityDto
    {

        /// <summary>
        /// 
        /// </summary>
        public long Id { get; set; }
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
        /// 点检类型;天/小时
        /// </summary>
        public EquipmentMaintenanceTypeEnum Type { get; set; }

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
        /// 是否跳过节假日;日历中的休息日
        /// </summary>
        public TrueOrFalseEnum? IsSkipHoliday { get; set; }

        /// <summary>
        /// 首次执行时间;首次执行时间需在开始-结束时间范围内
        /// </summary>
        public DateTime? FirstExecuteTime { get; set; }

        /// <summary>
        /// 循环周期（月）
        /// </summary>
        public int? Cycle { get; set; }

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
        /// 关联设备信息
        /// </summary>
        public IEnumerable<EquRelatedDto> RelationDto { get; set; }
    }

    /// <summary>
    /// 设备点检计划分页Dto
    /// </summary>
    public class EquMaintenancePlanPagedQueryDto : PagerInfo
    {
        /// <summary>
        /// 点检计划编码
        /// </summary>
        public string? Code { get; set; }

        /// <summary>
        /// 点检计划名称
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// 版本
        /// </summary>
        public string? Version { get; set; }

        /// <summary>
        /// 点检计划状态
        /// </summary>
        public DisableOrEnableEnum? Status { get; set; }

        /// <summary>
        /// 点检类型;天/小时
        /// </summary>
        public EquipmentMaintenanceTypeEnum? Type { get; set; }

        /// <summary>
        /// 设备编码 
        /// </summary>
        public string? EquipmentCode { get; set; }

        /// <summary>
        /// 设备名称 
        /// </summary>
        public string? EquipmentName { get; set; }

        /// <summary>
        /// 点检执行人;用户中心UserId集合
        /// </summary>
        public string? ExecutorIds { get; set; }

        /// <summary>
        /// 点检负责人;用户中心UserId集合
        /// </summary>
        public string? LeaderIds { get; set; }
    }

    /// <summary>
    /// 设备点检计划更新Dto
    /// </summary>
    public record GenerateDto
    {
        /// <summary>
        /// 计划ID
        /// </summary>
        public long MaintenancePlanId { get; set; }

        /// <summary>
        /// 计划ID
        /// </summary>
        public int? ExecType { get; set; } = 0;
    }


    /// <summary>
    /// 设备点检计划更新Dto
    /// </summary>
    public record DeletesDto
    {
        /// <summary>
        /// 设备Id
        /// </summary>
        public IEnumerable<long> Ids { get; set; } 
    }

    #region  关联信息

    /// <summary>
    /// 设备点检计划更新Dto
    /// </summary>
    public record EquRelatedDto
    {

        /// <summary>
        /// 设备Id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 计划
        /// </summary>
        public long MaintenancePlanId { get; set; }

        /// <summary>
        /// 模板
        /// </summary>
        public long TemplateId { get; set; }

        /// <summary>
        /// 设备
        /// </summary>
        public long EquipmentId { get; set; }

        /// <summary>
        /// 执行人
        /// </summary>
        public string? ExecutorIds { get; set; }

        /// <summary>
        /// 责任人
        /// </summary>
        public string? LeaderIds { get; set; }
    }

    public record QueryEquRelationListDto
    {
        /// <summary>
        /// 计划
        /// </summary>
        public long MaintenancePlanId { get; set; }


        /// <summary>
        /// 模板
        /// </summary>
        public long TemplateId { get; set; }

        /// <summary>
        /// 模板
        /// </summary>
        public string TemplateCode { get; set; }

        /// <summary>
        /// 模板
        /// </summary>
        public string TemplateVersion { get; set; }

        /// <summary>
        /// 设备
        /// </summary>
        public long EquipmentId { get; set; }

        /// <summary>
        /// 设备
        /// </summary>
        public string EquipmentCode { get; set; }
        /// <summary>
        /// 设备名称
        /// </summary>
        public string EquipmentName { get; set; }

        /// <summary>
        /// 执行人
        /// </summary>
        public string? ExecutorIds { get; set; }

        /// <summary>
        /// 责任人
        /// </summary>
        public string? LeaderIds { get; set; }
    }
    #endregion
}
