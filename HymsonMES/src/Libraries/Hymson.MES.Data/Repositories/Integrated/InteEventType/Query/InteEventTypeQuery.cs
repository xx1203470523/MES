namespace Hymson.MES.Data.Repositories.Integrated.Query
{
    /// <summary>
    /// 事件维护 查询参数
    /// </summary>
    public class InteEventTypeQuery
    {
        /// <summary>
        /// 站点ID
        /// </summary>
        public long SiteId { get; set; }
        public string? EventTypeCode { get; set; }
    }
}
