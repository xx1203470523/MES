namespace Hymson.MES.Data.Repositories.ManuJzBind.Query
{
    /// <summary>
    /// 极组绑定 查询参数
    /// </summary>
    public class ManuJzBindQuery
    {
        /// <summary>
        /// 极组条码
        /// </summary>
        public string JzSfc { get; set; }

        /// <summary>
        /// 站点ID
        /// </summary>
        public long SiteId { get; set; }
    }
}
