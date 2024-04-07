using FluentValidation;
using Hymson.Infrastructure;
using Hymson.MES.CoreServices.Services.Job;
using Hymson.MES.CoreServices.Services.Job.JobUtility;
using Hymson.MES.CoreServices.Services.Job.JobUtility.Context;
using Hymson.MES.CoreServices.Services.Job.JobUtility.Execute;
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
        public static IServiceCollection AddCoreService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSequenceService(configuration);
            services.AddMessagePushService(configuration);
            services.AddData(configuration);

            AddManuServices(services);
            AddManuServicesForXinShiJie(services);

            AddIntegratedServices(services);

            AddValidators(services);
            AddValidatorsForXinShiJie(services);

            AddConfig(services, configuration);
            return services;
        }

        /// <summary>
        /// 添加服务依赖
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        private static IServiceCollection AddManuServices(this IServiceCollection services)
        {
            services.AddSingleton<ScopedServiceFactory>();
            services.AddTransient<IJobContextProxy, JobContextProxy>();
            services.AddSingleton(typeof(IExecuteJobService<>), typeof(ExecuteJobService<>));
            //services.AddSingleton<IJobService, VehicleBindJobService>();
            //services.AddSingleton<IJobService, VehicleUnBindJobService>();
            var typeFinder = Singleton<ITypeFinder>.Instance;
            var keyValuePairs = typeFinder.GetInterfaceImplPairs("Service");
          
            foreach (var keyValuePair in keyValuePairs)
            {
                services.AddSingleton(keyValuePair.Value, keyValuePair.Key);
            }
            return services;
        }

        /// <summary>
        /// 添加服务依赖（综合模块）
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        private static IServiceCollection AddIntegratedServices(this IServiceCollection services)
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

        /// <summary>
        /// 添加配置
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        private static IServiceCollection AddConfig(IServiceCollection services, IConfiguration configuration)
        {
            //数据库连接
            services.Configure<Data.Options.ParameterOptions>(configuration.GetSection(nameof(Data.Options.ParameterOptions)));
            return services;
        }
    }
}
