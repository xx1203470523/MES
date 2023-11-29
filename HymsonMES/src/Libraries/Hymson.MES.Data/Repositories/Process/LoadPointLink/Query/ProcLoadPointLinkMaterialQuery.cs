namespace Hymson.MES.Data.Repositories.Process.LoadPointLink.Query
{
    /// <summary>
    /// 上料点关联物料表 查询参数
    /// </summary>
    public class ProcLoadPointLinkMaterialQuery
    {
        /// <summary>
        /// 站点ID
        /// </summary>
        public long? SiteId { get; set; }

        /// <summary>
        /// 上料点ID
        /// </summary>
        public long? LoadPointId { get; set; }
    }
}
