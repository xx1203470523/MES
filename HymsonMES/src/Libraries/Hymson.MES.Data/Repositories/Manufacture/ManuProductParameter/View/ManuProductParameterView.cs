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
        public string ParameterValue { get; set; }
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


    /// <summary>
    /// Pack
    /// </summary>
    public class ManuProductParameterPackEOLView : BaseEntity
    {
        /// <summary>
        /// 条码
        /// </summary>
        public string SFC { get; set; }
        /// <summary>
        /// 开路电压
        /// </summary>
        public string Voltage { get; set; }
        /// <summary>
        /// 内阻
        /// </summary>
        public string InternalResistance { get; set; }
        /// <summary>
        /// PACK正极绝缘阻抗
        /// </summary>
        public string PositiveImpedance { get; set; }
        /// <summary>
        /// PACK负极绝缘阻抗
        /// </summary>
        public string NegativeImpedance { get; set; }
        /// <summary>
        /// PACK正极耐压
        /// </summary>
        public string PositivePressurization { get; set; }
        /// <summary>
        /// PACK负极耐压
        /// </summary>
        public string NegativePressurization { get; set; }
        /// <summary>
        /// 压差
        /// </summary> 
        public string DifferentialPressure { get; set; }
        /// <summary>
        /// 温差
        /// </summary>
        public string TemperatureDifference { get; set; }
        /// <summary>
        /// 判定结果
        /// </summary>
        public string JudgmentResult { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }

    /// <summary>
    /// 配件
    /// </summary>
    public class ManuProductParameterCCSView : BaseEntity 
    {
        /// <summary>
        /// 条码
        /// </summary>
        public string SFC { get; set; }
        /// <summary>
        /// 测试压力
        /// </summary>
        public string TestPressure { get; set; }
        /// <summary>
        /// 压力衰减
        /// </summary>
        public string PressureDecay { get; set; }
        /// <summary>
        /// 泄漏率
        /// </summary>
        public string Leakage { get; set; }
        /// <summary>
        /// 判定结果
        /// </summary>
        public string JudgmentResult { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }

    /// <summary>
    /// OCV
    /// </summary>
    public class ManuProductParameterOCVView : BaseEntity
    {
        /// <summary> 
        /// 条码
        /// </summary>
        public string PSFC { get; set; }
        /// <summary>
        /// 条码
        /// </summary>
        public string MSFC { get; set; }
        /// <summary>
        /// 条码
        /// </summary>
        public string BSFC { get; set; }
        /// <summary>
        /// 电压
        /// </summary>
        public string Voltage { get; set; }
        /// <summary>
        /// 内阻
        /// </summary>
        public string PressureDecay { get; set; }

        /// <summary>
        /// 判定结果
        /// </summary>
        public string JudgmentResult { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }
}
