using Hymson.Infrastructure;
using Hymson.MES.Core.Enums.Manufacture;

namespace Hymson.MES.Data.Repositories.Equipment.Qkny
{
    public class ManuEquipmentStatusReportView : BaseEntity
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
        /// 状态
        /// </summary>
        public ManuEquipmentStatusEnum CurrentStatus { get; set; }

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
}
