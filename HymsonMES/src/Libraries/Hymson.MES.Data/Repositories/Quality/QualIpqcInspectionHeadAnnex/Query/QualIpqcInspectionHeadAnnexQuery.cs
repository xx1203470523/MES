namespace Hymson.MES.Data.Repositories.Quality.Query
{
    /// <summary>
    /// 首检附件 查询参数
    /// </summary>
    public class QualIpqcInspectionHeadAnnexQuery
    {
        /// <summary>
        /// 检验单Id
        /// </summary>
        public long? InspectionOrderId { get; set; }
    }
}
