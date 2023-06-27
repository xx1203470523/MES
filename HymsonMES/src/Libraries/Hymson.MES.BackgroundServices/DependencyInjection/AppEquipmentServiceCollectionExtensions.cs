using FluentValidation;
using Hymson.MES.BackgroundServices.Services.EquHeartbeat;
using Microsoft.Extensions.Configuration;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// 
    /// </summary>
    public static class AppEquipmentServiceCollectionExtensions
    {
        /// <summary>
        /// 业务逻辑层依赖服务添加
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddBackgroundServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddData(configuration);
            AddConfig(services, configuration);
            AddServices(services);
            AddValidators(services);
            return services;
        }

        /// <summary>
        /// 添加服务依赖
        /// </summary>
        /// <param name="services"></param>
        private static void AddServices(this IServiceCollection services)
        {
            services.AddSingleton<IEquHeartbeatService, EquHeartbeatService>();

        }

        /// <summary>
        /// 添加配置
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        private static void AddConfig(IServiceCollection services, IConfiguration configuration)
        {
            //数据库连接
            //services.Configure<TestOptions>(configuration.GetSection(nameof(TestOptions)));
            //services.Configure<ConnectionOptions>(configuration);
            //return services;
        }

        /// <summary>
        /// 添加验证器相关服务
        /// </summary>
        /// <param name="services"></param>
        private static void AddValidators(IServiceCollection services)
        {
            //services.AddSingleton<AbstractValidator<SfcBindingDto>, SfcBindingValidator>();
        }

    }
}
