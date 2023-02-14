using AutoMapper;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Api.Filters;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
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
            });
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            AddSwaggerGen(builder.Services);

            builder.Services.AddJwtBearerService(builder.Configuration);
            builder.Services.AddAppService(builder.Configuration);
            // 注入nlog日志服务
            builder.AddNLogWeb(builder.Configuration);
            AddAutoMapper();
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

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
#if DEBUG
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
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));

                var xmlFilename2 = $"Hymson.MES.Services.xml";
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename2));

                options.OperationFilter<AddResponseHeadersFilter>();
                //options.OperationFilter<AppendAuthorizeToSummaryOperationFilter>();

                // 在header 中添加token，传递到后台
                options.OperationFilter<SecurityRequirementsOperationFilter>();
                // JwtBearerDefaults.AuthenticationScheme
                options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "前置Bearer。示例：Bearer {Token}",
                    Name = "Authorization",//jwt默认的参数名称,
                    Type = SecuritySchemeType.ApiKey, //指定ApiKey
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
#endif
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