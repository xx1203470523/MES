namespace Hymson.MES.Data.Repositories.Common.Query
{
    /// <summary>
    /// 查询实体
    /// </summary>
    public class EntityByParentIdQuery
    {
        /// <summary>
        /// 工厂
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 父ID
        /// </summary>
        public long? ParentId { get; set; }

    }
}
