namespace Hymson.MES.Data.Repositories.Manufacture.Query
{
    /// <summary>
    /// 条码关系表 查询参数
    /// </summary>
    public class ManuBarcodeRelationQuery
    {
        /// <summary>
        /// 产出步骤表
        /// </summary>
        public long? OutputSfcStepId { get; set; }

        /// <summary>
        /// 投入条码表
        /// </summary>
        public long? InputSfcStepId { get; set; }

        /// <summary>
        /// 工厂
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 投入条码表
        /// </summary>
        public long? DisassembledSfcStepId { get; set; }
    }
}
