using Hymson.MES.EquipmentServices.Request.InBound;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.EquipmentServices.Services.InBound
{
    /// <summary>
    /// 进站
    /// </summary>
    public interface IInBoundService
    {
        /// <summary>
        /// 进站
        /// </summary>
        /// <param name="inBoundRequest"></param>
        /// <returns></returns>
        Task InBound(InBoundRequest inBoundRequest);

        /// <summary>
        /// 进站（多个）
        /// </summary>
        /// <param name="inBoundMoreRequest"></param>
        /// <returns></returns>
        Task InBoundMore(InBoundMoreRequest inBoundMoreRequest);
    }
}
