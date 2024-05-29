using Hymson.EventBus.Abstractions;
using Hymson.MES.CoreServices.Events.Equipment;
using Hymson.MES.CoreServices.Services.EquMaintenancePlan;

namespace Hymson.MES.BackgroundServices.Quality.EquMaintenanceAutoStopIntegration
{
    public class EquMaintenanceAutoStopIntegrationEventHandler : IIntegrationEventHandler<EquMaintenanceAutoStopIntegrationEvent>
    {
        private readonly IEquMaintenancePlanCoreService _EquMaintenanceStopService;

        public EquMaintenanceAutoStopIntegrationEventHandler(IEquMaintenancePlanCoreService EquMaintenanceStopService)
        {
            _EquMaintenanceStopService = EquMaintenanceStopService;
        }

        public async Task Handle(EquMaintenanceAutoStopIntegrationEvent @event)
        {
            await _EquMaintenanceStopService.StopEquMaintenanceTaskAsync(@event);
        }
    }
}
