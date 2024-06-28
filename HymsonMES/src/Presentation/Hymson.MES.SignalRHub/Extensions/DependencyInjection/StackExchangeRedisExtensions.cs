using Hymson.Authentication.JwtBearer;
using Hymson.MES.SignalRHub.Extensions.Options;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using StackExchange.Redis;
using System.Net;

namespace Hymson.MES.SignalRHub.Extensions.DependencyInjection
{
    public static class StackExchangeRedisExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddSignalRStackExchangeRedis(this IServiceCollection services, IConfiguration configuration)
        {
            RedisOptions redisOptions = new RedisOptions();
            configuration.GetSection("RedisOptions").Bind(redisOptions);
            if (redisOptions.Enable)
            {
                services.AddSignalR().AddStackExchangeRedis(redisOptions.ConnectionString, options =>
                {
                    options.Configuration.ChannelPrefix = new RedisChannel(redisOptions.InstanceName, RedisChannel.PatternMode.Auto); // 替换为你的前缀
                });
            }
            else
            {
                services.AddSignalR();
            }
            return services;
        }

    }
}
