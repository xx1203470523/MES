using Hymson.Infrastructure;
using Hymson.MES.Core.Enums.Manufacture;
using OfficeOpenXml.Attributes;

namespace Hymson.MES.Services.Dtos.Manufacture
{
    public class ManuEquipmentStatusTimeDto
    {
    }

    /// <summary>
    /// 返回Dto
    /// </summary>
    public record ManuEquipmentStatusReportViewDto : BaseEntityDto
    {
        /// <summary> 
        /// 工作中心名称
        /// </summary>
        public string WorkCenterCode { get; set; }
        /// <summary> 
        /// 工作中心名称
        /// </summary>
        public string WorkCenterName { get; set; }

        /// <summary>
        /// 工序名称
        /// </summary>
        public string ProcedureCode { get; set; }
        /// <summary>
        /// 工序名称
        /// </summary>
        public string ProcedureName { get; set; }

        /// <summary>
        /// 设备编码
        /// </summary>
        public string EquipmentCode { get; set; }

        /// <summary>
        /// 设备名称
        /// </summary>
        public string EquipmentName { get; set; }

        /// <summary>
        /// 当前状态
        /// </summary>
        public ManuEquipmentStatusEnum CurrentStatus { get; set; }

        /// <summary>
        /// 时间
        /// </summary>
        public DateTime LocalTime { get; set; }

        /// <summary>
        /// 时间 
        /// </summary>
        public DateTime UpdatedOn { get; set; }

        /// <summary>
        /// 状态开始时间
        /// </summary>
        public DateTime BeginTime { get; set; }

        /// <summary>
        /// 状态结束时间
        /// </summary>
        public DateTime EndTime { get; set; }

        /// <summary>
        /// 状态持续时间（单位秒）
        /// </summary>
        public int StatusDuration { get; set; }
    }

    /// <summary>
    /// 设备状态时间分页Dto
    /// </summary>
    public class ManuEquipmentStatusTimePagedQueryDto : PagerInfo
    {
        /// <summary>
        /// 工作中心名称
        /// </summary>
        public long? WorkCenterId { get; set; }

        /// <summary>
        /// 设备编码
        /// </summary>
        public long? EquipmentId { get; set; }
    }

    public class ManuEquipmentStatusExportResultDto
    {
        public string Path { get; set; }

        public string FileName { get; set; }
    }

    /// <summary>
    /// 返回Dto
    /// </summary>
    public record ManuEquipmentStatusRExportDto : BaseExcelDto
    {
        /// <summary> 
        /// 工作中心编码
        /// </summary>
        [EpplusTableColumn(Header = "工作中心编码", Order = 1)]
        public string WorkCenterCode { get; set; }

        /// <summary> 
        /// 工作中心名称
        /// </summary>
        [EpplusTableColumn(Header = "工作中心名称", Order = 2)]
        public string WorkCenterName { get; set; }

        /// <summary>
        /// 工序编码
        /// </summary>
        [EpplusTableColumn(Header = "工序编码", Order = 3)]
        public string ProcedureCode { get; set; }

        /// <summary>
        /// 工序名称
        /// </summary>
        [EpplusTableColumn(Header = "工序名称", Order = 4)]
        public string ProcedureName { get; set; }

        /// <summary>
        /// 设备编码
        /// </summary>
        [EpplusTableColumn(Header = "设备编码", Order = 5)]
        public string EquipmentCode { get; set; }

        /// <summary>
        /// 设备名称
        /// </summary>
        [EpplusTableColumn(Header = "设备名称", Order = 6)]
        public string EquipmentName { get; set; }

        /// <summary>
        /// 当前状态ManuEquipmentStatusEnum
        /// </summary>
        [EpplusTableColumn(Header = "运行状态", Order = 7)]
        public string CurrentStatus { get; set; }

        /// <summary>
        /// 状态持续时间（单位秒）
        /// </summary>
        [EpplusTableColumn(Header = "状态持续时间", Order = 8)]
        public int StatusDuration { get; set; }

        /// <summary>
        /// 状态开始时间
        /// </summary>
        [EpplusTableColumn(Header = "开始时间", Order = 9)]
        public DateTime BeginTime { get; set; }

        /// <summary>
        /// 状态结束时间
        /// </summary>
        [EpplusTableColumn(Header = "结束时间", Order = 10)]
        public DateTime EndTime { get; set; }

        /// <summary>
        /// 时间 
        /// </summary>
        [EpplusTableColumn(Header = "更新时间", Order = 11)]
        public DateTime? UpdatedOn { get; set; }
    }
}
