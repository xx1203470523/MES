namespace Hymson.MES.Data.Repositories.Integrated
{
    /// <summary>
    /// 载具装载 查询参数
    /// </summary>
    public class InteVehicleFreightQuery
    {
        /// <summary>
        /// 条码列表
        /// </summary>
        public IEnumerable<string> Sfcs { get; set; }
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }
    }
}
