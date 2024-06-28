namespace Hymson.MES.Data.Repositories.Integrated.Query
{
    /// <summary>
    /// 字段定义 查询参数
    /// </summary>
    public class InteBusinessFieldQuery
    {
        /// <summary>
        /// 编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 工厂id
        /// </summary>
        public long SiteId { get; set; }
    }
}
