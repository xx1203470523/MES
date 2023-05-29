
using Hymson.MES.HttpClients;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Net.NetworkInformation;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// 
    /// </summary>
    public static class HttpClientCollectionExtensions
    {
        /// <summary>
        /// 业务逻辑层依赖服务添加
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddHttpClientService(this IServiceCollection services, IConfiguration configuration)
        {


           
            var printOptions = new PrintOptions();
            configuration.GetSection("PrintOptions").Bind(printOptions);
            services.AddHttpClient<ILabelPrintRequest, FastReportPrintRequest>().ConfigureHttpClient(httpClient =>
            {
                httpClient.BaseAddress = new Uri(printOptions.BaseAddressUri);
                //httpClient.BaseAddress = new Uri("http://10.9.1.57:50892/");
                //httpClient.BaseAddress = new Uri("http://localhost/NFXBaseService/");
            });

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
            //
            services.Configure<PrintOptions>(configuration.GetSection(nameof(PrintOptions)));
            //services.Configure<ConnectionOptions>(configuration);
            return services;
        }

        
    }
}
