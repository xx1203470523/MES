using Hymson.MES.CoreServices.Dtos.Manufacture.ManuInStation;

namespace Hymson.MES.CoreServices.Services.Manufacture.ManuInStation
{
    /// <summary>
    /// 进站
    /// @author wangkeming
    /// @date 2023-05-25
    /// </summary>
    public class ManuInStationService : IManuInStationService
    {
        /// <summary>
        /// 
        /// </summary>
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
            await Task.CompletedTask;
        }

        /// <summary>
        /// 根据设备批量进站
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task InStationRangeByEquipment(EquipmentInStationDto param)
        {
            await Task.CompletedTask;
        }

        /// <summary>
        /// 根据资源进站
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task InStationByResource(ResourceInStationDto param)
        {
            await Task.CompletedTask;
        }

        /// <summary>
        /// 根据资源批量进站
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task InStationRangeByResource(ResourceInStationsDto param)
        {
            await Task.CompletedTask;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private async Task ExecuteInStation()
        {
            await Task.CompletedTask;
        }

    }
}
