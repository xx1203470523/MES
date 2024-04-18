using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.EquipmentServices.Dtos.Qkny.Manufacture
{
    /// <summary>
    /// 请求产出极卷码
    /// </summary>
    public record GenerateSfcDto: QknyBaseDto
    {
        /// <summary>
        /// 产出数量
        /// </summary>
        public int Qty { get; set; }
    }
}
