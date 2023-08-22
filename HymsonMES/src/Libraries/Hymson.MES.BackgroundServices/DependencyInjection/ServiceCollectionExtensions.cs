using Hymson.EventBus.Abstractions;
using Hymson.MES.BackgroundServices.EventHandling;
using Hymson.MES.CoreServices.IntegrationEvents.Events.Messages;
using Hymson.MES.CoreServices.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Hymson.MES.CoreServices.DependencyInjection
{
    /// <summary>
    /// 依赖注入项配置
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// 业务逻辑层依赖服务添加
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddBackgroundServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddCoreService(configuration);
            AddEventBusServices(services);
            AddServices(services);
            AddValidators(services);
            AddConfig(services, configuration);
            return services;
        }

        /// <summary>
        /// 添加服务依赖
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        private static IServiceCollection AddServices(this IServiceCollection services)
        {
            //services.AddSingleton<IMessagePushService, MessagePushService>();
            return services;
        }

        /// <summary>
        /// 添加验证器相关服务
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        private static IServiceCollection AddValidators(IServiceCollection services)
        {
            return services;
        }

        /// <summary>
        /// 添加配置
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        private static IServiceCollection AddConfig(IServiceCollection services, IConfiguration configuration)
        {
            //数据库连接
            services.Configure<ParameterOptions>(configuration.GetSection(nameof(ParameterOptions)));
            //services.Configure<ConnectionOptions>(configuration);
            return services;
        }
        /// <summary>
        /// 订阅
        /// </summary>
        /// <param name="services"></param>
        static void AddEventBusServices(IServiceCollection services)
        {
            services.AddSingleton<IIntegrationEventHandler<MessageHandleUpgradeEvent>, MessageHandleUpgradeEventHandler>();
            services.AddSingleton<IIntegrationEventHandler<MessageReceiveUpgradeEvent>, MessageReceiveUpgradeEventHandler>();
            services.AddSingleton<IIntegrationEventHandler<MessageTriggerUpgradeEvent>, MessageTriggerUpgradeEventHandler>();
        }
    }
}
