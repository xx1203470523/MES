using Hymson.Authentication.JwtBearer;
using Hymson.ClearCache;
using Hymson.Infrastructure.Enums;
using Hymson.Localization.Services;
using Hymson.MES.Core.Constants;
using Microsoft.Extensions.Options;

namespace Hymson.MES.Equipment.Api
{
    /// <summary>
    /// 
    /// </summary>
    public class HostedService : IHostedService
    {
        private readonly JwtOptions _jwtOptions;
        private readonly IClearCacheService _clearCacheService;
        private readonly IResourceService _resourceService;
        private readonly ILogger<HostedService> _logger;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="jwtOptions"></param>
        /// <param name="clearCacheService"></param>
        /// <param name="logger"></param>
        /// <param name="resourceService"></param>
        public HostedService(IOptions<JwtOptions> jwtOptions, IClearCacheService clearCacheService,ILogger<HostedService> logger,
            IResourceService resourceService)
        {
            _jwtOptions = jwtOptions.Value;
            _clearCacheService = clearCacheService;
            _logger = logger;
            _resourceService = resourceService;
        }
        /// <summary>
        /// 启动时运行
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            try
            {
                await _clearCacheService.ClearCacheAsync(new ServiceTypeEnum[] {
                 ServiceTypeEnum.User,
                 ServiceTypeEnum.MES
                }, cancellationToken);
                
                //初始化错误提示
                await _resourceService.InitErrorCodeAsync(typeof(ErrorCode));

            }
            catch (Exception e)
            {
                _logger.LogError(e, "初始化失败");
            }
#if DEBUG
            var equipmentModel = new EquipmentModel
            {
                FactoryId = 123456,
                Id = 22225697780920320,
                Name = "盖板转接片激光焊接机1#",
                SiteId = 123456,
                Code = "Test"
            };
            var token = JwtHelper.GenerateJwtToken(equipmentModel, _jwtOptions);
            Console.WriteLine(token);
#endif
        }
        /// <summary>
        /// 关闭时运行
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task StopAsync(CancellationToken cancellationToken)
        {
            return  Task.CompletedTask;
        }
    }
}
