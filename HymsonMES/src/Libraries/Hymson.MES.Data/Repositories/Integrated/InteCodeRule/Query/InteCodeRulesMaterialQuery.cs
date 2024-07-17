namespace Hymson.MES.Data.Repositories.Integrated
{
    /// <summary>
    /// 编码规则物料 查询参数
    /// </summary>
    public class InteCodeRulesMaterialQuery
    {
        /// <summary>
        /// 排序(默认为 CreatedOn DESC)
        /// </summary>
        public string Sorting { get; set; } = "CreatedOn DESC";

        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 编码规则id
        /// </summary>
        public long? CodeRulesId { get; set; }

        /// <summary>
        /// 物料Id
        /// </summary>
        public long? MaterialId { get; set; }

        /// <summary>
        /// 物料Id
        /// </summary>
        public IEnumerable<long>? MaterialIds { get; set; }
    }
}
