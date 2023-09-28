using Hymson.Infrastructure;

namespace Hymson.MES.Services.Dtos.Report
{
    public class ManuProductParameterReportPagedQueryDto : PagerInfo
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
        /// 参数编码
        /// </summary>
        public string? ParameterCode { get; set; }
        /// <summary>
        /// 参数名称
        /// </summary>
        public string? ParameterName { get; set; }
        /// <summary>
        /// 条码字符，多个使用分号;分割
        /// </summary>
        public string? SFCStr { get; set; }
        /// <summary>
        /// 条码集合
        /// </summary>
        public string[]? SFCS { get; set; }
        /// <summary>
        /// 上报时间
        /// </summary>
        public DateTime[]? LocalTimes { get; set; }
    }

    public record ManuProductParameterReportViewDto : BaseEntityDto
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
        /// 条码
        /// </summary>
        public string SFC { get; set; }
        /// <summary>
        /// 参数编码
        /// </summary>
        public string ParameterCode { get; set; }
        /// <summary>
        /// 参数单位
        /// </summary>
        public string ParameterUnit { get; set; }
        /// <summary>
        /// 参数值
        /// </summary>
        public string Paramvalue { get; set; }
        /// <summary>
        /// 参数值
        /// </summary>
        public string ParameterName { get; set; }
        /// <summary>
        /// 上限
        /// </summary>
        public string StandardUpperLimit { get; set; }
        /// <summary>
        /// 下限
        /// </summary>
        public string StandardLowerLimit { get; set; }
        /// <summary>
        /// 判定结果（LA梁工硬要添加只做展示）
        /// </summary>
        public string JudgmentResult { get; set; }
        /// <summary>
        /// 测试持续时间（LA梁工硬要添加只做展示）
        /// </summary>
        public string TestDuration { get; set; }
        /// <summary>
        /// 测试时间（LA梁工硬要添加只做展示）
        /// </summary>
        public string TestTime { get; set; }
        /// <summary>
        /// 测试结果（LA梁工硬要添加只做展示）
        /// </summary>
        public string TestResult { get; set; }
        /// <summary>
        /// 触发时间
        /// </summary>
        public DateTime LocalTime { get; set; }
        public DateTime CreatedOn { get; set; }

        public DateTime UpdatedOn { get; set; }
    }
}
