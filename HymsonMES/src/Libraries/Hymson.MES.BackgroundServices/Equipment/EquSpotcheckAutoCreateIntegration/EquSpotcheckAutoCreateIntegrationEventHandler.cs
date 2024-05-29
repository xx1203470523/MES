using Hymson.EventBus.Abstractions;
using Hymson.MES.CoreServices.Events.Quality;
using Hymson.MES.CoreServices.Services.EquMaintenancePlan;
using Hymson.MES.CoreServices.Services.EquSpotcheckPlan;

namespace Hymson.MES.BackgroundServices.Quality.EquSpotcheckAutoCreateIntegration
{
    public class EquSpotcheckAutoCreateIntegrationEventHandler : IIntegrationEventHandler<EquSpotcheckAutoCreateIntegrationEvent>
    {
        private readonly IEquSpotcheckPlanCoreService _EquSpotcheckCreateService;

        public EquSpotcheckAutoCreateIntegrationEventHandler(IEquSpotcheckPlanCoreService EquSpotcheckCreateService)
        {
            _EquSpotcheckCreateService = EquSpotcheckCreateService;
        }

        public async Task Handle(EquSpotcheckAutoCreateIntegrationEvent @event)
        {
            await _EquSpotcheckCreateService.GenerateEquSpotcheckTaskAsync(@event);
        }
    }
}
