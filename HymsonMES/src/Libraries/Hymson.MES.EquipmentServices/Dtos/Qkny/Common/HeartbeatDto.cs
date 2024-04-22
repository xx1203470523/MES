using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.EquipmentServices.Dtos.Qkny.Common
{
    /// <summary>
    /// 设备心跳
    /// </summary>
    public record HeartbeatDto : QknyBaseDto
    {
        /// <summary>
        /// 设备是否在线 True：在线False：离线
        /// </summary>
        public bool IsOnline { get; set; } = false;
    }
}
