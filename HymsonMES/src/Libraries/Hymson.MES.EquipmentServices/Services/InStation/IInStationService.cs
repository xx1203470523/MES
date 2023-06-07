using Hymson.MES.Core.Domain.Manufacture;

namespace Hymson.MES.EquipmentServices.Services.Manufacture.InStation
{
    /// <summary>
    /// 进站
    /// </summary>
    public interface IInStationService
    {
        /// <summary>
        /// 执行（进站）
        /// </summary>
        /// <param name="inStationDto"></param>
        /// <returns></returns>
        Task InStationExecuteAsync(InStationDto inStationDto); 

        /// <summary>
        /// 执行（进站）
        /// </summary>
        /// <param name="sfcProduceEntity"></param>
        /// <returns></returns> 
        Task<int> InStationAsync(ManuSfcProduceEntity sfcProduceEntity);

    }
}
