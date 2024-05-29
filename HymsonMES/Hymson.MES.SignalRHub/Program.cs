using Hymson.Authentication.JwtBearer;
using Hymson.MES.BackgroundTasks.HostedServices;
using Hymson.MES.SignalRHub.Hubs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

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
            builder.Services.AddHostedService<SubHostedService>();
            ConfigureAuthService(builder.Services, builder.Configuration);

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();
            app.MapHub<NotificationHub>("/api2");
            app.UseCors("CorsPolicy");
            app.Run();
        }

        private static void ConfigureAuthService(IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<JwtOptions>(configuration.GetSection("JwtOptions"));
            JwtOptions jwtOptions = new JwtOptions();
            configuration.GetSection("JwtOptions").Bind(jwtOptions);
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(delegate (JwtBearerOptions options)
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidIssuer = jwtOptions.Issuer,
                    ValidateAudience = false,
                    ValidAudience = jwtOptions.Audience,
                    //ValidateLifetime = jwtOptions.ValidateLifetime,
                    RequireExpirationTime = jwtOptions.RequireExpirationTime,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SymmetricSecurityKeyString))
                };
                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var accessToken = context.Request.Query["access_token"];
                        var path = context.HttpContext.Request.Path;
                        if (!string.IsNullOrEmpty(accessToken) &&
                            (path.StartsWithSegments("/api2")))
                        {
                            context.Token = accessToken;
                        }
                        return Task.CompletedTask;
                    }
                };
            });
        }
    }
}