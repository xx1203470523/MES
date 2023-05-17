using Hymson.MES.EquipmentServices.Dtos.InBound;
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
        /// <param name="inBoundDto"></param>
        /// <returns></returns>
        Task InBound(InBoundDto inBoundDto);

        /// <summary>
        /// 进站（多个）
        /// </summary>
        /// <param name="inBoundMoreDto"></param>
        /// <returns></returns>
        Task InBoundMore(InBoundMoreDto inBoundMoreDto);
    }
}
