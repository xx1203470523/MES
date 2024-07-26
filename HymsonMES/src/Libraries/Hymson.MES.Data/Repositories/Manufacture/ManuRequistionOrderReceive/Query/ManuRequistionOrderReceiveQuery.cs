namespace Hymson.MES.Data.Repositories.Manufacture.Query
{
    /// <summary>
    /// 生产领料单明细 查询参数
    /// </summary>
    public class ManuRequistionOrderReceiveQuery
    {
        /// <summary>
        /// 物料条码
        /// </summary>
        public List<string> MaterialBarCodeList { get; set; }

        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }
    }
}
