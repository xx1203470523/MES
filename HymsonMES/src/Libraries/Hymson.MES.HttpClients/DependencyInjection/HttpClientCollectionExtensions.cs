using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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
            //services.Configure<TestOptions>(configuration.GetSection(nameof(TestOptions)));
            //services.Configure<ConnectionOptions>(configuration);
            return services;
        }

        
    }
}
