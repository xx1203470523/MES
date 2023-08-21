using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;

namespace Hymson.MES.Data.Repositories.Equipment
{
    public class EquAlarmReportPagedQuery : PagerInfo
    {
        /// <summary>
        /// 站点id
        /// </summary>
        public long? SiteId { get; set; }
        /// <summary>
        /// 工序名称
        /// </summary>
        public string? ProcedureName { get; set; }
        /// <summary>
        /// 工序编码
        /// </summary>
        public string? ProcedureCode { get; set; }
        /// <summary>
        /// 工序编码集合
        /// </summary>
        public string[]? ProcedureCodes { get; set; }
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
        /// 状态;1：触发 2、恢复
        /// </summary>
        public EquipmentAlarmStatusEnum? Status { get; set; }
        /// <summary>
        /// 触发时间
        /// </summary>
        public DateTime[]? TriggerTimes { get; set; }
    }
}
