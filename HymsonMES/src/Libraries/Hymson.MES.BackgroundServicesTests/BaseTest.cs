using Hymson.MES.Data.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Hymson.MES.BackgroundServicesTests
{
    public class BaseTest
    {
        /// <summary>
        /// Configuration
        /// </summary>
        public IConfiguration? Configuration { get; set; }
        /// <summary>
        /// 当前连接可能为空
        /// </summary>
        public ConnectionOptions? ConnectionOptions { get; set; }
        /// <summary>
        /// ServiceProvider
        /// </summary>
        public ServiceProvider ServiceProvider { get; set; }

        public BaseTest()
        {
            Configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();
            //注入一些依赖的服务
            ServiceProvider = new ServiceCollection()
                .AddOptions()
                .AddMemoryCache()
                .AddSequenceService(Configuration)
                .AddBackgroundServices(Configuration)
                .Configure<ConnectionOptions>(Configuration)
                .BuildServiceProvider();

            ConnectionOptions = ServiceProvider.GetRequiredService<IOptions<ConnectionOptions>>().Value;
        }
    }
}
