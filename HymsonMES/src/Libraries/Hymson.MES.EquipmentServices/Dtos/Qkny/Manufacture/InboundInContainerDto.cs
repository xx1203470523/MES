using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.EquipmentServices.Dtos.Qkny.Manufacture
{

    /// <summary>
    /// 容器进站
    /// </summary>
    public record InboundInContainerDto : QknyBaseDto
    {
        /// <summary>
        /// 托盘码
        /// </summary>
        public string ContainerCode { get; set; } = "";
    }
}
