using Hymson.Infrastructure;
using Hymson.MES.Core.Enums.Integrated;

namespace Hymson.MES.SystemServices.Dtos
{
    /// <summary>
    /// 工单BOM
    /// </summary>
    public record BomDto : BaseEntityDto
    {
        /// <summary>
        /// Bom编码
        /// </summary>
        public string BomCode { get; set; } = "";

        /// <summary>
        /// Bom名称
        /// </summary>
        public string BomName { get; set; } = "";

        /// <summary>
        /// Bom版本
        /// </summary>
        public string BomVersion { get; set; } = "";

        /// <summary>
        /// Bom物料清单
        /// </summary>
        public List<BomMaterialDto> BomMaterials { get; set; } = new();

    }

    /// <summary>
    /// bom物料
    /// </summary>
    public class BomMaterialDto
    {
        /// <summary>
        /// 发料方式
        /// </summary>
        public MaterialMethodEnum MaterialMethod { get; set; }

        /// <summary>
        /// 产出工序
        /// </summary>
        public string? OutPutProcedure { get; set; }

        /// <summary>
        /// 物料编码
        /// </summary>
        public string MaterialCode { get; set; } = "";

        /// <summary>
        /// 物料版本
        /// </summary>
        public string MaterialVersion { get; set; } = "";

        /// <summary>
        /// 工序编码
        /// </summary>
        public string OperationCode { get; set; } = "";

        /// <summary>
        /// 物料用量
        /// </summary>
        public decimal MaterialDosage { get; set; }

        /// <summary>
        /// 物料损耗
        /// </summary>
        public decimal MaterialLoss { get; set; }

    }
}
