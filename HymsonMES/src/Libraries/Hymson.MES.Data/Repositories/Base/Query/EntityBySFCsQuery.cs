namespace Hymson.MES.Data.Repositories.Common.Query
{
    /// <summary>
    /// SFC查询实体
    /// </summary>
    public class EntityBySFCQuery : EntityBySiteIdQuery
    {
        /// <summary>
        /// 条码
        /// </summary>
        public string SFC { get; set; }

    }

    /// <summary>
    /// SFC查询实体
    /// </summary>
    public class EntityBySFCsQuery : EntityBySiteIdQuery
    {
        /// <summary>
        /// 条码
        /// </summary>
        public IEnumerable<string> SFCs { get; set; }

    }
}
