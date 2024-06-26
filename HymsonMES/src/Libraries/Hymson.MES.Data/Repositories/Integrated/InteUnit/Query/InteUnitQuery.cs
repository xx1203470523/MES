namespace Hymson.MES.Data.Repositories.Integrated.Query
{
    /// <summary>
    /// 单位维护 查询参数
    /// </summary>
    public class InteUnitQuery
    {
        /// <summary>
        /// 所属站点id
        /// </summary>
        public long? SiteId { get; set; } = 0;

        /// <summary>
        /// 编码集合
        /// </summary>
        public IEnumerable<string> Codes { get; set; }

    }
}
