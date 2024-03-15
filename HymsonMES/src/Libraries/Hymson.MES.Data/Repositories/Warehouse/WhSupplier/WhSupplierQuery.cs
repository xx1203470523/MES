namespace Hymson.MES.Data.Repositories.Warehouse
{
    /// <summary>
    /// 供应商 查询参数
    /// </summary>
    public class WhSupplierQuery
    {
        /// <summary>
        /// 站点
        /// </summary>
        public long? SiteId { get; set; }

        /// <summary>
        /// 供应商编号
        /// </summary>
        public string? Code { get; set; }

        /// <summary>
        /// 供应商名称
        /// </summary>
        public string? Name { get; set; }


    }
}
