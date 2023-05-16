using Hymson.MES.EquipmentServices.Request.InBound;
using Hymson.MES.EquipmentServices.Request.OutBound;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.EquipmentServices.Services.OutBound
{
    /// <summary>
    /// 出站（多个）
    /// </summary>
    public interface IOutBoundMoreService
    {
        /// <summary>
        /// 进站（多个）
        /// </summary>
        /// <param name="outBoundMoreRequest"></param>
        /// <returns></returns>
        Task OutBoundMore(OutBoundMoreRequest outBoundMoreRequest);
    }
}
