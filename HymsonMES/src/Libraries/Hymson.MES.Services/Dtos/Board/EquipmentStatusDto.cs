using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Services.Dtos.Board
{
    public class EquipmentStatusDto
    {
    }

    /// <summary>
    /// 设备最新状态查询
    /// </summary>
    public class EquipmentNewestInfoQueryDto
    {
        /// <summary>
        /// 站点id
        /// </summary>
        public long SiteId { get; set; } = 0;
    }
}
