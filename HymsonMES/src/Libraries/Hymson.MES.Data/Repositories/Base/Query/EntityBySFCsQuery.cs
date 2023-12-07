namespace Hymson.MES.Data.Repositories.Common.Query
{
    /// <summary>
    /// 查询实体
    /// </summary>
    public class EntityBySFCsQuery
    {
        /// <summary>
        /// 工厂
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 产品序列码
        /// </summary>
        public IEnumerable<string> SFCs { get; set; }

    }
}
