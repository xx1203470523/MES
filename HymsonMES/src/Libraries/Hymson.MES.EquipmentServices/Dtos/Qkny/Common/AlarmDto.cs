using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.EquipmentServices.Dtos.Qkny.Common
{
    /// <summary>
    /// 设备报警
    /// </summary>
    public record AlarmDto : QknyBaseDto
    {
        /// <summary>
        /// 0：恢复; 1：发生;
        /// </summary>
        public string Status { get; set; } = "";

        /// <summary>
        /// 报警详细信息
        /// </summary>
        public string AlarmMsg { get; set; } = "";

        /// <summary>
        /// 报警代码
        /// </summary>
        public string AlarmCode { get; set; } = "";

        /// <summary>
        /// L：提示不停机；M：提示停机；H故障停机
        /// </summary>
        public string AlarmLevel { get; set; } = "";
    }
}
