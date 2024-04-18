using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.EquipmentServices.Dtos.Qkny.Manufacture
{
    /// <summary>
    /// 出站多个返回
    /// </summary>
    public record OutboundMoreReturnDto : QknyReturnBaseDto
    {
        /// <summary>
        /// 条码
        /// </summary>
        public string Sfc { get; set; } = "";
    }
}
