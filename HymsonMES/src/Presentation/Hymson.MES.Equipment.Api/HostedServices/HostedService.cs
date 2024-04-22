using Hymson.Authentication.JwtBearer;
using Hymson.ClearCache;
using Hymson.Infrastructure.Enums;
using Hymson.Localization.Services;
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

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="jwtOptions"></param>
        /// <param name="clearCacheService"></param>
        /// <param name="resourceService"></param>
        public HostedService(IOptions<JwtOptions> jwtOptions,
            IClearCacheService clearCacheService,
            IResourceService resourceService)
        {
            _jwtOptions = jwtOptions.Value;
            _clearCacheService = clearCacheService;
            _resourceService = resourceService;
        }

        /// <summary>
        /// 启动时运行
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            //await _clearCacheService.ClearCacheAsync(new ServiceTypeEnum[] { ServiceTypeEnum.MES }, cancellationToken);
            //await _resourceService.HotLoadingAsync();
            //await _resourceService.InitErrorCodeAsync(typeof(ErrorCode));
            //var equipmentModel = new EquipmentModel
            //{
            //    FactoryId = 123456,
            //    Id = 12870073632952320,
            //    Name = "盖板转接片激光焊接机1#",
            //    SiteId = 123456,
            //    Code = ""
            //};
            //var token = JwtHelper.GenerateJwtToken(equipmentModel, _jwtOptions);
            //Console.WriteLine(token);
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
