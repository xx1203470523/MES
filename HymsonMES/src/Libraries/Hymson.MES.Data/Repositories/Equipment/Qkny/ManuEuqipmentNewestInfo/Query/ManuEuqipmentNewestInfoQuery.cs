namespace Hymson.MES.Data.Repositories.ManuEuqipmentNewestInfo.Query
{
    /// <summary>
    /// 设备最新信息 查询参数
    /// </summary>
    public class ManuEuqipmentNewestInfoQuery
    {
        /// <summary>
        /// 设备id
        /// </summary>
        public long EquipmentId { get; set; }
    }

    /// <summary>
    /// 设备最新信息站点查询
    /// </summary>
    public class ManuEuqipmentNewestInfoSiteQuery
    {
        /// <summary>
        /// 设备id
        /// </summary>
        public long SiteId { get; set; }
    }
}
