using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Equipment;

namespace Hymson.MES.Services.Dtos.Equipment
{
    /// <summary>
    /// 点检任务新增/更新Dto
    /// </summary>
    public record EquSpotcheckTaskSaveDto : BaseEntityDto
    {
        /// <summary>
        /// 
        /// </summary>
        public long Id { get; set; }

       /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

       
    }

    /// <summary>
    /// 点检任务Dto
    /// </summary>
    public record EquSpotcheckTaskDto : BaseEntityDto
    {
        /// <summary>
        /// 
        /// </summary>
        public long Id { get; set; }

       /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }
        /// <summary>
        /// 点检任务编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 点检任务名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 开始时间（实际）
        /// </summary>
        public DateTime? BeginTime { get; set; }

        /// <summary>
        /// 结束时间（实际）
        /// </summary>
        public DateTime? EndTime { get; set; }

        /// <summary>
        /// 状态;1:待处理、2:处理中、3:待审核、4:已关闭
        /// </summary>
        public EquSpotcheckTaskStautusEnum? Status { get; set; }
        public string? StatusText { get; set; }

        /// <summary>
        /// 是否合格;0、不合格 1、合格
        /// </summary>
        public TrueOrFalseEnum? IsQualified { get; set; }
        public string? IsQualifiedText { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string? Remark { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public string? CreatedBy { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? CreatedOn { get; set; }

        /// <summary>
        /// 最后修改人
        /// </summary>
        public string? UpdatedBy { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? UpdatedOn { get; set; }

        /// <summary>
        /// 是否逻辑删除
        /// </summary>
        public long? IsDeleted { get; set; }

        /// <summary>
        /// 点检计划编码
        /// </summary>
        public string? PlanCode { get; set; }

        /// <summary>
        /// 版本
        /// </summary>
        public string? Version { get; set; }

        /// <summary>
        /// 设备ID;equ_equipment表的Id
        /// </summary>
        public long? EquipmentId { get; set; }

        /// <summary>
        /// 设备编码
        /// </summary>
        public string? EquipmentCode{ get; set; }

        /// <summary>
        /// 设备名称
        /// </summary>
        public string? EquipmentName{ get; set; }

        /// <summary>
        /// 位置
        /// </summary>
        public string? Location { get; set; }

        /// <summary>
        /// 开始时间（计划）
        /// </summary>
        public DateTime? PlanBeginTime { get; set; }

        /// <summary>
        /// 结束时间（计划）
        /// </summary>
        public DateTime? PlanEndTime { get; set; }

        /// <summary>
        /// 不合格处理方式;1-通过；2-不通过
        /// </summary>
        public SpotcheckTaskProcessedEnum? HandMethod { get; set; }

        /// <summary>
        /// 处理人
        /// </summary>
        public string? ProcessedBy { get; set; }

    }

 

    /// <summary>
    /// 点检任务分页Dto
    /// </summary>
    public class EquSpotcheckTaskPagedQueryDto : PagerInfo {
        /// <summary>
        /// 点检任务编码
        /// </summary>
        public string? Code { get; set; }

        /// <summary>
        /// 点检任务名称
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// 负责人
        /// </summary>
        public string? LeaderIds { get; set; }

        /// <summary>
        /// 设备编码
        /// </summary>
        public string? EquipmentCode { get; set; }

        /// <summary>
        /// 状态;1:待处理、2:处理中、3:待审核、4:已关闭
        /// </summary>
        public EquSpotcheckTaskStautusEnum? Status { get; set; }

        /// <summary>
        /// 是否合格;0、不合格 1、合格
        /// </summary>
        public TrueOrFalseEnum? IsQualified { get; set; }

        /// <summary>
        /// 处理结果 不合格处理方式;1-通过；2-不通过
        /// </summary>
        public SpotcheckTaskProcessedEnum? HandMethod { get; set; }

        /// <summary>
        /// 计划开始时间  时间范围  数组
        /// </summary>
        public DateTime[]? PlanStartTime { get; set; }
    }


    /// <summary>
    /// 设备点检快照任务项目Dto
    /// </summary>
    public record SpotcheckTaskSnapshotItemQueryDto : BaseEntityDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long SpotCheckTaskId { get; set; }

    }

    /// <summary>
    /// 点检任务项全部信息-执行时,
    /// </summary>
    public record TaskItemInfoView()
    {
        
    }

}
