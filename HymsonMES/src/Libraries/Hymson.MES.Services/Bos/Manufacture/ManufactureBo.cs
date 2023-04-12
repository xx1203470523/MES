namespace Hymson.MES.Services.Bos.Manufacture
{
    /// <summary>
    /// 
    /// </summary>
    public class ManufactureBo
    {
        /// <summary>
        /// 工序ID
        /// </summary>
        public long ProcedureId { get; set; }
        /// <summary>
        /// 资源ID
        /// </summary>
        public long ResourceId { get; set; }
        /// <summary>
        /// 产品条码
        /// </summary>
        public string SFC { get; set; }

        /*
        /// <summary>
        /// 额外参数
        /// </summary>
        public string Extra { get; set; }
        */
    }

    /// <summary>
    /// 扣料
    /// </summary>
    public class MaterialDeductBo
    {
        /// <summary>
        /// 物料ID
        /// </summary>
        public long MaterialId { get; set; }

        /// <summary>
        /// 数量（扣减）
        /// </summary>
        public decimal Qty { get; set; }

        /// <summary>
        /// 替代料集合（BOM）
        /// </summary>
        public IEnumerable<MaterialDeductItemBo> ReplaceMaterial { get; set; }

        /// <summary>
        /// 替代料集合（物料）
        /// </summary>
        public IEnumerable<MaterialDeductItemBo> ReplaceBoms { get; set; }
    }

    /// <summary>
    /// 扣料项
    /// </summary>
    public class MaterialDeductItemBo
    {
        /// <summary>
        /// 物料ID
        /// </summary>
        public long MaterialId { get; set; }
        /// <summary>
        /// 数量（扣减）
        /// </summary>
        public decimal Qty { get; set; }
    }
}
