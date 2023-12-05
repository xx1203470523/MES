using Hymson.EventBus.Abstractions;
using Hymson.MES.BackgroundServices.EventHandling;
using Hymson.MES.BackgroundServices.Manufacture;
using Hymson.MES.BackgroundServices.Manufacture.Productionstatistic;
using Hymson.MES.CoreServices.IntegrationEvents.Events.Messages;
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
            AddConfig(services);
            AddServices(services);
            return services;
        }

        /// <summary>
        /// 添加配置
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        private static IServiceCollection AddConfig(IServiceCollection services)
        {
            return services;
        }
        /// <summary>
        /// 订阅
        /// </summary>
        /// <param name="services"></param>
        static void AddEventBusServices(IServiceCollection services)
        {
            services.AddSingleton<IIntegrationEventHandler<MessageHandleUpgradeIntegrationEvent>, MessageHandleUpgradeIntegrationEventHandler>();
            services.AddSingleton<IIntegrationEventHandler<MessageReceiveUpgradeIntegrationEvent>, MessageReceiveUpgradeIntegrationEventHandler>();
            services.AddSingleton<IIntegrationEventHandler<MessageTriggerUpgradeIntegrationEvent>, MessageTriggerUpgradeIntegrationEventHandler>();
        }

        /// <summary>
        /// 注入服务
        /// </summary>
        /// <param name="services"></param>
        static void AddServices(IServiceCollection services)
        {
            services.AddSingleton<IProductionstatisticService, ProductionstatisticService>();
            services.AddSingleton<IWorkOrderStatisticService, WorkOrderStatisticService>();
        }
    }
}
