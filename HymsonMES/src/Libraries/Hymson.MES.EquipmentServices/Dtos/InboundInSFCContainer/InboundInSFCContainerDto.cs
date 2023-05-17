using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.EquipmentServices.Dtos.InboundInSFCContainer
{
    /// <summary>
    /// 进站-电芯和托盘-装盘2
    /// </summary>
    public record InboundInSFCContainerDto : BaseDto
    {
        /// <summary>
        /// 托盘码
        /// </summary>
        public string ContainerCode { get; set; } = string.Empty;

        /// <summary>
        /// 产品条码
        /// </summary>
        public string SFC { get; set; } = string.Empty;

    }
}
