using Hymson.Infrastructure;
using Hymson.MES.Core.Enums.Integrated;
using System.Xml;

namespace Hymson.MES.SystemServices.Dtos
{
    /// <summary>
    /// 工单BOM
    /// </summary>
    public record SyncBomDto : BaseEntityDto
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        public long? Id { get; set; }

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
        public List<SyncBomMaterialDto> BomMaterials { get; set; } = new();

    }

    /// <summary>
    /// bom物料
    /// </summary>
    public class SyncBomMaterialDto
    {
        /// <summary>
        /// 发料方式
        /// </summary>
        public MaterialMethodEnum MaterialMethod { get; set; } = MaterialMethodEnum.Picking;

        /// <summary>
        /// 主键Id
        /// </summary>
        public long? Id { get; set; }

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
        public string? ProcedureCode { get; set; } = "";

        /// <summary>
        /// 物料用量
        /// </summary>
        public decimal MaterialDosage { get; set; }

        /// <summary>
        /// 物料损耗
        /// </summary>
        public decimal MaterialLoss { get; set; }

        /// <summary>
        /// 物料的替代料列表
        /// </summary>
        public List<SyncBomReplaceMaterialDto>? ReplaceMaterials { get; set; } = new();

    }

    /// <summary>
    /// bom替代物料
    /// </summary>
    public class SyncBomReplaceMaterialDto
    {
        /// <summary>
        /// 发料方式
        /// </summary>
        public MaterialMethodEnum MaterialMethod { get; set; } = MaterialMethodEnum.Picking;

        /// <summary>
        /// 主键Id
        /// </summary>
        public long? Id { get; set; }

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
        public string? ProcedureCode { get; set; } = "";

        /// <summary>
        /// 物料用量
        /// </summary>
        public decimal MaterialDosage { get; set; }

        /// <summary>
        /// 物料损耗
        /// </summary>
        public decimal MaterialLoss { get; set; }

        /// <summary>
        /// 替代次序
        /// </summary>
        public int Sort { get; set; }

        /// <summary>
        /// 替代比
        /// </summary>
        public decimal Proportion { get; set; }
        
        /// <summary>
        /// 生效日期
        /// </summary>
        public DateTime EffectTime { get; set; }

        /// <summary>
        /// 生效日期
        /// </summary>
        public DateTime ExpiredTime { get; set; }

    }
}
