namespace Hymson.MES.Data.Repositories.Manufacture.Query
{
    /// <summary>
    /// 查询实体
    /// </summary>
    public class LocationQuery
    {
        /// <summary>
        /// 工厂
        /// </summary>
        public long? SiteId { get; set; }

        /// <summary>
        /// 条码（产品序列码）
        /// </summary>
        public string SFC { get; set; }

        /// <summary>
        /// 位置号
        /// </summary>
        public string Location { get; set; }

    }
}
