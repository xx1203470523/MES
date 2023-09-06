using Hymson.MES.EquipmentServices.Dtos.InBound;
using Hymson.MES.EquipmentServices.Dtos.SfcCirculation;
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
        Task InBoundAsync(InBoundDto inBoundDto);

        /// <summary>
        /// 进站（多个）
        /// </summary>
        /// <param name="inBoundMoreDto"></param>
        /// <returns></returns>
        Task InBoundMoreAsync(InBoundMoreDto inBoundMoreDto);

        /// <summary>
        /// 获取工单
        /// </summary>
        /// <param name="baseDto"></param>
        /// <returns></returns>
        Task<PlanWorkOrderDto> GetWorkOrderAsync(BaseDto baseDto);
    }
}
