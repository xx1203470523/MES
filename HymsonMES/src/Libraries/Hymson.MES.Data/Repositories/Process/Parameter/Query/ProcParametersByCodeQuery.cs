namespace Hymson.MES.Data.Repositories.Process.Query
{
    /// <summary>
    ///更具编码查询参数
    /// </summary>
    public  class ProcParametersByCodeQuery
    {
        /// <summary>
        /// 站点id
        /// </summary>
        public long? SiteId { get; set; }

        /// <summary>
        /// 编码集合
        /// </summary>
        public IEnumerable<string > Codes { get; set; }
    }
}
