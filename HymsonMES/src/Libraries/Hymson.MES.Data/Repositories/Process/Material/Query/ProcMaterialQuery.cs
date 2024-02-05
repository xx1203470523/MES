namespace Hymson.MES.Data.Repositories.Process
{
    /// <summary>
    /// 物料维护 查询参数
    /// </summary>
    public partial class ProcMaterialQuery
    {
        /// <summary>
        /// 站点编码 
        /// </summary>
        public long? SiteId { get; set; }

        /// <summary>
        /// 物料编码
        /// </summary>
        public string? MaterialCode { get; set; }

        /// <summary>
        /// 版本
        /// </summary>
        public string? Version { get; set; }
    }
}
