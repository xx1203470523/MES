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
    public static partial class AppServiceCollectionExtensions
    {
        /// <summary>
        /// 业务逻辑层依赖服务添加
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddAppService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSqlExecuteTaskService(configuration);
            services.AddWebFrameworkService(configuration);
            services.AddExcelService();
            services.AddMinioService(configuration);
            services.AddCoreService(configuration);
            services.AddElasticsearchService(configuration);
            AddConfig(services, configuration);

            AddServices(services);

            AddValidators(services);

            return services;
        }

        /// <summary>
        /// 添加服务依赖
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        private static IServiceCollection AddServices(this IServiceCollection services)
        {
            var typeFinder = Singleton<ITypeFinder>.Instance;
            var keyValuePairs = typeFinder.GetInterfaceImplPairs("Service");
            foreach (var keyValuePair in keyValuePairs)
            {
                services.TryAddSingleton(keyValuePair.Value, keyValuePair.Key);
            }
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
            return services;
        }

        /// <summary>
        /// 添加验证器相关服务
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        private static IServiceCollection AddValidators(IServiceCollection services)
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
            return services;
        }

    }
}
