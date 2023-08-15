using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;

namespace Hymson.MES.Services.Dtos.Integrated
{
    /// <summary>
    /// 消息管理新增/更新Dto
    /// </summary>
    public record InteMessageManageSaveDto : BaseEntityDto
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
        /// 推送场景;1、触发2、接收3、接收升级4、处理5、处理升级6、关闭
        /// </summary>
        public PushSceneEnum Status { get; set; }

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
        /// 推送场景;1、触发2、接收3、接收升级4、处理5、处理升级6、关闭
        /// </summary>
        public PushSceneEnum Status { get; set; }

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
