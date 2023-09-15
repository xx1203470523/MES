using Hymson.Infrastructure;

namespace Hymson.MES.Data.Repositories.Equipment
{
    /// <summary>
    /// OEE查询对象
    /// </summary>
    public class EquOeeReportPagedQuery : PagerInfo
    {
        /// <summary>
        /// 站点id
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 设备编码
        /// </summary>
        public string[]? EquipmentCodes { get; set; }

        /// <summary>
        /// 1白班，0夜班
        /// </summary>
        public int DayShift { get; set; }

        /// <summary>
        /// 查询时间
        /// </summary>
        public DateTime[] QueryTime { get; set; }
    }
}