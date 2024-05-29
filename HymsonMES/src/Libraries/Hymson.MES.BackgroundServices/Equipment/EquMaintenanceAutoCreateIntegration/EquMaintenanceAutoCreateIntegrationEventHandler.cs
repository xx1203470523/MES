using Hymson.EventBus.Abstractions;
using Hymson.MES.CoreServices.Events.Quality;
using Hymson.MES.CoreServices.Services.EquMaintenancePlan;
using Hymson.MES.CoreServices.Services.EquMaintenancePlan;

namespace Hymson.MES.BackgroundServices.Quality.EquMaintenanceAutoCreateIntegration
{
    public class EquMaintenanceAutoCreateIntegrationEventHandler : IIntegrationEventHandler<EquMaintenanceAutoCreateIntegrationEvent>
    {
        private readonly IEquMaintenancePlanCoreService _EquMaintenanceCreateService;

        public EquMaintenanceAutoCreateIntegrationEventHandler(IEquMaintenancePlanCoreService EquMaintenanceCreateService)
        {
            _EquMaintenanceCreateService = EquMaintenanceCreateService;
        }

        public async Task Handle(EquMaintenanceAutoCreateIntegrationEvent @event)
        {
            await _EquMaintenanceCreateService.GenerateEquMaintenanceTaskAsync(@event);
        }
    }
}
