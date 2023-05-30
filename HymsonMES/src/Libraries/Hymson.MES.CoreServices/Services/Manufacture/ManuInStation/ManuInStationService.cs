using Hymson.MES.CoreServices.Dtos.Manufacture.ManuInStation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.CoreServices.Services.Manufacture.ManuInStation
{
    /// <summary>
    /// 进站
    /// @author wangkeming
    /// @date 2023-05-25
    /// </summary>
    public class ManuInStationService : IManuInStationService
    {
        public ManuInStationService()
        {

        }

        /// <summary>
        /// 根据设备进站
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task InStationByEquipment(EquipmentInStationDto param)
        {

        }   

        /// <summary>
        /// 根据设备批量进站
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task InStationRangeByEquipment(EquipmentInStationDto param)
        {

        }

        /// <summary>
        /// 根据资源进站
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task InStationByResource(ResourceInStationDto param)
        {

        }

        /// <summary>
        /// 根据资源批量进站
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task InStationRangeByResource(ResourceInStationsDto param)
        {

        }
    }
}
