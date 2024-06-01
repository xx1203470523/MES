using Confluent.Kafka;
using Hymson.Authentication.JwtBearer;
using Hymson.ClearCache;
using Hymson.Infrastructure.Enums;
using Hymson.Localization.Services;
using Microsoft.Extensions.Options;

namespace Hymson.MES.System.Api.HostedServices
{
    /// <summary>
    /// 
    /// </summary>
    public class HostedService : BackgroundService
    {
        private readonly JwtOptions _jwtOptions;

        /// <summary>
        /// 
        /// </summary>
        private readonly ILogger<HostedService> _logger;
        private readonly IResourceService _resourceService;
        private readonly IClearCacheService _clearCacheService;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="jwtOptions"></param>
        public HostedService(IOptions<JwtOptions> jwtOptions, ILogger<HostedService> logger,
            IResourceService resourceService,
            IClearCacheService clearCacheService)
        {
            _jwtOptions = jwtOptions.Value;
            _resourceService = resourceService;
            _clearCacheService = clearCacheService;
        }
        /// <summary>
        /// 启动时运行
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task StartAsync(CancellationToken cancellationToken)
        {
            var systemModel = new SystemModel
            {
                FactoryId = 123456,
                Id = 12870073632952320,
                Name = "金蝶ERP",
                SiteId = 123456
            };
            var token = JwtHelper.GenerateJwtToken(systemModel, _jwtOptions);
            Console.WriteLine(token);
            return Task.CompletedTask;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="stoppingToken"></param>
        /// <returns></returns>
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                await _clearCacheService.ClearCacheAsync(new ServiceTypeEnum[] {
                 ServiceTypeEnum.User,
                 ServiceTypeEnum.MES
                }, stoppingToken);
                await _resourceService.InitEnumAsync();
                await _resourceService.InitErrorCodeAsync(typeof(ErrorCode));
                //await InitExcelDtoAsync();
            }
            catch (Exception e)
            {

                _logger.LogError(e, "初始化失败");
            }
        }

        /// <summary>
        /// 关闭时运行
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
