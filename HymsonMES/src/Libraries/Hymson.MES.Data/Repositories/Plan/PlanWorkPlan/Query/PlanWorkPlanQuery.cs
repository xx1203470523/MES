namespace Hymson.MES.Data.Repositories.Plan.Query
{
    /// <summary>
    /// 生产计划信息表 分页参数
    /// </summary>
    public class PlanWorkPlanQuery
    {
        /// <summary>
        /// 站点id
        /// </summary>
        public long? SiteId { get; set; }

        /// <summary>
        /// 编码集合
        /// </summary>
        public IEnumerable<string>? Codes { get; set; }
    }
}
