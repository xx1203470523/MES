namespace Hymson.MES.Data.Repositories.Process
{
    /// <summary>
    /// 物料替代组件表 查询参数
    /// </summary>
    public class ProcReplaceMaterialQuery
    {
        public long SiteId { get; set; }

        public long MaterialId { get; set; }

    }

    /// <summary>
    /// 物料替代组件表 查询参数
    /// </summary>
    public class ProcReplaceMaterialsQuery
    {
        /// <summary>
        /// 
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<long> MaterialIds { get; set; }
    }
}
