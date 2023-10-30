namespace Hymson.MES.Data.Repositories.Common.Query
{
    /// <summary>
    /// 查询实体
    /// </summary>
    public class EntityByParentIdsQuery
    {
        /// <summary>
        /// 工厂
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 父ID
        /// </summary>
        public IEnumerable<long> ParentIds { get; set; }

    }
}
