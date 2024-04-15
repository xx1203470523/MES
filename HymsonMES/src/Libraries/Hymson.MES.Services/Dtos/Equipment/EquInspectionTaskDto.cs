using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;
using Hymson.MES.Services.Dtos.Process;

namespace Hymson.MES.Services.Dtos.Equipment
{
    /// <summary>
    /// 点检任务新增/更新Dto
    /// </summary>
    public record EquInspectionTaskSaveDto : BaseEntityDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        public string Code { get; set; }

       /// <summary>
        /// 点检类型 1、日点检 2、周点检
        /// </summary>
        public EquInspectionTypeEnum InspectionType { get; set; }

       /// <summary>
        /// 工作中心
        /// </summary>
        public long WorkCenterId { get; set; }

       /// <summary>
        /// 设备Id
        /// </summary>
        public long EquipmentId { get; set; }

       /// <summary>
        /// 执行月1-12
        /// </summary>
        public EquInspectionTaskMonthEnum? Month { get; set; }

       /// <summary>
        /// 执行日 1、周一，2、周二，3、周三，4、周四，5、周五，6、周六，7、周日
        /// </summary>
        public EquInspectionTaskDayEnum? Day { get; set; }

       /// <summary>
        /// 执行开始时间
        /// </summary>
        public string Time { get; set; }

       /// <summary>
        /// 完成时长（分钟）
        /// </summary>
        public int? CompleteTime { get; set; }

        /// <summary>
        /// 版本
        /// </summary>
        public string? Version { get; set; } = "";

        /// <summary>
        /// 类别 1、白班2、晚班3、巡检
        /// </summary>
        public EquInspectionTaskTypeEnum? Type { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; } = "";

        /// <summary>
        /// 关联项目
        /// </summary>
        public IEnumerable<EquInspectionTaskDetailsSaveDto>? TaskDetailsSaveDtos { get; set; }
    }

    public record EquInspectionTaskDetailsSaveDto : BaseEntityDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long? Id { get; set; }

        /// <summary>
        /// 点检任务Id
        /// </summary>
        public long? InspectionTaskId { get; set; }

        /// <summary>
        /// 点检项目Id
        /// </summary>
        public long InspectionItemId { get; set; }

        /// <summary>
        /// 基准值
        /// </summary>
        public decimal? BaseValue { get; set; }

        /// <summary>
        /// 最大值
        /// </summary>
        public decimal? MaxValue { get; set; }

        /// <summary>
        /// 最小值
        /// </summary>
        public decimal? MinValue { get; set; }

        /// <summary>
        /// 单位
        /// </summary>
        public string? Unit { get; set; } = "";

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; } = "";
    }

    public record EquInspectionTaskDetailDto : BaseEntityDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long? Id { get; set; }

        /// <summary>
        /// 点检项目Id
        /// </summary>
        public long InspectionItemId { get; set; }

        /// <summary>
        /// 点检项目编码
        /// </summary>
        public string InspectionItemCode { get; set; }

        public string Code { get; set; }

        /// <summary>
        /// 点检项目名称
        /// </summary>
        public string InspectionItemName { get; set; }

        /// <summary>
        /// 基准值
        /// </summary>
        public decimal? BaseValue { get; set; }

        /// <summary>
        /// 最大值
        /// </summary>
        public decimal? MaxValue { get; set; }

        /// <summary>
        /// 最小值
        /// </summary>
        public decimal? MinValue { get; set; }

        /// <summary>
        /// 单位
        /// </summary>
        public string? Unit { get; set; } = "";

        /// <summary>
        /// 备注
        /// </summary>
        public string? Remark { get; set; } = "";

    }
    /// <summary>
    /// 点检任务Dto
    /// </summary>
    public record EquInspectionTaskDto : BaseEntityDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 点检类型 1、日点检 2、周点检
        /// </summary>
        public EquInspectionTypeEnum InspectionType { get; set; }

       /// <summary>
        /// 工作中心
        /// </summary>
        public long WorkCenterId { get; set; }

       /// <summary>
        /// 设备Id
        /// </summary>
        public long EquipmentId { get; set; }

        /// <summary>
        /// 工作中心编码
        /// </summary>
        public string WorkCenterCode { get; set; }

        /// <summary>
        /// 设备编码
        /// </summary>
        public string EquipmentCode { get; set; }

        /// <summary>
        /// 设备名称
        /// </summary>
        public string EquipmentName { get; set; }

        /// <summary>
        /// 执行月1-12
        /// </summary>
        public EquInspectionTaskMonthEnum? Month { get; set; }

       /// <summary>
        /// 执行日 1、周一，2、周二，3、周三，4、周四，5、周五，6、周六，7、周日
        /// </summary>
        public EquInspectionTaskDayEnum? Day { get; set; }

        /// <summary>
        /// 单位
        /// </summary>
        public string Unit { get; set; }

       /// <summary>
        /// 执行开始时间
        /// </summary>
        public string Time { get; set; }

       /// <summary>
        /// 完成时长（分钟）
        /// </summary>
        public int? CompleteTime { get; set; }

        /// <summary>
        /// 版本
        /// </summary>
        public string? Version { get; set; } = "";

        /// <summary>
        /// 状态 1、新建2、启用3、保留4、废除
        /// </summary>
        public SysDataStatusEnum Status { get; set; }

        /// <summary>
        /// 类别 1、白班2、晚班3、巡检
        /// </summary>
        public EquInspectionTaskTypeEnum? Type { get; set; }

       /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

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
    /// 点检任务分页Dto
    /// </summary>
    public class EquInspectionTaskPagedQueryDto : PagerInfo 
    {
        /// <summary>
        /// 点检任务编码
        /// </summary>
        public string? Code { get; set; }

        /// <summary>
        /// 设备编码
        /// </summary>
        public string? EquipmentCode { get; set; }

        /// <summary>
        /// 工作中心编码
        /// </summary>
        public string? WorkCenterCode { get; set; }

        /// <summary>
        /// 点检类型 1、日点检 2、周点检
        /// </summary>
        public EquInspectionTypeEnum? InspectionType { get; set; }

        /// <summary>
        /// 点检类别 1、白班2、晚班3、巡检
        /// </summary>
        public EquInspectionTaskTypeEnum? Type { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public SysDataStatusEnum? Status { get; set; }
    }

    /// <summary>
    /// 生成点检录入任务
    /// </summary>
    public class GenerateInspectionRecordDto
    {
        /// <summary>
        /// 任务Id
        /// </summary>
        public long Id { get; set; }
    }

}
