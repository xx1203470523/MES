namespace Hymson.MES.Data.Repositories.Quality.Query
{
    /// <summary>
    /// 首检检验单样本 查询参数
    /// </summary>
    public class QualIpqcInspectionHeadSampleQuery
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long? SiteId { get; set; }

        /// <summary>
        /// 检验单Id
        /// </summary>
        public long? InspectionOrderId { get; set; }
    }
}
