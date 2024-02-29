using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.EquipmentServices.Dtos.Qkny.Manufacture
{

    /// <summary>
    /// 空托盘校验
    /// </summary>
    public record EmptyContainerCheckDto : QknyBaseDto
    {
        /// <summary>
        /// 托盘
        /// </summary>
        public string ContainerCode { get; set; } = "";
    }
}
