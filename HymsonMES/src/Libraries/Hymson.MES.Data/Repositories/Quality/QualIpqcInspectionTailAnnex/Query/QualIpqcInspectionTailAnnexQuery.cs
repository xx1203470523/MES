namespace Hymson.MES.Data.Repositories.Quality.Query
{
    /// <summary>
    /// 尾检附件 查询参数
    /// </summary>
    public class QualIpqcInspectionTailAnnexQuery
    {
        /// <summary>
        /// 检验单Id
        /// </summary>
        public long? InspectionOrderId { get; set; }
    }
}
