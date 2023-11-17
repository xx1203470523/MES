namespace Hymson.MES.CoreServices.Bos.Manufacture
{
    /// <summary>
    /// 条码载具
    /// </summary>
    public class VehicleSFCRequestBo
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 载具条码
        /// </summary>
        public IEnumerable<string> VehicleCodes { get; set; }
    }

    /// <summary>
    /// 条码载具
    /// </summary>
    public class VehicleSFCResponseBo
    {
        /// <summary>
        /// 条码
        /// </summary>
        public string SFC { get; set; }

        /// <summary>
        /// 载具条码
        /// </summary>
        public string VehicleCode { get; set; }
    }

}
