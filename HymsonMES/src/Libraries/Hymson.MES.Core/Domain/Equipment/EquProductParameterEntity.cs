using Hymson.Infrastructure;

namespace Hymson.MES.Core.Domain.Equipment
{
    /// <summary>
    /// 设备生产参数，数据实体对象   
    /// equ_product_parameter
    /// @author Czhipu
    /// @date 2023-05-17 01:36:24
    /// </summary>
    public class EquProductParameterEntity : BaseEntity
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 当前工序id
        /// </summary>
        public long? ProcedureId { get; set; }

        /// <summary>
        /// 资源id
        /// </summary>
        public long? ResourceId { get; set; }

        /// <summary>
        /// 设备Id
        /// </summary>
        public long EquipmentId { get; set; }

        /// <summary>
        /// 传输时间
        /// </summary>
        public DateTime LocalTime { get; set; }

        /// <summary>
        /// 标准参数Id
        /// </summary>
        public long ParameterId { get; set; }

        /// <summary>
        /// 参数值
        /// </summary>
        public string ParamValue { get; set; }

        /// <summary>
        /// 标准上限
        /// </summary>
        public string? StandardUpperLimit { get; set; }

        /// <summary>
        /// 标准下限
        /// </summary>
        public string? StandardLowerLimit { get; set; }

        /// <summary>
        /// 判定结果
        /// </summary>
        public string? JudgmentResult { get; set; }

        /// <summary>
        /// 测试时长
        /// </summary>
        public string? TestDuration { get; set; }

        /// <summary>
        /// 测试时间
        /// </summary>
        public string? TestTime { get; set; }

        /// <summary>
        /// 测试结果
        /// </summary>
        public string? TestResult { get; set; }

        /// <summary>
        /// 参数采集到的时间
        /// </summary>
        public DateTime Timestamp { get; set; }

    }
}
