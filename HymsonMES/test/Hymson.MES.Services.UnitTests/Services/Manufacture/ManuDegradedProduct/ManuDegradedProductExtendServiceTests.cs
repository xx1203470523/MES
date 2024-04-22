using NUnit.Framework;
using Hymson.MES.CoreServices.Services.Manufacture;
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

namespace Hymson.MES.CoreServices.Services.Manufacture.Tests
{
    [TestFixture()]
    public class ManuDegradedProductExtendServiceTests
    {

        ServiceCollection services = new ServiceCollection();
        IManuDegradedProductExtendService _service;
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
            _service = provider.GetRequiredService<IManuDegradedProductExtendService>();

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
        public  void GetManuDowngradingsByConsumesAsyncTest()
        {
            var lst1 =  _service.GetManuDownGradingsAsync(new Bos.Manufacture.DegradedProductExtendBo
            {
                SiteId = 30654441397841920,
                UserName = "Test",
                KeyValues = {new Bos.Manufacture.DegradedProductExtendKeyValueBo
                    {
                        SFC="wxk",
                        BarCode= "dx231218647"
                    },
                    new Bos.Manufacture.DegradedProductExtendKeyValueBo
                    {
                        SFC="wxk",
                        BarCode= "dx231220652"
                    },
                    new Bos.Manufacture.DegradedProductExtendKeyValueBo
                    {
                        SFC="wxk",
                        BarCode= "dx231221657"
                    },
                     new Bos.Manufacture.DegradedProductExtendKeyValueBo
                    {
                        SFC="wxk",
                        BarCode= "dx231222662"
                    }
                }
            });
            var task = _service.GetManuDowngradingsByConsumesAsync(new Bos.Manufacture.DegradedProductExtendBo
            {
                SiteId = 30654441397841920,
                UserName = "wxk",
                KeyValues = new List<Bos.Manufacture.DegradedProductExtendKeyValueBo>()
                {
                    new Bos.Manufacture.DegradedProductExtendKeyValueBo
                    {
                        SFC="wxk",
                        BarCode= "dx231218647"
                    },
                    new Bos.Manufacture.DegradedProductExtendKeyValueBo
                    {
                        SFC="wxk",
                        BarCode= "dx231220652"
                    },
                    new Bos.Manufacture.DegradedProductExtendKeyValueBo
                    {
                        SFC="wxk",
                        BarCode= "dx231221657"
                    },
                     new Bos.Manufacture.DegradedProductExtendKeyValueBo
                    {
                        SFC="wxk",
                        BarCode= "dx231222662"
                    }
                }
            }, lst1.Result);
        }
    }
}