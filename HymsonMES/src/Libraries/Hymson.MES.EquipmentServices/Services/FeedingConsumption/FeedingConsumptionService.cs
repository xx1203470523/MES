using FluentValidation;
using Hymson.MES.EquipmentServices.Request.FeedingConsumption;
using Hymson.Web.Framework.WorkContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.EquipmentServices.Services.FeedingConsumption
{
    /// <summary>
    /// 上报物料消耗服务
    /// </summary>
    public class FeedingConsumptionService : IFeedingConsumptionService
    {
        private readonly ICurrentEquipment _currentEquipment;
        private readonly AbstractValidator<FeedingConsumptionRequest> _validationFeedingConsumptionRequestRules;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="validationFeedingConsumptionRequestRules"></param>
        /// <param name="currentEquipment"></param>
        public FeedingConsumptionService(AbstractValidator<FeedingConsumptionRequest> validationFeedingConsumptionRequestRules, ICurrentEquipment currentEquipment)
        {
            _validationFeedingConsumptionRequestRules = validationFeedingConsumptionRequestRules;
            _currentEquipment = currentEquipment;
        }

        /// <summary>
        /// 上报物料消耗
        /// </summary>
        /// <param name="feedingConsumptionRequest"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task FeedingConsumptionAsync(FeedingConsumptionRequest feedingConsumptionRequest)
        {
            await _validationFeedingConsumptionRequestRules.ValidateAndThrowAsync(feedingConsumptionRequest);
            throw new NotImplementedException();
        }
    }
}
