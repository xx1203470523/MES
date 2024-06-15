namespace Hymson.MES.Data.Repositories.Integrated.Query
{
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

    public class InteWorkCenterFirstQuery   
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
