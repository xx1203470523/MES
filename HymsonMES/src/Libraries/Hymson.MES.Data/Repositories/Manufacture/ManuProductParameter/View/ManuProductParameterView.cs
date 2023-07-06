using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;

namespace Hymson.MES.Data.Repositories.Manufacture
{
    public class ManuProductParameterView : BaseEntity
    {
        /// <summary>
        /// 站点ID
        /// </summary>
        public long SiteId { get; set; }
        /// <summary>
        /// 工序ID
        /// </summary>
        public long ProcedureId { get; set; }
        /// <summary>
        /// 资源ID
        /// </summary>
        public long ResourceId { get; set; }
        /// <summary>
        /// 设备ID
        /// </summary>
        public long EquipmentId { get; set; }
        /// <summary>
        /// 条码
        /// </summary>
        public string SFC { get; set; }
        /// <summary>
        /// 工单ID
        /// </summary>
        public long WorkOrderId { get; set; }
        /// <summary>
        /// 产品ID
        /// </summary>
        public long ProductId { get; set; }
        /// <summary>
        /// 设备本地时间
        /// </summary>
        public DateTime LocalTime { get; set; }
        /// <summary>
        /// 参数编码
        /// </summary>
        public string ParameterCode { get; set; }
        /// <summary>
        /// 参数名称
        /// </summary>
        public string ParameterName { get; set; }
        /// <summary>
        /// 参数值
        /// </summary>
        public string ParamValue { get; set; }
        /// <summary>
        /// 参数单位
        /// </summary>
        public string ParameterUnit { get; set; }
        /// <summary>
        /// 步骤ID，出站步骤ID
        /// </summary>
        public long StepId { get; set; }
        /// <summary>
        /// 参数类型
        /// </summary>
        public ParameterTypeEnum? ParameterType { get; set; }
    }
}
