using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;

namespace Hymson.MES.Services.Dtos.Equipment
{
    /// <summary>
    /// 点检记录表新增/更新Dto
    /// </summary>
    public record EquInspectionRecordSaveDto : BaseEntityDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 点检单号
        /// </summary>
        public string OrderCode { get; set; }

        /// <summary>
        /// 点检任务Id
        /// </summary>
        public long? InspectionTaskSnapshootId { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime? StartExecuTime { get; set; }

        /// <summary>
        /// 1、待检验2、检验中3、已完成
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// 是否合格
        /// </summary>
        public string IsQualified { get; set; }

        /// <summary>
        /// 是否通知维修
        /// </summary>
        public string IsNoticeRepair { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedOn { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public string CreatedBy { get; set; }

        /// <summary>
        /// 更新人
        /// </summary>
        public string UpdatedBy { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime? UpdatedOn { get; set; }

        /// <summary>
        /// 站点ID
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 删除标识
        /// </summary>
        public long IsDeleted { get; set; }


    }

    /// <summary>
    /// 点检记录表Dto
    /// </summary>
    public record EquInspectionRecordDto : BaseEntityDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime? StartExecuTime { get; set; }

        /// <summary>
        /// 设备编码
        /// </summary>
        public string EquipmentCode { get; set; }

        /// <summary>
        /// 设备名称
        /// </summary>
        public string EquipmentName { get; set; }

        /// <summary>
        /// 工作中心
        /// </summary>
        public string WorkCenterCode { get; set; }

        /// <summary>
        /// 点检类型 1、日点检 2、周点检
        /// </summary>
        public EquInspectionTypeEnum InspectionType { get; set; }

        /// <summary>
        /// 类别 1、白班2、晚班3、巡检
        /// </summary>
        public EquInspectionTaskTypeEnum Type { get; set; }

        /// <summary>
        /// 点检单号
        /// </summary>
        public string OrderCode { get; set; }

        ///// <summary>
        // /// 1、待检验2、检验中3、已完成
        // /// </summary>
        // public string Status { get; set; }

        ///// <summary>
        // /// 是否合格
        // /// </summary>
        // public string IsQualified { get; set; }

        ///// <summary>
        // /// 是否通知维修
        // /// </summary>
        // public string IsNoticeRepair { get; set; }

        ///// <summary>
        // /// 备注
        // /// </summary>
        // public string Remark { get; set; }

        /// <summary>
        /// 完成时长（分钟）
        /// </summary>
        public int? CompleteTime { get; set; }

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
    /// 点检操作查询dto
    /// </summary>
    public record EquInspectionOperateDto : BaseEntityDto
    {
        public long Id { get; set; }

        /// <summary>
        /// 点检单号
        /// </summary>
        public string OrderCode { get; set; }

        /// <summary>
        /// 设备编码
        /// </summary>
        public string EquipmentCode { get; set; }

        /// <summary>
        /// 设备名称
        /// </summary>
        public string EquipmentName { get; set; }

        /// <summary>
        /// 点检人员
        /// </summary>
        public string OperateBy { get; set; }

        /// <summary>
        /// 点检项目和结果列表
        /// </summary>
        public IEnumerable<EquInspectioTaskItemDto> TaskItemDtos { get; set; }
    }

    /// <summary>
    /// 完成Dto
    /// </summary>
    public record EquInspectionCompleteDto
    {
        /// <summary>
        /// 记录Id
        /// </summary>
        public long Id { get; set; }
    }

    /// <summary>
    /// 保存校验dto
    /// </summary>
    public record EquInspectionSaveDto : BaseEntityDto
    {
        /// <summary>
        /// 记录Id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 是否通知维修
        /// </summary>
        public bool IsNoticeRepair { get; set; } = false;

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; } = "";

        /// <summary>
        /// 点检项目和结果列表
        /// </summary>
        public IEnumerable<EquInspectioTaskSaveDto> TaskItemDtos { get; set; }
    }

    /// <summary>
    /// 点检记录表Dto
    /// </summary>
    public record EquInspectioTaskSaveDto : BaseEntityDto
    {
        /// <summary>
        /// 记录详情Id
        /// </summary>
        public long InspectionRecordDetailId { get; set; }

        /// <summary>
        /// 点检结果
        /// </summary>
        public string InspectionResult { get; set; } = "";

        /// <summary>
        /// 是否合格
        /// </summary>
        public bool? IsQualified { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; } = "";
    }

    /// <summary>
    /// 点检记录表Dto
    /// </summary>
    public record EquInspectioTaskItemDto : BaseEntityDto
    {
        /// <summary>
        /// 记录详情Id
        /// </summary>
        public long InspectionRecordDetailId { get; set; }

        /// <summary>
        /// 点检项目编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 点检项目名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 操作方法
        /// </summary>
        public string OperationMethod { get; set; }

        /// <summary>
        /// 操作内容
        /// </summary>
        public string OperationContent { get; set; }

        /// <summary>
        /// 点检结果
        /// </summary>
        public string InspectionResult { get; set; } = "";

        /// <summary>
        /// 是否合格
        /// </summary>
        public bool? IsQualified { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; } = "";
    }

    /// <summary>
    /// 点检记录表分页Dto
    /// </summary>
    public class EquInspectionRecordPagedQueryDto : PagerInfo
    {
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime StartExecuTime { get; set; }

        /// <summary>
        /// 设备编码
        /// </summary>
        public string? EquipmentCode { get; set; }

        /// <summary>
        /// 设备名称
        /// </summary>
        public string? EquipmentName { get; set; }

        /// <summary>
        /// 工作中心
        /// </summary>
        public string? WorkCenterCode { get; set; }

        /// <summary>
        /// 工作中心Id
        /// </summary>
        public long? WorkCenterId { get; set; }

        /// <summary>
        /// 点检类型 1、日点检 2、周点检
        /// </summary>
        public EquInspectionTypeEnum? InspectionType { get; set; }

        /// <summary>
        /// 类别 1、白班2、晚班3、巡检
        /// </summary>
        public EquInspectionTaskTypeEnum? Type { get; set; }
    }

}
