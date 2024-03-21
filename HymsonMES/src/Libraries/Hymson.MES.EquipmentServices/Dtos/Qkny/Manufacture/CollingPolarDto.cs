using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.EquipmentServices.Dtos.Qkny.Manufacture
{
    /// <summary>
    /// 卷绕极组产出
    /// </summary>
    public record CollingPolarDto : QknyBaseDto
    {
        /// <summary>
        /// 条码
        /// </summary>
        public string Sfc { get; set; } = string.Empty;
    }
}
