using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Data.Options
{
    /// <summary>
    /// 参数
    /// </summary>
    public class ParameterOptions
    {
        /// <summary>
        /// 参数取模
        /// </summary>
        public int ParameterDelivery { get; set; } = 2048;

        /// <summary>
        /// 设备参数取模
        /// </summary>
        public int EquipmentParameterDelivery { get; set; } = 2048;
    }
}
