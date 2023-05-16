using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.EquipmentServices.Request.UnBindContainer
{
    /// <summary>
    /// 容器解绑
    /// </summary>
    public class UnBindContainerRequest : BaseRequest
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
