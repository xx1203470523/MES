using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.EquipmentServices.Dtos.Qkny.Manufacture
{
    /// <summary>
    /// 多极组出站
    /// </summary>
    public record OutboundMultPolarDto : QknyBaseDto
    {
        /// <summary>
        /// 出站产品条码列表
        /// </summary>
        public List<QknyOutboundBaseDto> SfcList { get; set; } = new List<QknyOutboundBaseDto>();
    }
}
