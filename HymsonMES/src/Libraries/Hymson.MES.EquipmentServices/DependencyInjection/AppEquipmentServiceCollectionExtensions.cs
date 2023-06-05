using Hymson.MES.CoreServices.Services.Common;
using Hymson.MES.EquipmentServices.Services.Common;
using Hymson.MES.EquipmentServices.Services.Job.Implementing;
using Hymson.MES.EquipmentServices.Services.Manufacture.InStation;
using Hymson.MES.EquipmentServices.Services.SfcBinding;
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
        public static IServiceCollection AddEquipmentService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddWebFrameworkService(configuration);
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
            services.AddSingleton<ICommonService, CommonService>();
            services.AddSingleton<IInStationService, InStationService>();
            services.AddSingleton<IJobManufactureService, JobManuSfcConvertService>();
            services.AddSingleton<ISfcBindingService, SfcBindingService>();

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

        }

    }
}
