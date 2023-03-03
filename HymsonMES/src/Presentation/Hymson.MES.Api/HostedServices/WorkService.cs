using Hymson.Localization.Services;
using Hymson.MES.Core.Constants;

namespace Hymson.MES.Api
{
    public class WorkService : BackgroundService
    {
        private readonly IResourceService _resourceService;
        private readonly ILogger<WorkService> _logger;

        public WorkService(IResourceService resourceService,ILogger<WorkService> logger)
        {
            _resourceService = resourceService;
            _logger = logger;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                await _resourceService.InitErrorCodeAsync(typeof(ErrorCode));
                await _resourceService.InitEnumAsync();
            }
            catch (Exception e)
            {

                _logger.LogError(e,"初始化失败");
            }
           
        }
    }
}
