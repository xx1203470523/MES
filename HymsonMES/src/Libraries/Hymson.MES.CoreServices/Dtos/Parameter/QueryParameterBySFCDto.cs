namespace Hymson.MES.CoreServices.Dtos.Parameter
{
    /// <summary>
    /// 更具sfcs查询
    /// </summary>
    public  class QueryParameterBySfcDto
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 条码集合
        /// </summary>
        public IEnumerable<string> SFCs { get; set; } = new List<string>();
    }
}
