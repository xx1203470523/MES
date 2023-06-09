using Hymson.MES.CoreServices.Services.Common;
using Hymson.MES.CoreServices.Services.Common.ManuCommon;
using Hymson.MES.CoreServices.Services.Common.MasterData;
using Hymson.MES.CoreServices.Services.Manufacture.ManuMainstreamProcess.GenerateBarcode;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Hymson.MES.CoreServices.DependencyInjection
{
    /// <summary>
    /// 
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// 业务逻辑层依赖服务添加
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddCoreService(this IServiceCollection services, IConfiguration configuration)
        {
            AddServices(services);
            return services;
        }

        /// <summary>
        /// 添加服务依赖
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        private static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddSingleton<IManuCommonService, ManuCommonService>();
            services.AddSingleton<IMasterDataService, MasterDataService>();
            services.AddSingleton<IJobCommonService, JobCommonService>();

            services.AddSingleton<IManuGenerateBarcodeService, ManuGenerateBarcodeService>();//条码生成

            return services;
        }

    }
}
