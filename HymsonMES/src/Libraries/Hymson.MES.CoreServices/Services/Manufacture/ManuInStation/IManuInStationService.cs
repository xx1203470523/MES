using Hymson.MES.CoreServices.Dtos.Manufacture.ManuInStation;

namespace Hymson.MES.CoreServices.Services.Manufacture.ManuInStation
{
    /// <summary>
    /// 进站接口
    /// @author wangkeming
    /// @date 2023-05-25
    /// </summary>
    public interface IManuInStationService
    {
        /// <summary>
        /// 根据设备进站
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task InStationByEquipment(EquipmentInStationDto param);

        /// <summary>
        /// 根据设备批量进站
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task InStationRangeByEquipment(EquipmentInStationDto param);

        /// <summary>
        /// 根据资源进站
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task InStationByResource(ResourceInStationDto param);

        /// <summary>
        /// 根据资源批量进站
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task InStationRangeByResource(ResourceInStationsDto param);
    }
}
