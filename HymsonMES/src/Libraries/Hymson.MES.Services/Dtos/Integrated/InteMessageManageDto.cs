using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;

namespace Hymson.MES.Services.Dtos.Integrated
{
    /// <summary>
    /// 消息管理Dto（触发）
    /// </summary>
    public record InteMessageManageTriggerDto : BaseEntityDto
    {
        /// <summary>
        /// 消息单号
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 车间id
        /// </summary>
        public long WorkShopId { get; set; }

        /// <summary>
        /// 线体id
        /// </summary>
        public long LineId { get; set; }

        /// <summary>
        /// 资源ID
        /// </summary>
        public long? ResourceId { get; set; }

        /// <summary>
        /// 设备ID
        /// </summary>
        public long? EquipmentId { get; set; }

        /// <summary>
        /// 事件类型id
        /// </summary>
        public long EventTypeId { get; set; }

        /// <summary>
        /// 事件描述
        /// </summary>
        public string? EventDescription { get; set; }

        /// <summary>
        /// 消息状态;1、触发2、接收3、处理4、关闭
        /// </summary>
        public MessageStatusEnum Status { get; set; }

        /// <summary>
        /// 紧急程度;1、高2、中3、低
        /// </summary>
        public DegreeEnum UrgencyLevel { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string? Remark { get; set; }

    }

    /// <summary>
    /// 消息管理Dto（接收）
    /// </summary>
    public record InteMessageManageReceiveDto : BaseEntityDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long Id { get; set; }
    }

    /// <summary>
    /// 消息管理Dto（处理）
    /// </summary>
    public record InteMessageManageHandleDto : BaseEntityDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 责任部门
        /// </summary>
        public long DepartmentId { get; set; }

        /// <summary>
        /// 责任人
        /// </summary>
        public string ResponsibleBy { get; set; }

        /// <summary>
        /// 原因分析
        /// </summary>
        public string ReasonAnalysis { get; set; }

        /// <summary>
        /// 处理方案
        /// </summary>
        public string HandleSolution { get; set; }

        /// <summary>
        /// 原因分析（附件）
        /// </summary>
        public IEnumerable<string>? ReasonAttachments { get; set; }

        /// <summary>
        /// 处理方案（附件）
        /// </summary>
        public IEnumerable<string>? HandleAttachments { get; set; }

        /// <summary>
        /// 备注（处理）
        /// </summary>
        public string? HandleRemark { get; set; }
    }

    /// <summary>
    /// 消息管理Dto（关闭）
    /// </summary>
    public record InteMessageManageCloseDto : BaseEntityDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 备注（评价）
        /// </summary>
        public string? EvaluateRemark { get; set; }
    }

    /// <summary>
    /// 消息管理Dto
    /// </summary>
    public record InteMessageManageDto : BaseEntityDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 消息单号
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 车间id
        /// </summary>
        public long WorkShopId { get; set; }

        /// <summary>
        /// 车间名称
        /// </summary>
        public string WorkShopName { get; set; }

        /// <summary>
        /// 线体id
        /// </summary>
        public long LineId { get; set; }

        /// <summary>
        /// 线体名称
        /// </summary>
        public string LineName { get; set; }

        /// <summary>
        /// 资源ID
        /// </summary>
        public long? ResourceId { get; set; }

        /// <summary>
        /// 设备ID
        /// </summary>
        public long? EquipmentId { get; set; }

        /// <summary>
        /// 事件类型id
        /// </summary>
        public long EventTypeId { get; set; }

        /// <summary>
        /// 事件类型名称
        /// </summary>
        public string EventTypeName { get; set; }

        /// <summary>
        /// 事件描述
        /// </summary>
        public string? EventDescription { get; set; }

        /// <summary>
        /// 消息状态;1、触发2、接收3、处理4、关闭
        /// </summary>
        public MessageStatusEnum Status { get; set; }

        /// <summary>
        /// 紧急程度;1、高2、中3、低
        /// </summary>
        public DegreeEnum UrgencyLevel { get; set; }

        /// <summary>
        /// 责任部门
        /// </summary>
        public long? DepartmentId { get; set; }

        /// <summary>
        /// 责任人
        /// </summary>
        public string? ResponsibleBy { get; set; }

        /// <summary>
        /// 原因分析
        /// </summary>
        public string? ReasonAnalysis { get; set; }

        /// <summary>
        /// 处理方案
        /// </summary>
        public string? HandleSolution { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string? Remark { get; set; }

        /// <summary>
        /// 接收时长
        /// </summary>
        public decimal? ReceiveDuration { get; set; }

        /// <summary>
        /// 处理时长
        /// </summary>
        public decimal? HandleDuration { get; set; }

        /// <summary>
        /// 评价时间
        /// </summary>
        public string? EvaluateOn { get; set; }

        /// <summary>
        /// 评价人
        /// </summary>
        public string? EvaluateBy { get; set; }

        /// <summary>
        /// 更新人
        /// </summary>
        public string UpdatedBy { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime? UpdatedOn { get; set; }

    }

    /// <summary>
    /// 消息管理分页Dto
    /// </summary>
    public class InteMessageManagePagedQueryDto : PagerInfo { }

}
