using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.EquipmentServices.Dtos.OutBound
{
    /// <summary>
    /// 出站
    /// </summary>
    public record OutBoundDto
    {
        /// <summary>
        /// 产品条码
        /// </summary>
        public OutBound? SFC { get; set; }

    }

}
