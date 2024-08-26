using Hymson.MES.HttpClients;
using Hymson.MES.HttpClients.Options;
using Hymson.MES.HttpClients.RotorHandle;
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
            var rotorOptions = new RotorOption();
            configuration.GetSection("RotorOption").Bind(rotorOptions);
            services.AddHttpClient<IRotorApiClient, RotorApiClient>().ConfigureHttpClient(httpClient =>
            {
                httpClient.BaseAddress = new Uri(rotorOptions.BaseAddressUri);
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(nameof(RotorOption.SYSTOKEN), rotorOptions.SYSTOKEN);
            }); //.AddHttpMessageHandler<LoggingDelegatingHandler>();

            var printOptions = new PrintOptions();
            configuration.GetSection("PrintOptions").Bind(printOptions);
            services.AddHttpClient<ILabelPrintRequest, FastReportPrintRequest>().ConfigureHttpClient(httpClient =>
            {
                httpClient.BaseAddress = new Uri(printOptions.BaseAddressUri);
            });

            services.AddSingleton<IERPApiClient, ERPApiClient>();
            services.AddSingleton<IWMSApiClient, WMSApiClient>();
            AddHttpClientConfig(services, configuration);
            return services;
        }

        /// <summary>
        /// 添加配置
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        private static IServiceCollection AddHttpClientConfig(IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<PrintOptions>(configuration.GetSection(nameof(PrintOptions)));
            services.Configure<ERPOptions>(configuration.GetSection(nameof(ERPOptions)));
            services.Configure<WMSOptions>(configuration.GetSection(nameof(WMSOptions)));
            return services;
        }

    }
}
