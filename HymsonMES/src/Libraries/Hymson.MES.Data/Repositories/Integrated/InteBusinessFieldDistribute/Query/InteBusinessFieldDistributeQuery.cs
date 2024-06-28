namespace Hymson.MES.Data.Repositories.Integrated.Query
{
    /// <summary>
    /// 字段分配管理 查询参数
    /// </summary>
    public class InteBusinessFieldDistributeQuery
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
