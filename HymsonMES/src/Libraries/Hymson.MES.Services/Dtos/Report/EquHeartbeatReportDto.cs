using Hymson.Infrastructure;

namespace Hymson.MES.Services.Dtos.Report
{
    public record EquHeartbeatReportViewDto : BaseEntityDto
    {
        /// <summary>
        /// 设备ID
        /// </summary>
        public long EquipmentId { get; set; }
        /// <summary>
        /// 设备编码
        /// </summary>
        public string EquipmentCode { get; set; }
        /// <summary>
        /// 设备名称
        /// </summary>
        public string EquipmentName { get; set; }
        /// <summary>
        /// 状态;0：离线 1、在线
        /// </summary>
        public bool Status { get; set; }
        /// <summary>
        /// 工作中心
        /// </summary>
        public long WorkCenterLineId { get; set; }
        /// <summary>
        /// 资源编码
        /// </summary>
        public string ResCode { get; set; }
        /// <summary>
        /// 资源名称
        /// </summary>
        public string ResName { get; set; }
        /// <summary>
        /// 工序编码
        /// </summary>
        public string ProcedureCode { get; set; }
        /// <summary>
        /// 工序名称
        /// </summary>
        public string ProcedureName { get; set; }
        /// <summary>
        /// 最后在线时间
        /// </summary>
        public DateTime LastOnLineTime { get; set; }
        /// <summary>
        /// 上报时间
        /// </summary>
        public DateTime CreateOn { get; set; }
    }

    public class EquHeartbeatReportPagedQueryDto : PagerInfo
    {
        /// <summary>
        /// 工序名称
        /// </summary>
        public string? ProcedureName { get; set; }
        /// <summary>
        /// 工序编码
        /// </summary>
        public string? ProcedureCode { get; set; }
        /// <summary>
        /// 设备编码
        /// </summary>
        public string? EquipmentCode { get; set; }
        /// <summary>
        /// 设备名称
        /// </summary>
        public string? EquipmentName { get; set; }
        /// <summary>
        /// 资源编码
        /// </summary>
        public string? ResCode { get; set; }
        /// <summary>
        /// 资源名称
        /// </summary>
        public string? ResName { get; set; }
        /// <summary>
        /// 状态;0：离线 1、在线
        /// </summary>
        public bool? Status { get; set; }

        /// <summary>
        /// 采集时间
        /// </summary>
        public DateTime[]? AcquisitionTime { get; set; }
    }

}
