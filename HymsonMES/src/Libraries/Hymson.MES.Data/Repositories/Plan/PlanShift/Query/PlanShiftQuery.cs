namespace Hymson.MES.Data.Repositories.Plan.Query
{
    /// <summary>
    /// 班制 查询参数
    /// </summary>
    public class PlanShiftQuery
    {
        public long SiteId { get; set; }
        /// <summary>
        /// 班制编码
        /// </summary>
        public string Code { get; set; }
    }
}
