namespace Hymson.MES.Data.Repositories.Common.Query
{
    /// <summary>
    /// code查询实体
    /// </summary>
    public class EntityByCodesQuery
    {
        /// <summary>
        /// 工厂
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        public IEnumerable<string> Codes { get; set; }

    }
}
