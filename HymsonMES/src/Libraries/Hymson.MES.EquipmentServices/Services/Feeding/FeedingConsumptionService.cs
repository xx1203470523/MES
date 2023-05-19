using FluentValidation;
using Hymson.MES.EquipmentServices.Dtos.FeedingConsumption;
using Hymson.Web.Framework.WorkContext;

namespace Hymson.MES.EquipmentServices.Services.Feeding
{
    /// <summary>
    /// 上报物料消耗服务
    /// </summary>
    public class FeedingConsumptionService : IFeedingConsumptionService
    {
        private readonly ICurrentEquipment _currentEquipment;
        private readonly AbstractValidator<FeedingConsumptionDto> _validationFeedingConsumptionDtoRules;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="validationFeedingConsumptionDtoRules"></param>
        /// <param name="currentEquipment"></param>
        public FeedingConsumptionService(AbstractValidator<FeedingConsumptionDto> validationFeedingConsumptionDtoRules, ICurrentEquipment currentEquipment)
        {
            _validationFeedingConsumptionDtoRules = validationFeedingConsumptionDtoRules;
            _currentEquipment = currentEquipment;
        }

        /// <summary>
        /// 上报物料消耗
        /// </summary>
        /// <param name="feedingConsumptionDto"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task FeedingConsumptionAsync(FeedingConsumptionDto feedingConsumptionDto)
        {
            await _validationFeedingConsumptionDtoRules.ValidateAndThrowAsync(feedingConsumptionDto);
            throw new NotImplementedException();
        }
    }
}
