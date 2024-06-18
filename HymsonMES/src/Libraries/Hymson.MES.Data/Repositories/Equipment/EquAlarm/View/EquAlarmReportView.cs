using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;

namespace Hymson.MES.Data.Repositories.Equipment.EquAlarm.View
{
    public class EquAlarmReportView : BaseEntity
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
        /// 状态;1：触发 2、恢复
        /// </summary>
        public EquipmentAlarmStatusEnum? Status { get; set; }
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
        /// 报警代码
        /// </summary>
        public string FaultCode { get; set; }
        /// <summary>
        /// 故障信息
        /// </summary>
        public string AlarmMsg { get; set; }
        /// <summary>
        /// 触发时间
        /// </summary>
        public DateTime LocalTime { get; set; }
    }
}
