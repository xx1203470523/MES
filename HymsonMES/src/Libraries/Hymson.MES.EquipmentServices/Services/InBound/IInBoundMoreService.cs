using Hymson.MES.EquipmentServices.Request.InBound;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.EquipmentServices.Services.InBound
{
    /// <summary>
    /// 进站（多个）
    /// </summary>
    public interface IInBoundMoreService
    {
        /// <summary>
        /// 进站（多个）
        /// </summary>
        /// <param name="inBoundMoreRequest"></param>
        /// <returns></returns>
        Task InBoundMore(InBoundMoreRequest inBoundMoreRequest);
    }
}
