using AutoMapper;
using Dapper;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Mapper;
using Hymson.Kafka.Debezium;
using Hymson.Kafka.Debezium.DbInstances;
using Hymson.MES.Data;
using Hymson.Utils;
using Hymson.Web.Framework.Filters;
using Hymson.WebApi.Filters;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System.Globalization;
using System.Reflection;

namespace Hymson.MES.Api
{
    /// <summary>
    /// 
    /// </summary>
    public class Program
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers(options =>
            {
                options.Filters.Add(typeof(HttpGlobalExceptionFilter));
                options.Filters.Add(typeof(HttpGlobalActionFilter));
                options.Filters.Add(new AuthorizeFilter());
            }).AddJsonOptions((jsonOptions) =>
            {
                jsonOptions.JsonSerializerOptions.Converters.Add(new CustomInt64Converter());
            });

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddMemoryCache();
            builder.Services.AddClearCacheService(builder.Configuration);
            builder.Services.AddHostedService<WorkService>();

            AddSwaggerGen(builder.Services);

            builder.Services.AddJwtBearerService(builder.Configuration);
            builder.Services.AddAppService(builder.Configuration);
            builder.Services.AddSqlLocalization(builder.Configuration);
            builder.Services.AddHttpClientService(builder.Configuration);
            builder.Services.AddEventBusRabbitMQService(builder.Configuration);
            builder.Services.AddLocalization();
            builder.Services.AddHealthChecks();
            builder.Services.AddPrintService(builder.Configuration);
            // 注入nlog日志服务
            builder.AddNLogWeb(builder.Configuration);
            AddAutoMapper();
            var app = builder.Build();
#if DM
            HashSet<string?> keys = CommonHelper.GetConstHashSet(typeof(CachedTables));
            SqlTransformHelper.SetSqlTransform((sql) =>
            {
                var tempSql=sql.ToLower();
                if (!tempSql.StartsWith("select"))
                {
                    var  keyValues = keys.Where(x => tempSql.Contains(x ?? ""));
                    if (keyValues!=null&& keyValues.Any())
                    {
                        var tableName= keyValues.First();
                        Singleton<KafkaProducer<MESDbInstance>>.Instance.Publish($"mos.MESBATTERY.{tableName}", new BinlogData {  Source=new Source {  DB= "MESBATTERY", Table= tableName, Name=""} });
                    }
                }
                return sql.Replace("`", "\"").Replace("@", ":");
            });
#endif
            app.UseHealthChecks("/healthy");
            //https://learn.microsoft.com/zh-cn/aspnet/core/host-and-deploy/linux-nginx?view=aspnetcore-6.0&tabs=linux-ubuntu
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });
            // TODO 龙总说要这么要开放出来
            //#if DEBUG
            // Configure the HTTP request pipeline.
            //if (!app.Environment.IsProduction())
            //{
            app.UseSwagger();
            app.UseSwaggerUI();
            //}
            //#endif

#if DEBUG
            app.UseCors(builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyHeader()
                       .AllowAnyMethod()
                       .SetPreflightMaxAge(TimeSpan.FromHours(24));
            });
#endif

            #region snippet_ConfigureLocalization
            var supportedCultures = new List<CultureInfo>
            {
                new CultureInfo("en-US"),
                new CultureInfo("en-AU"),
                new CultureInfo("en-GB"),
                new CultureInfo("es-ES"),
                new CultureInfo("ja-JP"),
                new CultureInfo("fr-FR"),
                new CultureInfo("zh"),
                new CultureInfo("zh-CN"),
                new CultureInfo("en")
            };
            var options = new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture("zh-CN"),
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures
            };

            app.UseRequestLocalization(options);
            #endregion
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();
            app.Run();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        private static void AddSwaggerGen(IServiceCollection services)
        {
            //#if DEBUG
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "MES Web API",
                    Description = "MES Web API",
                    TermsOfService = new Uri("https://www.hymson.com/"),
                    Contact = new OpenApiContact
                    {
                        Name = "靳毅",
                        Url = new Uri("https://www.hymson.com/")
                    },
                    License = new OpenApiLicense
                    {
                        Name = "海目星激光",
                        Url = new Uri("https://www.hymson.com/")
                    }
                });

                // using System.Reflection;
                var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename), true);

                var xmlFilename2 = $"Hymson.MES.Services.xml";
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename2), true);

                options.OperationFilter<AddResponseHeadersFilter>();
                //options.OperationFilter<AppendAuthorizeToSummaryOperationFilter>();

                // 在header 中添加token，传递到后台
                options.OperationFilter<SecurityRequirementsOperationFilter>();
                // JwtBearerDefaults.AuthenticationScheme
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "前置Bearer。示例：Bearer {Token}",
                    Name = "Authorization",//jwt默认的参数名称,
                    Type = SecuritySchemeType.Http, //指定ApiKey
                    BearerFormat = "JWT",//标识承载令牌的格式 该信息主要是出于文档目的
                    Scheme = JwtBearerDefaults.AuthenticationScheme//授权中要使用的HTTP授权方案的名称
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
                        },
                        new List<string>()
                    }
                });

                //options.OperationFilter<SecurityRequirementsOperationFilter>();
                //options.OperationFilter<AuthorizationOperationFilter>();
            });
            //#endif
        }

        /// <summary>
        /// 
        /// </summary>
        private static void AddAutoMapper()
        {
            //find mapper configurations provided by other assemblies
            var typeFinder = Singleton<ITypeFinder>.Instance;
            var mapperConfigurations = typeFinder.FindClassesOfType<IOrderedMapperProfile>();

            //create and sort instances of mapper configurations
            var instances = mapperConfigurations
                .Select(mapperConfiguration => (IOrderedMapperProfile)Activator.CreateInstance(mapperConfiguration))
                .OrderBy(mapperConfiguration => mapperConfiguration.Order);

            //create AutoMapper configuration
            var config = new MapperConfiguration(cfg =>
            {
                foreach (var instance in instances)
                {
                    cfg.AddProfile(instance.GetType());
                }
            });

            //register
            AutoMapperConfiguration.Init(config);
        }

    }
}