
using Hymson.Infrastructure.Mapper;
using Hymson.Infrastructure;
using Hymson.Localization.Services;
using Hymson.MES.CoreServices.DependencyInjection;



using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Configuration;
using AutoMapper;
using Hymson.MES.Api;

namespace Hymson.MES.CoreServices.Services.Manufacture.ManuBind.Tests
{
    [TestFixture()]
    public class ManuMergeServiceTests
    {
        ServiceCollection services = new ServiceCollection();
        IManuMergeService _service;
        ILocalizationService _localizationService;
        IConfiguration _configuration;
        [SetUp]
        public void Setup()
        {
           
            var cb = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.Staging.json"); 
            
            _configuration = cb.Build();
           // services.AddEndpointsApiExplorer();
            services.AddMemoryCache();
            //services.AddClearCacheService(_configuration);
            //services.AddHostedService<WorkService>();

           // services.AddJwtBearerService(_configuration);
            services.AddAppService(_configuration);
            services.AddSqlLocalization(_configuration);
            services.AddHttpClientService(_configuration);
            //services.AddEventBusRabbitMQService(_configuration);
            services.AddLocalization();
            AddAutoMapper();
          
            var provider = services.BuildServiceProvider();
            _localizationService = provider.GetService<ILocalizationService>();
            _service = provider.GetRequiredService<IManuMergeService>();

        }
        [Test()]
        public void MergeAsyncTest()
        {
            var result = _service.MergeAsync(new Dtos.Manufacture.ManuBind.ManuMergeRequestDto
            {
                Barcodes = new[] { "AMF1E041801138", "CMF2E041802123" },
                SiteId = 39612349211041792,
               // ProcedureId = 12816809150152704,
                //TargetSFC = "hahaha"
            });
            result.Wait();
            Assert.Fail();
        }
        public static void AddAutoMapper()
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