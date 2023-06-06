using AutoMapper;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Mapper;
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
            builder.Services.AddSequenceService(builder.Configuration);
            builder.Services.AddHttpClientService(builder.Configuration);
            builder.Services.AddLocalization();

            // ע��nlog��־����
            builder.AddNLogWeb(builder.Configuration);
            AddAutoMapper();
            var app = builder.Build();
            //https://learn.microsoft.com/zh-cn/aspnet/core/host-and-deploy/linux-nginx?view=aspnetcore-6.0&tabs=linux-ubuntu
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });
#if DEBUG
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
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
#if DEBUG
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
                        Name = "����",
                        Url = new Uri("https://www.hymson.com/")
                    },
                    License = new OpenApiLicense
                    {
                        Name = "��Ŀ�Ǽ���",
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

                // ��header �����token�����ݵ���̨
                options.OperationFilter<SecurityRequirementsOperationFilter>();
                // JwtBearerDefaults.AuthenticationScheme
                options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "ǰ��Bearer��ʾ����Bearer {Token}",
                    Name = "Authorization",//jwtĬ�ϵĲ�������,
                    Type = SecuritySchemeType.ApiKey, //ָ��ApiKey
                    BearerFormat = "JWT",//��ʶ�������Ƶĸ�ʽ ����Ϣ��Ҫ�ǳ����ĵ�Ŀ��
                    Scheme = JwtBearerDefaults.AuthenticationScheme//��Ȩ��Ҫʹ�õ�HTTP��Ȩ����������
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