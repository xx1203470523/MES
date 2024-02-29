using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.EquipmentServices.Dtos.Qkny.Manufacture
{
    /// <summary>
    /// 电芯极组出站
    /// </summary>
    public record OutboundSfcPolarDto : OutboundDto
    {
        /// <summary>
        /// 极组条码
        /// </summary>
        public string JzSfc { get; set; } = "";
    }
}
