namespace Hymson.MES.Data.Repositories.Integrated.Query
{
    /// <summary>
    /// 作业配置 查询参数
    /// </summary>
    public class InteJobConfigQuery
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 作业Id
        /// </summary>
        public long? JobId { get; set; }
    }
}
