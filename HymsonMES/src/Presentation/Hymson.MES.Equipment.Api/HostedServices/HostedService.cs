using Hymson.Authentication.JwtBearer;
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
        private readonly IResourceService _resourceService;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="jwtOptions"></param>
        /// <param name="resourceService"></param>
        public HostedService(IOptions<JwtOptions> jwtOptions,
            IResourceService resourceService)
        {
            _jwtOptions = jwtOptions.Value;
            _resourceService = resourceService;
        }
        /// <summary>
        /// 启动时运行
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var equipmentModel = new EquipmentModel
            {
                FactoryId = 123456,
                Id = 12870073632952320,
                Name = "盖板转接片激光焊接机1#",
                SiteId = 123456,
                Code = "Test"
            };
            var token = JwtHelper.GenerateJwtToken(equipmentModel, _jwtOptions);
            Console.WriteLine(token);

            try
            {
                await _resourceService.InitEnumAsync();
                await _resourceService.InitErrorCodeAsync(typeof(ErrorCode));
            }
            catch (Exception e)
            {
                
            }

            await Task.CompletedTask;
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
