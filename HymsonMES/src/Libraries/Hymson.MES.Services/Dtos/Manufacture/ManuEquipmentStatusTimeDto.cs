using Hymson.Infrastructure;
using Hymson.MES.Core.Enums.Manufacture;

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
}
