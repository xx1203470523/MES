using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.EquipmentServices.Dtos.Qkny.Manufacture
{
    /// <summary>
    /// 容器出站
    /// </summary>
    public record OutboundInContainerDto : QknyBaseDto
    {
        /// <summary>
        /// 容器
        /// </summary>
        public string ContainerCode { get; set; } = "";

        /// <summary>
        /// 参数列表
        /// </summary>
        public List<QknyParamBaseDto> ParamList { get; set; } = new List<QknyParamBaseDto>();
    }
}
