using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Data.Repositories.Equipment
{
    public class EquHeartbeatQuery
    {
        /// <summary>
        /// 站点ID
        /// </summary>
        public long? SiteId { get; set; }

        /// <summary>
        /// 设备ID
        /// </summary>
        public long? EquipmentId { get; set; }

        /// <summary>
        /// 设备在线状态
        /// 0离线 1在线
        /// </summary>
        public bool? Status { get; set; }

        /// <summary>
        /// 最后在线开始时间
        /// </summary>

        public DateTime? LastOnLineTimeStart { get; set; }

        /// <summary>
        /// 最后在线结束时间
        /// </summary>
        public DateTime? LastOnLineTimeEnd { get; set; }
    }
}
