namespace Hymson.MES.Data.Repositories.Common.Query
{
    /// <summary>
    /// 查询实体
    /// </summary>
    public class EntityByLinkIdQuery
    {
        /// <summary>
        /// 工厂
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 关联ID
        /// </summary>
        public long LinkId { get; set; }

    }
}
