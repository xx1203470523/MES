using Hymson.MES.EquipmentServices.Dtos.FeedingConsumption;

namespace Hymson.MES.EquipmentServices.Services.Feeding
{
    /// <summary>
    /// 上报物料消耗
    /// </summary>
    public interface IFeedingConsumptionService
    {
        /// <summary>
        /// 上报物料消耗
        /// </summary>
        /// <param name="feedingConsumptionDto"></param>
        /// <returns></returns> 
        Task FeedingConsumptionAsync(FeedingConsumptionDto feedingConsumptionDto); 
    }
}
