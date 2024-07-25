using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Plan;

namespace Hymson.MES.SystemServices.Dtos
{
    /// <summary>
    /// 工作计划 Dto
    /// </summary>
    public record SyncWorkPlanDto : BaseEntityDto
    {
        /// <summary>
        /// 计划类型
        /// </summary>
        public PlanWorkOrderTypeEnum Type { get; set; }

        /// <summary>
        /// 计划类型
        /// </summary>
        public PlanWorkPlanTypeEnum PlanType { get; set; } = PlanWorkPlanTypeEnum.Rotor;

        /// <summary>
        /// 产线编码 
        /// </summary>
        public string LineCode { get; set; } = "";

        /// <summary>
        /// 需求单号
        /// </summary>
        public string? RequirementNumber { get; set; }

        /// <summary>
        /// 主键Id
        /// </summary>
        public long? Id { get; set; }

        /// <summary>
        /// 计划单号 
        /// </summary>
        public string PlanCode { get; set; } = "";

        /// <summary>
        /// 产品明细
        /// </summary>
        public List<SyncWorkPlanProductDto> Products { get; set; } = new();

    }

    /// <summary>
    /// 工作计划 Dto
    /// </summary>
    public record SyncWorkPlanProductDto : BaseEntityDto
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        public long? Id { get; set; }

        /// <summary>
        /// 产品编号 
        /// </summary>
        public string ProductCode { get; set; } = "";

        /// <summary>
        /// BomId
        /// </summary>
        public long BomId { get; set; }

        /// <summary>
        /// Bom编码
        /// </summary>
        public string BomCode { get; set; } = "";

        /// <summary>
        /// Bom版本
        /// </summary>
        public string BomVersion { get; set; } = "";

        /// <summary>
        /// 生产数量
        /// </summary>
        public decimal Qty { get; set; }

        /// <summary>
        /// 生产时间（计划）
        /// </summary>
        public DateTime? StartTime { get; set; }

        /// <summary>
        /// 结束时间（计划）
        /// </summary>
        public DateTime? EndTime { get; set; }

        /// <summary>
        /// 超产比例
        /// </summary>
        public decimal OverScale { get; set; }

        /// <summary>
        /// 物料明细
        /// </summary>
        public List<SyncWorkPlanMaterialDto> Materials { get; set; } = new();

    }

    /// <summary>
    /// 工作计划 Dto
    /// </summary>
    public record SyncWorkPlanMaterialDto
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        public long? Id { get; set; }

        /// <summary>
        /// 物料Id
        /// </summary>
        public long MaterialId { get; set; }

        /// <summary>
        /// 物料编号 
        /// </summary>
        public string MaterialCode { get; set; } = "";

        /// <summary>
        /// 物料版本
        /// </summary>
        public string MaterialVersion { get; set; } = "";

        /// <summary>
        /// BomId
        /// </summary>
        public long? BomId { get; set; }

        /// <summary>
        /// 用量
        /// </summary>
        public decimal MaterialDosage { get; set; }

        /// <summary>
        /// 损耗
        /// </summary>
        public decimal MaterialLoss { get; set; }

        /// <summary>
        /// 计划BomId（这里有可能是空，有学问）
        /// </summary>
        public long? PlanBomId { get; set; }

        /*
        /// <summary>
        /// 物料的替代料列表
        /// </summary>
        public List<SyncWorkPlanMaterialDto>? ReplaceMaterials { get; set; } = new();
        */

    }

}
