using Hymson.MES.EquipmentServices.Request.FeedingConsumption;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.EquipmentServices.Services.FeedingConsumption
{
    /// <summary>
    /// 上报物料消耗
    /// </summary>
    public interface IFeedingConsumptionService
    {
        /// <summary>
        /// 上报物料消耗
        /// </summary>
        /// <param name="feedingConsumptionRequest"></param>
        /// <returns></returns> 
        Task FeedingConsumptionAsync(FeedingConsumptionRequest feedingConsumptionRequest); 
    }
}
