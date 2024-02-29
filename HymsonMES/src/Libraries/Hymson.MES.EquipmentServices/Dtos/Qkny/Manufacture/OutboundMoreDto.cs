using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.EquipmentServices.Dtos.Qkny.Manufacture
{
    /// <summary>
    /// 出站多个
    /// </summary>
    public record OutboundMoreDto : QknyBaseDto
    {
        /// <summary>
        /// 出站产品条码列表
        /// </summary>
        public List<OutboundDto> SfcList { get; set; } = new List<OutboundDto>();
    }
}
