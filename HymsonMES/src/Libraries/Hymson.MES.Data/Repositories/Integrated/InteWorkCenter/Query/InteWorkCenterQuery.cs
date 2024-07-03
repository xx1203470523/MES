namespace Hymson.MES.Data.Repositories.Integrated.Query
{
    /// <summary>
    /// 
    /// </summary>
    public class InteWorkCenterQuery
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 工作中心编码列表
        /// </summary>
        public IEnumerable<string>? Codes { get; set; }

    }

    /// <summary>
    /// 
    /// </summary>
    public class InteWorkCenterOneQuery
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 工作中心编码列表
        /// </summary>
        public string? Code { get; set; }

        /// <summary>
        /// 工作中心名称列表
        /// </summary>
        public string? Name { get; set; }

    }

}
