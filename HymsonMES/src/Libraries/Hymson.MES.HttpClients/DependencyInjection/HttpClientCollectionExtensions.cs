
using Hymson.MES.HttpClients;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// 
    /// </summary>
    public static partial class HttpClientCollectionExtensions
    {
        /// <summary>
        /// 业务逻辑层依赖服务添加
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddHttpClientService(this IServiceCollection services, IConfiguration configuration)
        {
           
            //var printOptions = new PrintOptions();
            //configuration.GetSection("PrintOptions").Bind(printOptions);
            //services.AddHttpClient<ILabelPrintRequest, FastReportPrintRequest>().ConfigureHttpClient(httpClient =>
            //{
            //    httpClient.BaseAddress = new Uri(printOptions.BaseAddressUri);
            //});

            var wmsOptions = new XnebulaWMSOptions();
            configuration.GetSection("WMSOptions").Bind(wmsOptions);
            services.AddHttpClient<IXnebulaWMSServer, XnebulaWMSServer>().ConfigureHttpClient(httpClient =>
            {
                httpClient.BaseAddress = new Uri(wmsOptions.BaseAddressUri);
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
            services.Configure<PrintOptions>(configuration.GetSection(nameof(PrintOptions)));
            return services;
        }

        
    }
}
