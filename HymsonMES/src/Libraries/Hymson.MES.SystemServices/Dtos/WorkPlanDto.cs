using Hymson.Infrastructure;

namespace Hymson.MES.SystemServices.Dtos
{
    /// <summary>
    /// 工作计划 Dto
    /// </summary>
    public record WorkPlanDto : BaseEntityDto
    {
        /// <summary>
        /// 产线编码（隔离转子线和定子线数据）
        /// </summary>
        public string LineCode { get; set; } = "";

        /// <summary>
        /// 计划单号 
        /// </summary>
        public string PlanCode { get; set; } = "";

        /// <summary>
        /// 需求单号
        /// </summary>
        public string? RequirementNumber { get; set; }

        /// <summary>
        /// 产品编号 
        /// </summary>
        public string ProductCode { get; set; } = "";

        /// <summary>
        /// 产品版本
        /// </summary>
        public string ProductVersion { get; set; } = "";

        /// <summary>
        /// Bom编码
        /// </summary>
        public string BomCode { get; set; } = "";

        /// <summary>
        /// Bom版本
        /// </summary>
        public string BomVersion { get; set; } = "";

        /// <summary>
        /// 计划类型
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// 工单数量
        /// </summary>
        public decimal Qty { get; set; }

        /// <summary>
        /// 计划生产时间
        /// </summary>
        public DateTime? StartTime { get; set; }

        /// <summary>
        /// 计划结束时间
        /// </summary>
        public DateTime? EndTime { get; set; }

    }
}
