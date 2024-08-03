using FluentValidation;
using Hymson.Infrastructure;
using Hymson.MES.CoreServices.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// 
    /// </summary>
    public static partial class AppEquipmentServiceCollectionExtensions
    {
        /// <summary>
        /// 业务逻辑层依赖服务添加
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddEquipmentService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddWebFrameworkService(configuration);
            services.AddCoreService(configuration);
            services.AddMinioService(configuration);
            AddConfig(services, configuration);

            AddServices(services);
            AddServicesForXinShiJie(services);

            AddValidators(services);
            AddValidatorsForXinShiJie(services);

            return services;
        }

        /// <summary>
        /// 添加服务依赖
        /// </summary>
        /// <param name="services"></param>
        private static void AddServices(this IServiceCollection services)
        {
            var typeFinder = Singleton<ITypeFinder>.Instance;
            var keyValuePairs = typeFinder.GetInterfaceImplPairs("Service");
            foreach (var keyValuePair in keyValuePairs)
            {
                services.TryAddSingleton(keyValuePair.Value, keyValuePair.Key);
            }
        }

        /// <summary>
        /// 添加配置
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        private static void AddConfig(IServiceCollection services, IConfiguration configuration)
        {

        }

        /// <summary>
        /// 添加验证器相关服务
        /// </summary>
        /// <param name="services"></param>
        private static void AddValidators(IServiceCollection services)
        {
            var typeFinder = Singleton<ITypeFinder>.Instance;
            var abstractValidators = typeFinder.FindClassesOfType(typeof(IValidator<>)).ToList();
            foreach (var abstractValidator in abstractValidators)
            {
                if (abstractValidator.BaseType != null)
                {
                    services.TryAddSingleton(abstractValidator.BaseType, abstractValidator);
                }
            }
        }

    }
}
