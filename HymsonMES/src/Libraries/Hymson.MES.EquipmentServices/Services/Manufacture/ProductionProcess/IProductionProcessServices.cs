using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.EquipmentServices.Services.Manufacture.ProductionProcess
{
    /// <summary>
    /// 生产管控
    /// </summary>
    public  interface IProductionProcessServices
    {
        /// <summary>
        /// 条码
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task InStationAsync(InStationDto param);
    }
}
