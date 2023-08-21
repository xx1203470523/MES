using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;
using OfficeOpenXml.Attributes;

namespace Hymson.MES.Services.Dtos.Equipment
{
    public record EquAlarmReportExportDto : BaseExcelDto
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
        /// 状态;1：触发 2、恢复
        /// </summary>
        [EpplusTableColumn(Header = "设备状态", Order = 3)]
        public EquipmentAlarmStatusEnum? Status { get; set; }
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
        /// 报警代码
        /// </summary>
        [EpplusTableColumn(Header = "报警代码", Order = 9)]
        public string FaultCode { get; set; }
        /// <summary>
        /// 故障信息
        /// </summary>
        [EpplusTableColumn(Header = "故障信息", Order = 10)]
        public string AlarmMsg { get; set; }
        /// <summary>
        /// 触发时间
        /// </summary>
        [EpplusTableColumn(Header = "触发时间", Order = 11)]
        public DateTime LocalTime { get; set; }

    }
}
