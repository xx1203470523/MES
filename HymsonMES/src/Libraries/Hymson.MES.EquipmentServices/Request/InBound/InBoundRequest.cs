using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.EquipmentServices.Request.InBound
{
    /// <summary>
    /// 进站
    /// </summary>
    public class InBoundRequest : BaseRequest
    {
        /// <summary>
        /// 进站条码
        /// </summary>
        public string SFC { get; set; } = string.Empty;
    }
}
