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
using Hymson.MES.CoreServices.Bos.Job;
using Hymson.MES.Data.Repositories.Manufacture;

namespace Hymson.MES.CoreServices.Services.Manufacture.Tests
{
    [TestFixture()]
    public class ManuDegradedProductExtendServiceTests
    {

        ServiceCollection services = new ServiceCollection();
        IManuDegradedProductExtendService _service;
        ILocalizationService _localizationService;
        IConfiguration _configuration;
        /// <summary>
        /// 仓储接口（降级录入）
        /// </summary>
        private  IManuDowngradingRepository _manuDowngradingRepository;

        /// <summary>
        /// 仓储接口（降级品录入记录）
        /// </summary>
        private  IManuDowngradingRecordRepository _manuDowngradingRecordRepository;
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

            _manuDowngradingRepository = provider.GetRequiredService<IManuDowngradingRepository>();
            _manuDowngradingRecordRepository = provider.GetRequiredService<IManuDowngradingRecordRepository>();

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
                SiteId = 39612349211041792,
                UserName = "Test",
                KeyValues = {new Bos.Manufacture.DegradedProductExtendKeyValueBo
                    {
                        SFC="AYJ1E0603001",
                        BarCode= "AZJ1E0603001"
                    },
                    new Bos.Manufacture.DegradedProductExtendKeyValueBo
                    {
                        SFC="AYJ1E0603001",
                        BarCode= "AZJ1E0603001"
                    },
                    new Bos.Manufacture.DegradedProductExtendKeyValueBo
                    {
                        SFC="AYJ1E0603002",
                        BarCode= "AZJ1E0603001"
                    },
                    // new Bos.Manufacture.DegradedProductExtendKeyValueBo
                    //{
                    //    SFC="wxk",
                    //    BarCode= "dx231222662"
                    //}
                }
            });
            var task = _service.GetManuDowngradingsByConsumesAsync(new Bos.Manufacture.DegradedProductExtendBo
            {
                SiteId = 39612349211041792,
                UserName = "Test",
                KeyValues = new List<Bos.Manufacture.DegradedProductExtendKeyValueBo>()
                {
                    new Bos.Manufacture.DegradedProductExtendKeyValueBo
                    {
                        SFC="AYJ1E0603001",
                        BarCode= "AZJ1E0603001"
                    },
                    new Bos.Manufacture.DegradedProductExtendKeyValueBo
                    {
                         SFC="AYJ1E0603001",
                        BarCode= "AZJ1E0603001"
                    },
                    new Bos.Manufacture.DegradedProductExtendKeyValueBo
                    {
                        SFC="AYJ1E0603002",
                        BarCode= "AZJ1E0603001"
                    },
                    // new Bos.Manufacture.DegradedProductExtendKeyValueBo
                    //{
                    //     SFC="AYJ1E0603002",
                    //    BarCode= "AZJ1E0603001"
                    //}
                }
            }, lst1.Result);
            var responseBo = new OutStationResponseBo();
            responseBo.DowngradingEntities = task.Result.Item1;
            responseBo.DowngradingRecordEntities = task.Result.Item2;
            var responseBos = new List<OutStationResponseBo>();
            responseBos.Add(responseBo);
            var r1 = responseBos.Where(w => w.DowngradingRecordEntities != null).SelectMany(s => s.DowngradingEntities);
            var r2 = responseBos.Where(w => w.DowngradingRecordEntities != null).SelectMany(s => s.DowngradingRecordEntities);

            int i = _manuDowngradingRepository.InsertsAsync(r1).Result;
            int j = _manuDowngradingRecordRepository.InsertsAsync(r2).Result;

        }
    }
}