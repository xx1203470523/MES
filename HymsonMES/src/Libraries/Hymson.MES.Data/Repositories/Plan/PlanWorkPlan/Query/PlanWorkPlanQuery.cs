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
    public class PlanWorkPlanByPlanIdQuery
    {
        /// <summary>
        /// 站点id
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 计划Id
        /// </summary>
        public long PlanId { get; set; }
        /// <summary>
        /// 产品Id
        /// </summary>
        public long PlanProductId { get; set; }
    }
}
