namespace Hymson.MES.Services.BOs.Manufacture
{
    /// <summary>
    /// 
    /// </summary>
    public class ManufactureBO
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
    public class MaterialDeductBO
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
