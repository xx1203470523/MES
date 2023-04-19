using Hymson.ClearCache;
using Hymson.Infrastructure.Enums;
using Hymson.Localization.Services;
using Hymson.MES.Core.Constants;

namespace Hymson.MES.Api
{
    public class WorkService : BackgroundService
    {
        private readonly IResourceService _resourceService;
        private readonly ILogger<WorkService> _logger;
        private readonly IClearCacheService _clearCacheService;

        public WorkService(IResourceService resourceService,ILogger<WorkService> logger,
            IClearCacheService clearCacheService)
        {
            _resourceService = resourceService;
            _logger = logger;
            _clearCacheService = clearCacheService;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                await _clearCacheService.ClearCacheAsync(new ServiceTypeEnum[] { 
                 ServiceTypeEnum.User,
                  ServiceTypeEnum.MES
                },stoppingToken);
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
