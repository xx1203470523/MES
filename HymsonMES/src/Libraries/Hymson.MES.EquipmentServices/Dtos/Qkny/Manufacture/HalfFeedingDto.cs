using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.EquipmentServices.Dtos.Qkny.Manufacture
{
    /// <summary>
    /// 半成品上料
    /// </summary>
    public record HalfFeedingDto : QknyBaseDto
    {
        /// <summary>
        /// 上料条码
        /// </summary>
        public string Sfc { get; set; } = "";
    }
}
