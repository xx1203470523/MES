namespace Hymson.MES.Data.Repositories.Manufacture.ManuSfc.Query
{
    /// <summary>
    /// 条码表 查询参数
    /// </summary>
    public class ManuSfcQuery
    {
        /// <summary>
        /// 工厂Id
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 条码集合
        /// </summary>
        public IEnumerable<string> SFCs { get; set; } = new List<string>();
    }


    /// <summary>
    /// 根据SFC查询条码
    /// </summary>
    public class GetBySfcQuery
    {
        /// <summary>
        /// 站点编码 
        /// </summary>
        public long? SiteId { get; set; }

        /// <summary>
        /// 条码
        /// </summary>
        public string? SFC { get; set; }
    }
}
