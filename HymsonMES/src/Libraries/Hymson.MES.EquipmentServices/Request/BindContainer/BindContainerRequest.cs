using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.EquipmentServices.Request.BindContainer
{
    /// <summary>
    /// 容器绑定
    /// </summary>
    public class BindContainerRequest : BaseRequest
    {
        /// <summary>
        /// 容器编码
        /// </summary>
        public string ContainerCode { get; set; } = string.Empty;

        /// <summary>
        /// 绑定的电芯条码列表
        /// </summary>
        public ContainerSFC[] ContainerSFCs { get; set; } = Array.Empty<ContainerSFC>();
    }

}
