namespace Hymson.MES.Data.Repositories.Process.LoadPointLink.Query
{
    /// <summary>
    /// 上料点关联资源表 查询参数
    /// </summary>
    public class ProcLoadPointLinkResourceQuery
    {
        /// <summary>
        /// 站点ID
        /// </summary>
        public long? SiteId { get; set; }

        /// <summary>
        /// 上料点ID
        /// </summary>
        public long? LoadPointId {  get; set; }
    }

    /// <summary>
    /// 上料点关联资源表 查询参数
    /// </summary>
    public class ProcLoadPointCodeLinkResourceQuery
    {
        /// <summary>
        /// 上料点
        /// </summary>
        public string LoadPoint { get; set; }
    }
}
