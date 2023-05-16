using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.EquipmentServices.Request.BindContainer
{
    /// <summary>
    /// 容器SFC
    /// </summary>
    public class ContainerSFC
    {
        /// <summary>
        /// 绑定条码
        /// </summary>
        public string SFC { get; set; } = string.Empty;

        /// <summary>
        /// 绑定位置
        /// </summary>
        public int Location { get; set; }
    }
}
