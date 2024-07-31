using Hymson.Infrastructure;

namespace Hymson.MES.Services.Dtos.Report
{
    /// <summary>
    /// 物料追溯DTO
    /// </summary>
    public  record MaterialSourceDto:BaseEntityDto
    {
        /// <summary>
        /// 产品序列码
        /// </summary>
        public string Sfc { get; set; }

        /// <summary>
        /// 批次条码
        /// </summary>
        public string CirculationBarCode { get; set; }

        /// <summary>
        /// 扣料数量
        /// </summary>
        public decimal CirculationQty { get; set; }

        /// <summary>
        /// 工单
        /// </summary>
        public string WorkOrder { get; set; }

        /// <summary>
        /// 物料编码/产品
        /// </summary>
        public string MaterialRemark { get; set; }

        /// <summary>
        /// 需求用量
        /// </summary>
        public decimal BomUsages { get; set; }

        /// <summary>
        /// 扣料工序
        /// </summary>
        public string ProcedureName { get; set; }

        /// <summary>
        /// 扣料工序编码
        /// </summary>
        public string ProcedureCode { get; set; }

        /// <summary>
        /// 物料编码
        /// </summary>
        public string MaterialCode { get; set; }

        /// <summary>
        /// 物料名称
        /// </summary>
        public string MaterialName { get; set; }
    }
}
