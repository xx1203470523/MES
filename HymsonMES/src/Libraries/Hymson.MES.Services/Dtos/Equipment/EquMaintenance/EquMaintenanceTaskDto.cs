using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Common;
using Hymson.MES.Core.Enums.Equipment;
using Hymson.MES.Core.Enums.Equipment.EquMaintenance;
using Hymson.MES.Services.Dtos.Integrated;

namespace Hymson.MES.Services.Dtos.Equipment.EquMaintenance
{
    /// <summary>
    /// 点检任务新增/更新Dto
    /// </summary>
    public record EquMaintenanceTaskSaveDto : BaseEntityDto
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
    public record EquMaintenanceTaskDto : BaseEntityDto
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
        public EquMaintenanceTaskStautusEnum? Status { get; set; }
        public string? StatusText { get; set; }

        /// <summary>
        /// 是否合格;0、不合格 1、合格
        /// </summary>
        public TrueFalseEmptyEnum? IsQualified { get; set; }
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
        public string? EquipmentCode { get; set; }

        /// <summary>
        /// 设备名称
        /// </summary>
        public string? EquipmentName { get; set; }

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
        /// 点检类型
        /// </summary>
        public EquipmentMaintenanceTypeEnum? PlanType { get; set; }
        public string? PlanTypeText { get; set; }

        /// <summary>
        /// 不合格处理方式;1-通过；2-不通过
        /// </summary>
        public EquMaintenanceTaskProcessedEnum? HandMethod { get; set; }

        /// <summary>
        /// 处理人
        /// </summary>
        public string? ProcessedBy { get; set; }

        /// <summary>
        /// 点检执行人;用户中心UserId集合
        /// </summary>
        public string? ExecutorIds { get; set; }

        /// <summary>
        /// 点检负责人;用户中心UserId集合
        /// </summary>
        public string? LeaderIds { get; set; }

        /// <summary>
        /// 是否延期
        /// </summary>
        public TrueOrFalseEnum? IsDefer { get; set; }
        public string? IsDeferText { get; set; }

        /// <summary>
        /// 计划备注
        /// </summary>
        public string? PlanRemark { get; set; }

        /// <summary>
        /// 延期原因
        /// </summary>
        public string? DeferReason { get; set; }

    }



    /// <summary>
    /// 点检任务分页Dto
    /// </summary>
    public class EquMaintenanceTaskPagedQueryDto : PagerInfo
    {
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
        public EquMaintenanceTaskStautusEnum? Status { get; set; }

        /// <summary>
        /// 是否合格;0、不合格 1、合格
        /// </summary>
        public TrueOrFalseEnum? IsQualified { get; set; }

        /// <summary>
        /// 处理结果 不合格处理方式;1-通过；2-不通过
        /// </summary>
        public EquMaintenanceTaskProcessedEnum? HandMethod { get; set; }

        /// <summary>
        /// 计划开始时间  时间范围  数组
        /// </summary>
        public DateTime[]? PlanBeginTime { get; set; }
    }


    /// <summary>
    /// 设备点检快照任务项目Dto
    /// </summary>
    public record EquMaintenanceTaskSnapshotItemQueryDto : BaseEntityDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long MaintenanceTaskId { get; set; }

    }

    /// <summary>
    /// 检验单状态Dto
    /// </summary>
    public record EquMaintenanceTaskOrderOperationStatusDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long OrderId { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public EquMaintenanceOperationTypeEnum OperationType { get; set; }

    }

    /// <summary>
    /// 点检任务项全部信息-执行时,
    /// </summary>
    public record EquMaintenanceTaskItemUnionSnapshotView : BaseEntityDto
    {
        /// <summary>
        /// EquMaintenanceItemId
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// 点检任务ID;equ_EquMaintenance_task表的Id
        /// </summary>
        public long? EquMaintenanceTaskId { get; set; }

        /// <summary>
        /// 点检项目快照ID;equ_EquMaintenance_item_snapshot表的Id
        /// </summary>
        public long EquMaintenanceItemSnapshotId { get; set; }

        /// <summary>
        /// 检验值
        /// </summary>
        public string? InspectionValue { get; set; }

        /// <summary>
        /// 是否合格;(0-否 1-是)
        /// </summary>
        public TrueFalseEmptyEnum? IsQualified { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string? Remark { get; set; }

        /// <summary>
        /// 站点Id
        /// </summary>
        public long? SiteId { get; set; }


        /// <summary>
        /// 点检项目ID;equ_EquMaintenance_item的Id
        /// </summary>
        public long EquMaintenanceItemId { get; set; }

        /// <summary>
        /// 点检项目编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 点检项目名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public DisableOrEnableEnum? Status { get; set; }

        /// <summary>
        /// 数值类型;文本/数值
        /// </summary>
        public DataTypeEnum? DataType { get; set; }

        /// <summary>
        /// 点检方式
        /// </summary>
        public EquMaintenanceItemMethodEnum? CheckType { get; set; }

        /// <summary>
        /// 作业方法
        /// </summary>
        public string? CheckMethod { get; set; }

        /// <summary>
        /// 单位ID;inte_unit表的Id
        /// </summary>
        public long? UnitId { get; set; }
        /// <summary>
        /// 单位
        /// </summary>
        public string? Unit { get; set; }

        /// <summary>
        /// 操作内容
        /// </summary>
        public string OperationContent { get; set; }

        /// <summary>
        /// 部件
        /// </summary>
        public string Components { get; set; }

        /// <summary>
        /// 规格下限;值来源于点检模板
        /// </summary>
        public decimal? LowerLimit { get; set; }

        /// <summary>
        /// 规格值（规格中心）;值来源于点检模板
        /// </summary>
        public decimal? ReferenceValue { get; set; }

        /// <summary>
        /// 规格上限;值来源于点检模板
        /// </summary>
        public decimal? UpperLimit { get; set; }

        /// <summary>
        /// 附件集合
        /// </summary>
        public IEnumerable<InteAttachmentBaseDto> Attachments { get; set; }

    }


    /// <summary>
    ///点检单项更新保存
    /// </summary>
    public record EquMaintenanceTaskItemSaveDto : BaseEntityDto
    {
        /// <summary>
        /// 任务id
        /// </summary>
        public long MaintenanceTaskId { get; set; }

        /// <summary>
        /// 样品参数
        /// </summary>
        public IEnumerable<EquMaintenanceTaskItemDetailDto> Details { get; set; }

    }

    public record EquMaintenanceTaskItemDetailDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 检验值
        /// </summary>
        public string? InspectionValue { get; set; }

        /// <summary>
        /// 是否合格;0、不合格 1、合格
        /// </summary>
        public TrueFalseEmptyEnum IsQualified { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string? Remark { get; set; }

        /// <summary>
        /// 参数附件
        /// </summary>
        public IEnumerable<InteAttachmentBaseDto>? Attachments { get; set; }

    }

    /// <summary>
    /// 完成Dto
    /// </summary>
    public record EquMaintenanceTaskCompleteDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long Id { get; set; }

    }

    /// <summary>
    /// 处理结果完成Dto
    /// </summary>
    public record EquMaintenanceTaskCloseDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long MaintenanceTaskId { get; set; }

        /// <summary>
        /// 不合格处理方式
        /// </summary>
        public EquMaintenanceTaskProcessedEnum HandMethod { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string? Remark { get; set; }

    }

    /// <summary>
    /// 保养延期
    /// </summary>
    public record EquMaintenanceTaskDeferDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long MaintenanceTaskId { get; set; }

        /// <summary>
        /// 延期日期 截止日期
        /// </summary>
        public DateTime? PlanBeginTime { get; set; }

        /// <summary>
        ///延期原因  备注
        /// </summary>
        public string? Remark { get; set; }

    }




    /// <summary>
    /// 附件保存dto
    /// </summary>
    public record EquMaintenanceTaskSaveAttachmentDto
    {
        /// <summary>
        /// 单据id
        /// </summary>
        public long OrderId { get; set; }

        /// <summary>
        /// 检验单（附件）
        /// </summary>
        public IEnumerable<InteAttachmentBaseDto> Attachments { get; set; }

    }

    /// <summary>
    /// 单据明细查询(处理结果查询分页)
    /// </summary>
    public class EquMaintenanceTaskItemPagedQueryDto : PagerInfo
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long MaintenanceTaskId { get; set; }

        /// <summary>
        /// 项目编码
        /// </summary>
        public string? ItemCode { get; set; }

    }

}
