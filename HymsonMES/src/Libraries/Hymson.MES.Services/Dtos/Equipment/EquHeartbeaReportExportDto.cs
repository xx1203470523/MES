using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;
using OfficeOpenXml.Attributes;

namespace Hymson.MES.Services.Dtos.Equipment
{
    public record EquHeartbeaReportExportDto : BaseExcelDto
    {
        /// <summary>
        /// 设备编码
        /// </summary>
        [EpplusTableColumn(Header = "设备编码", Order = 1)]
        public string EquipmentCode { get; set; }
        /// <summary>
        /// 设备名称
        /// </summary>
        [EpplusTableColumn(Header = "设备名称", Order = 2)]
        public string EquipmentName { get; set; }
        /// <summary>
        /// 状态;0：离线 1、在线
        /// </summary>
        [EpplusTableColumn(Header = "心跳状态", Order = 3)]
        public bool Status { get; set; }
        /// <summary>
        /// 设备故障状态
        /// </summary>
        [EpplusTableColumn(Header = "设备状态", Order = 4)]
        public EquipmentStateEnum? EquipmentStatus { get; set; }
        /// <summary>
        /// 资源编码
        /// </summary>
        [EpplusTableColumn(Header = "资源编码", Order = 5)]
        public string ResCode { get; set; }
        /// <summary>
        /// 资源名称
        /// </summary>
        [EpplusTableColumn(Header = "资源名称", Order = 6)]
        public string ResName { get; set; }
        /// <summary>
        /// 工序编码
        /// </summary>
        [EpplusTableColumn(Header = "工序编码", Order = 7)]
        public string ProcedureCode { get; set; }
        /// <summary>
        /// 工序名称
        /// </summary>
        [EpplusTableColumn(Header = "工序名称", Order = 8)]
        public string ProcedureName { get; set; }
        /// <summary>
        /// 最后在线时间
        /// </summary>
        [EpplusTableColumn(Header = "最后在线时间", Order = 9)]
        public DateTime LastOnLineTime { get; set; }

    }
}
