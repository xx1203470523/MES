using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.EquipmentServices.Dtos.Qkny.Common
{
    /// <summary>
    /// 设备状态
    /// </summary>
    public record StateDto : QknyBaseDto
    {
        /// <summary>
        /// 状态代码 1-运行;2-故障;3-待机;
        /// </summary>
        public string StateCode { get; set; } = "";

        /// <summary>
        /// 停机原因
        /// </summary>
        public string DownReason { get; set; } = "";
    }
}
