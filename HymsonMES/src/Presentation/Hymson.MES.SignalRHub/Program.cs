using Hymson.Authentication.JwtBearer;
using Hymson.MES.BackgroundTasks.HostedServices;
using Hymson.MES.SignalRHub.Hubs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using StackExchange.Redis;
using System.Text;
using Hymson.MES.SignalRHub.Extensions.DependencyInjection;

namespace Hymson.MES.SignalRHub
{
    public class Program
    {     /// <summary>
          /// 
          /// </summary>
          /// <param name="args"></param>
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddCors(options =>
       {
           options.AddPolicy("CorsPolicy",
               builder => builder
               .AllowAnyMethod()
               .AllowAnyHeader()
               .SetIsOriginAllowed((host) => true)
               .AllowCredentials());
       });
            builder.Services.AddControllers();
            builder.Services.AddLocalization();
            builder.Services.AddSqlLocalization(builder.Configuration);
            builder.Services.AddMemoryCache();
            builder.Services.AddClearCacheService(builder.Configuration);
            builder.Services.AddNLog(builder.Configuration);
            builder.Services.AddEventBusRabbitMQService(builder.Configuration);
            builder.Services.AddSignalR();
            builder.Services.AddSignalRStackExchangeRedis(builder.Configuration);
            builder.Services.AddHostedService<SubHostedService>();

            builder.Services.AddCostomAuthentication(builder.Configuration);

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();
            app.MapHub<NotificationHub>("/api2");
            app.UseCors("CorsPolicy");
            app.Run();
        }
    }
}