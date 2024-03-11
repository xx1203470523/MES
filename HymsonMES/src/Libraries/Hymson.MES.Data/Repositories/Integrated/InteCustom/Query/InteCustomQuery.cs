namespace Hymson.MES.Data.Repositories.Integrated
{
    /// <summary>
    /// 客户维护 查询参数
    /// </summary>
    public class InteCustomQuery
    {
        /// <summary>
        /// 所属站点id
        /// </summary>
        public long? SiteId { get; set; } = 0;

        /// <summary>
        /// 客户编码列表
        /// </summary>
        public string[]? Codes { get; set; }

        /// <summary>
        /// 编号
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

    }
}
