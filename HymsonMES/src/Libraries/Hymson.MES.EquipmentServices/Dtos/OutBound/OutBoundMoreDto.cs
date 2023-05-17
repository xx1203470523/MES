using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.EquipmentServices.Dtos.OutBound
{
    /// <summary>
    /// 出站（多个）
    /// </summary>
    public record OutBoundMoreDto
    {
        /// <summary>
        /// 产品条码集合
        /// </summary>
        public OutBound[]? SFCs { get; set; }
    }
}
