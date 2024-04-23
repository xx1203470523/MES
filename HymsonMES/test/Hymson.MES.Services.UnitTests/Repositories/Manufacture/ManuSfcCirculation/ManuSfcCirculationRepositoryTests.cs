using NUnit.Framework;
using Hymson.MES.Data.Repositories.Manufacture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hymson.Localization.Services;
using Hymson.MES.CoreServices.Services.Manufacture.ManuBind;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Hymson.Infrastructure.Mapper;
using Hymson.Infrastructure;
using AutoMapper;
using Hymson.MES.Core.Domain.Manufacture;

namespace Hymson.MES.Data.Repositories.Manufacture.Tests
{
    [TestFixture()]
    public class ManuSfcCirculationRepositoryTests
    {
        ServiceCollection services = new ServiceCollection();
        IManuSfcCirculationRepository _service;
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
            _service = provider.GetRequiredService<IManuSfcCirculationRepository>();

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
        [Test()]
        public void InsertRangeAsyncTest()
        {
            //List<ManuSfcCirculationEntity> manuSfcCirculationEntitys=new List<ManuSfcCirculationEntity>();
            //manuSfcCirculationEntitys.Add(new ManuSfcCirculationEntity()
            //{
            //    SFC = "wxk",
            //    SiteId = 123456,
            //    CirculationBarCode= "12888510782521347",
            //    SubstituteId=0,
            //    CirculationMainSupplierId=0,
            //    CirculationMainProductId=0,
            //    CirculationProductId= 12870595507134464,
            //    CirculationQty= 1,
            //    CirculationType = Core.Enums.Manufacture.SfcCirculationTypeEnum.
            //})
           // _service.InsertRangeAsync(new );
            Assert.Fail();
        }
    }
}