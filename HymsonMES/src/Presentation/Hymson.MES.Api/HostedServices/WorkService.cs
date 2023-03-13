using Hymson.Kafka.Debezium;
using Hymson.Kafka.Debezium.DbInstances;
using Hymson.Localization.Services;
using Hymson.MES.Core.Constants;
using System.Threading;

namespace Hymson.MES.Api
{
    public class WorkService : BackgroundService
    {
        private readonly IResourceService _resourceService;
        private readonly ILogger<WorkService> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly KafkaConsumer<MESDbInstance> _kafkaConsumer;

        public WorkService(IResourceService resourceService,ILogger<WorkService> logger,
            IServiceProvider serviceProvider,
            KafkaConsumer<MESDbInstance> kafkaConsumer)
        {
            _resourceService = resourceService;
            _logger = logger;
            _serviceProvider = serviceProvider;
            _kafkaConsumer = kafkaConsumer;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                _kafkaConsumer.Subscribe($"mes.mes_master.equ_equipment", _serviceProvider.GetRequiredService<IKafkaEventHandler<BinlogData>>(), stoppingToken, 1);
                _kafkaConsumer.Subscribe($"mes.mes_master.equ_equipment_group", _serviceProvider.GetRequiredService<IKafkaEventHandler<BinlogData>>(), stoppingToken, 1);
                await _resourceService.InitEnumAsync();
                await _resourceService.InitErrorCodeAsync(typeof(ErrorCode));

            }
            catch (Exception e)
            {

                _logger.LogError(e, "初始化失败");
            }
           
        }
    }
}
