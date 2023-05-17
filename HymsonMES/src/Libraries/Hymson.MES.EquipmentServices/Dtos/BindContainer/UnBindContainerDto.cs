using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.EquipmentServices.Dtos.BindContainer
{
    /// <summary>
    /// 容器解绑
    /// </summary>
    public record UnBindContainerDto : BaseDto
    {
        /// <summary>
        /// 容器编码
        /// </summary>
        public string ContainerCode { get; set; } = string.Empty;

        /// <summary>
        /// 绑定的SFC数组
        /// </summary>
        public string[] ContainerSFCs { get; set; } = Array.Empty<string>();
    }
}
