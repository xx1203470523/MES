using Hymson.EventBus.Abstractions;
using Hymson.MES.CoreServices.Events.Quality;
using Hymson.MES.CoreServices.Services.Quality.QualFqcOrder;

namespace Hymson.MES.BackgroundServices.Quality.FQCOrderAutoCreateIntegration
{
    public class FQCOrderAutoCreateIntegrationEventHandler : IIntegrationEventHandler<FQCOrderAutoCreateIntegrationEvent>
    {
        private readonly IFQCOrderCreateService _fQCOrderCreateService;


        public FQCOrderAutoCreateIntegrationEventHandler(IFQCOrderCreateService fQCOrderCreateService)
        {
            _fQCOrderCreateService = fQCOrderCreateService;
        }

        public async Task Handle(FQCOrderAutoCreateIntegrationEvent @event)
        {
            await _fQCOrderCreateService.CreateFqcAsync(@event);
        }
    }
}
