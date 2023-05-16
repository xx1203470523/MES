using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.EquipmentServices.Request.InBound
{
    /// <summary>
    /// 进站（多个）
    /// </summary>
    public class InBoundMoreRequest : BaseRequest
    {
        /// <summary>
        /// 进站条码
        /// </summary>
        public string[]? SFCs { get; set; }
    }
}
