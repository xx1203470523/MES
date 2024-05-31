using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Data.Repositories.Equipment
{
    public class EquStatusStatisticsQuery
    {
        /// <summary>
        /// 站点ID
        /// </summary>
        public long SiteId { get; set; }
        /// <summary>
        /// 设备ID
        /// </summary>
        public long[]? EquipmentIds { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime? StartTime { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime? EndTime { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class EquEquipmentTheoryQuery
    {
        /// <summary>
        /// 设备ID
        /// </summary>
        public string[]? EquipmentCodes { get; set; }

        /// <summary>
        /// 设备ID
        /// </summary>
        public string? EquipmentCode { get; set; }
    }
}