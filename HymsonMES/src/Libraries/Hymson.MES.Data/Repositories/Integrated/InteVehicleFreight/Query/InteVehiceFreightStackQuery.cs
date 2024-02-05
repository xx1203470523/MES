namespace Hymson.MES.Data.Repositories.Integrated
{
    /// <summary>
    /// 二维载具条码明细 查询参数
    /// </summary>
    public class InteVehiceFreightStackQuery
    {
        /// <summary>
        /// 载具Id
        /// </summary>
        public long? VehicleId { get; set; }
        /// <summary>
        /// 位置Id
        /// </summary>
        public long? LocationId { get; set; }

        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }
        /// <summary>
        /// 条码列表
        /// </summary>
        public IEnumerable<string> Sfcs { get; set; }
    }

    /// <summary>
    /// 二维载具条码明细 查询参数
    /// </summary>
    public class InteVehiceFreightStackQueryByLocation
    {
        /// <summary>
        /// 位置Id
        /// </summary>
        public long LocationId { get; set; }
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }
    }

    public class InteVehiceFreightStackBySfcQuery 
    { 
        public long SiteId { get; set; }

        public string BarCode { get; set; }
    }
}
