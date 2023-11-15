namespace Hymson.MES.Data.Repositories.Manufacture
{
    /// <summary>
    /// 产品不良录入 查询参数
    /// </summary>
    public class ManuProducNGRecordQuery
    {
        /// <summary>
        /// 站点id
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 不良记录ID
        /// </summary>
        public long BadRecordId { get; set; }

        /// <summary>
        /// 不合格代码Id
        /// </summary>
        public long UnqualifiedId { get; set; }
    }
}
