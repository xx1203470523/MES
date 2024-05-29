using Hymson.EventBus.Abstractions;
using Hymson.MES.CoreServices.Events.Equipment;
using Hymson.MES.CoreServices.Services.EquSpotcheckPlan;

namespace Hymson.MES.BackgroundServices.Quality.EquSpotcheckAutoStopIntegration
{
    public class EquSpotcheckAutoStopIntegrationEventHandler : IIntegrationEventHandler<EquSpotcheckAutoStopIntegrationEvent>
    {
        private readonly IEquSpotcheckPlanCoreService _EquSpotcheckStopService;

        public EquSpotcheckAutoStopIntegrationEventHandler(IEquSpotcheckPlanCoreService EquSpotcheckStopService)
        {
            _EquSpotcheckStopService = EquSpotcheckStopService;
        }

        public async Task Handle(EquSpotcheckAutoStopIntegrationEvent @event)
        {
            await _EquSpotcheckStopService.StopEquSpotcheckTaskAsync(@event);
        }
    }
}
