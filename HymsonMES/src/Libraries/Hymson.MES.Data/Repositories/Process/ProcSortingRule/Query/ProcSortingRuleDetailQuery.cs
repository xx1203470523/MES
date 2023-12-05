namespace Hymson.MES.Data.Repositories.Process
{
    /// <summary>
    /// 分选规则详情 查询参数
    /// </summary>
    public class ProcSortingRuleDetailQuery
    {
        /// <summary>
        /// 站点id
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 分选规则id
        /// </summary>
        public long? SortingRuleId { get; set; }

        /// <summary>
        /// 分选规则id列表
        /// </summary>
        public long[]? SortingRuleIds { get; set; }
    }
}
