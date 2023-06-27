using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.BackgroundServices.Dtos.EquHeartbeat
{
    public class EquipmentHeartbeatUpdateDto
    {
        /// <summary>
        /// 最后在线时间距离当前时间间隔多少秒后置为离线
        /// 不传递默认30s
        /// </summary>
        public int? IntervalSeconds { get; set; }
    }
}
