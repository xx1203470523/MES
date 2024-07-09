using Hymson.EventBus.Abstractions;
using Hymson.Infrastructure;
using Hymson.MES.BackgroundServices.EventHandling;
using Hymson.MES.BackgroundServices.EventHandling.ProcessEventHandling.PrintEventHandling;
using Hymson.MES.BackgroundServices.Quality.EquMaintenanceAutoCreateIntegration;
using Hymson.MES.BackgroundServices.Quality.EquMaintenanceAutoStopIntegration;
using Hymson.MES.BackgroundServices.Quality.EquSpotcheckAutoCreateIntegration;
using Hymson.MES.BackgroundServices.Quality.EquSpotcheckAutoStopIntegration;
using Hymson.MES.BackgroundServices.Tasks.Quality.FQCOrderAutoCreateIntegration;
using Hymson.MES.CoreServices.Events.Equipment;
using Hymson.MES.CoreServices.Events.ProcessEvents.PrintEvents;
using Hymson.MES.CoreServices.Events.Quality;
using Hymson.MES.CoreServices.IntegrationEvents.Events.Messages;
using Hymson.MES.Data.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Hymson.MES.CoreServices.DependencyInjection
{
    /// <summary>
    /// 依赖注入项配置
    /// </summary>
    public static partial class ServiceCollectionExtensions
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
            AddConfig(services, configuration);

            AddEventBusServices(services);
            AddEventBusServicesForXinShiJie(services);

            AddServices(services);

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
            // 数据库连接
            services.Configure<ParameterOptions>(configuration.GetSection(nameof(ParameterOptions)));
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
            services.AddSingleton<IIntegrationEventHandler<PrintIntegrationEvent>, ExecPrintIntegrationEventHandler>();
            services.AddSingleton<IIntegrationEventHandler<FQCOrderAutoCreateIntegrationEvent>, FQCOrderAutoCreateIntegrationEventHandler>();
            services.AddSingleton<IIntegrationEventHandler<EquSpotcheckAutoCreateIntegrationEvent>, EquSpotcheckAutoCreateIntegrationEventHandler>();
            services.AddSingleton<IIntegrationEventHandler<EquSpotcheckAutoStopIntegrationEvent>, EquSpotcheckAutoStopIntegrationEventHandler>();
            services.AddSingleton<IIntegrationEventHandler<EquMaintenanceAutoCreateIntegrationEvent>, EquMaintenanceAutoCreateIntegrationEventHandler>();
            services.AddSingleton<IIntegrationEventHandler<EquMaintenanceAutoStopIntegrationEvent>, EquMaintenanceAutoStopIntegrationEventHandler>();
        }

        /// <summary>
        /// 注入服务
        /// </summary>
        /// <param name="services"></param>
        static void AddServices(IServiceCollection services)
        {
            var typeFinder = Singleton<ITypeFinder>.Instance;
            var keyValuePairs = typeFinder.GetInterfaceImplPairs("Service");
            foreach (var keyValuePair in keyValuePairs)
            {
                services.TryAddSingleton(keyValuePair.Value, keyValuePair.Key);
            }
        }

    }
}
