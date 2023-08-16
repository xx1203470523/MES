using Hymson.MES.Core.Enums;

namespace Hymson.MES.CoreServices.Bos.Manufacture
{
    /// <summary>
    /// 
    /// </summary>
    public class BomMaterialBo
    {
        /// <summary>
        /// 物料ID
        /// </summary>
        public long MaterialId { get; set; }

        /// <summary>
        /// 工序ID
        /// </summary>
        public long ProcedureId { get; set; }

        /// <summary>
        /// 是否启用物料维护的替代料
        /// </summary>
        public bool IsEnableReplace { get; set; }

        /// <summary>
        /// 数据收集方式 
        /// </summary>
        public MaterialSerialNumberEnum? DataCollectionWay { get; set; }

        /// <summary>
        /// 替代料集合（来自BOM）
        /// </summary>
        public IEnumerable<BomMaterialReplaceBo> BomMaterials { get; set; } = new List<BomMaterialReplaceBo>();

        /// <summary>
        /// 替代料集合（来自物料维护）
        /// </summary>
        public IEnumerable<BomMaterialReplaceBo> ProcMaterials { get; set; } = new List<BomMaterialReplaceBo>();

    }

    /// <summary>
    /// 
    /// </summary>
    public class BomMaterialReplaceBo
    {
        /// <summary>
        /// 物料ID
        /// </summary>
        public long MaterialId { get; set; }

        /// <summary>
        /// 数据收集方式 
        /// </summary>
        public MaterialSerialNumberEnum? DataCollectionWay { get; set; }

    }
}
