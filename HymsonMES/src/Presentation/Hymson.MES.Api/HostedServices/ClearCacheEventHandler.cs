using Hymson.Kafka.Debezium;
using Microsoft.Extensions.Caching.Memory;

namespace Hymson.MES.Api.HostedServices
{
    internal class ClearCacheEventHandler : IKafkaEventHandler<BinlogData>
    {
        private readonly ILogger<ClearCacheEventHandler> _logger;
        private readonly IMemoryCache _memoryCache;

        public ClearCacheEventHandler(ILogger<ClearCacheEventHandler> logger,IMemoryCache memoryCache)
        {
            _logger = logger;
            _memoryCache = memoryCache;
        }
        public Task Handle(BinlogData @event)
        {
            _memoryCache.RemoveCacheRegex(@event.Payload.Source.Table);
            _logger.LogInformation($"{@event}");
            return Task.CompletedTask;
        }
    }
}
