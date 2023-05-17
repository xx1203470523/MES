using Hymson.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace  Hymson.MES.EquipmentServices.Dtos.BindContainer
{
    /// <summary>
    /// 容器绑定
    /// </summary>
    public record BindContainerDto : BaseDto
    {
        /// <summary>
        /// 容器编码
        /// </summary>
        public string ContainerCode { get; set; } = string.Empty;

        /// <summary>
        /// 绑定的电芯条码列表
        /// </summary>
        public ContainerSFCDto[] ContainerSFCs { get; set; } = Array.Empty<ContainerSFCDto>();
    }

}
