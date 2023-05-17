using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.EquipmentServices.Dtos.InboundInContainer
{
    /// <summary>
    /// 进站-容器
    /// </summary>
    public record InboundInContainerDto : BaseDto
    {
        /// <summary>
        /// 托盘码
        /// </summary>
        public string ContainerCode { get; set; } = string.Empty;
    }
}
