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
    /// 出站
    /// </summary>
    public interface IOutBoundService
    {
        /// <summary>
        /// 出站
        /// </summary>
        /// <param name="outBoundRequest"></param>
        /// <returns></returns>
        Task OutBound(OutBoundRequest outBoundRequest);

        /// <summary>
        /// 进站（多个）
        /// </summary>
        /// <param name="outBoundMoreRequest"></param>
        /// <returns></returns>
        Task OutBoundMore(OutBoundMoreRequest outBoundMoreRequest);
    }
}
