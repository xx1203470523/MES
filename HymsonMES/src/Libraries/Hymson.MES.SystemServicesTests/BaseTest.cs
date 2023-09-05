using FluentValidation;
using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure.Mapper;
using Hymson.Infrastructure;
using Hymson.MES.CoreServices.DependencyInjection;
using Hymson.MES.Data.Options;
using Hymson.MES.SystemServices.Dtos.Manufacture;
using Hymson.MES.SystemServices.Dtos.Plan;
using Hymson.MES.SystemServices.Services.Manufacture;
using Hymson.MES.SystemServices.Validators.Manufacture;
using Hymson.MES.SystemServices.Validators.Plan;
using Hymson.MES.SystemServicesTests.Dtos;
using Hymson.Utils;
using Hymson.Web.Framework.WorkContext;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using AutoMapper;
using Hymson.MES.Services.Services.Report;

namespace Hymson.MES.SystemServicesTests
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
                .Configure<ConnectionOptions>(Configuration)
                .AddMemoryCache()
                .AddJwtBearerService(Configuration)
                .AddCoreService(Configuration)
                .AddData(Configuration)
                .AddSqlLocalization(Configuration)
                .AddSequenceService(Configuration)
                .AddLocalization()
                .AddSingleton<ICurrentSystem, TestCurrentSystem>()
                .AddSingleton<ICurrentUser, TestCurrentUser>()//测试所用CurrentUser服务
                .AddSingleton<ICurrentSite, TestCurrentSite>()//测试所用CurrentSite服务
                .AddSingleton<IManuSfcCirculationService, ManuSfcCirculationService>()//条码流转服务
                .AddSingleton<IProductTraceReportService, ProductTraceReportService>()//
                .AddSingleton<AbstractValidator<PlanWorkOrderDto>, PlanWorkOrderValidator>()
                .AddSingleton<AbstractValidator<ManuSfcCirculationDto>, ManuSfcCirculationValidator>()
                .BuildServiceProvider();
            AddAutoMapper();//这个测试使用到了AutoMapper
            ConnectionOptions = ServiceProvider.GetRequiredService<IOptions<ConnectionOptions>>().Value;
        }

        /// <summary>
        /// 设置当前测试系统信息
        /// </summary>
        /// <param name="equipmentInfoDto"></param>
        public static void SetSysInfoAsync(SystemInfoDto systemInfoDto)
        {
            //所以必须先设置站点Id
            var siteId = CurrentSystemInfo.SystemDic.Value["SiteId"].ParseToLong();
            Dictionary<string, object> equDic = new()
            {
                { "Id", systemInfoDto.Id },
                { "FactoryId", systemInfoDto.FactoryId },
                { "Name", systemInfoDto.Name }
            };
            CurrentSystemInfo.AddUpdate(equDic);
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
                .OrderBy(mapperConfiguration => mapperConfiguration?.Order);

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
