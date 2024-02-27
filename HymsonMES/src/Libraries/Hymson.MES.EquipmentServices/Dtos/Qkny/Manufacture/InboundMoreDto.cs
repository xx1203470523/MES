using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.EquipmentServices.Dtos.Qkny.Manufacture
{
    /// <summary>
    /// 进站多个
    /// </summary>
    public record InboundMoreDto : QknyBaseDto
    {
        /// <summary>
        /// 条码列表
        /// </summary>
        public List<string> SfcList { get; set; } = new List<string>();
    }
}
