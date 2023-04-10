namespace Hymson.MES.Services.Dtos.Manufacture.ManuMainstreamProcessDto
{
    /// <summary>
    /// 作业Dto
    /// </summary>
    public class SFCWorkDto
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
    }

    /// <summary>
    /// 扣料Dto
    /// </summary>
    public class MaterialDeductDto
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
