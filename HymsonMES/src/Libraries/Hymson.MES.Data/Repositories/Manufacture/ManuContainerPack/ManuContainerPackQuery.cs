namespace Hymson.MES.Data.Repositories.Manufacture
{
    /// <summary>
    /// 容器装载表（物理删除） 查询参数
    /// </summary>
    public class ManuContainerPackQuery
    {
        /// <summary>
        /// 条码
        /// </summary>
        public string? LadeBarCode { get; set; }

        /// <summary>
        /// 条码s
        /// </summary>
        public IEnumerable<string>  LadeBarCodes { get; set; }

        /// <summary>
        /// 站点id
        /// </summary>
        public long SiteId { get; set; }
    }
}
