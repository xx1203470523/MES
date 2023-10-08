using FluentValidation;
using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Excel;
using Hymson.Excel.Abstractions;
using Hymson.MES.CoreServices.DependencyInjection;
using Hymson.MES.Data.Options;
using Hymson.MES.EquipmentServices.Dtos.InBound;
using Hymson.MES.EquipmentServices.Dtos.OutBound;
using Hymson.MES.EquipmentServices.Dtos.SfcCirculation;
using Hymson.MES.EquipmentServices.Services.EquipmentCollect;
using Hymson.MES.EquipmentServices.Services.InBound;
using Hymson.MES.EquipmentServices.Services.OutBound;
using Hymson.MES.EquipmentServices.Services.SfcCirculation;
using Hymson.MES.EquipmentServices.Validators.InBound;
using Hymson.MES.EquipmentServices.Validators.OutBound;
using Hymson.MES.EquipmentServices.Validators.SfcCirculation;


using Hymson.MES.EquipmentServicesTests.Dtos;
using Hymson.MES.Services.Dtos.Equipment;
using Hymson.MES.Services.Dtos.Integrated;
using Hymson.MES.Services.Services.Equipment.EquEquipment;
using Hymson.MES.Services.Services.Integrated.InteSFCBox;
using Hymson.MES.Services.Validators.Process;
using Hymson.MES.Services.Validators.Integrated;

using Hymson.Utils;
using Hymson.Web.Framework.WorkContext;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Hymson.Minio;

namespace Hymson.MES.EquipmentServicesTests
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
                //.AddSingleton<ICurrentEquipment, CurrentEquipment>()//不能直接注入原有CurrentEquipment因为没在HttpContextAccessor层面模拟设备信息
                .AddSingleton<ICurrentEquipment, TestCurrentEquipment>()//测试所用CurrentEquipment服务，修改CurrentEquipmentInfo模拟设备信息
                .AddSingleton<ICurrentUser, TestCurrentUser>()//测试所用CurrentUser服务，用于模拟当前操作用户信息
                .AddSingleton<ICurrentSite, TestCurrentSite>()//测试所用CurrentSite服务，修改CurrentEquipmentInfo总SiteId和SiteName模拟信息
                .AddSingleton<IInBoundService, InBoundService>()//进站
                .AddSingleton<IOutBoundService, OutBoundService>()//出站
                .AddSingleton<IEquipmentCollectService, EquipmentCollectService>()//设备信息收集服务
                .AddSingleton<AbstractValidator<InBoundDto>, InBoundValidator>()//进站
                .AddSingleton<AbstractValidator<InBoundMoreDto>, InBoundMoreValidator>()//进站（多个） 便于测试需要将InBoundMoreValidator访问修饰符修改为public
                .AddSingleton<AbstractValidator<OutBoundDto>, OutBoundValidator>()//出站
                .AddSingleton<AbstractValidator<OutBoundMoreDto>, OutBoundMoreValidator>()//出站（多个）
                .AddSingleton<IEquEquipmentService, EquEquipmentService>()//注入使用到的其他服务
                .AddSingleton<AbstractValidator<EquEquipmentSaveDto>, EquEquipmentValidator>()
                .AddSingleton<ISfcCirculationService, SfcCirculationService>()
                .AddSingleton<IInteSFCBoxService, InteSFCBoxService>()
                .AddSingleton<IExcelService, ExcelService>()
                .AddSingleton<IMinioService, MinioService>()
                 .AddSingleton<AbstractValidator<InteSFCBoxImportDto>, InteSFCBoxValidator>()
                .AddSingleton<AbstractValidator<SfcCirculationBindDto>, SfcCirculationBindValidator>()//条码流转绑定
                .AddSingleton<AbstractValidator<SfcCirculationUnBindDto>, SfcCirculationUnBindValidator>()//条码流转解绑
                .BuildServiceProvider();

            ConnectionOptions = ServiceProvider.GetRequiredService<IOptions<ConnectionOptions>>().Value;
        }

        /// <summary>
        /// 设置当前测试设备信息
        /// </summary>
        /// <param name="equipmentInfoDto"></param>
        public static void SetEquInfoAsync(EquipmentInfoDto  equipmentInfoDto)
        {
            //所以必须先设置站点Id
            var siteId = CurrentEquipmentInfo.EquipmentInfoDic.Value["SiteId"].ParseToLong();
            Dictionary<string, object> equDic = new()
            {
                { "Id", equipmentInfoDto.Id },
                { "FactoryId", equipmentInfoDto.FactoryId },
                { "Code", equipmentInfoDto.Code },
                { "Name", equipmentInfoDto.Name }
            };
            CurrentEquipmentInfo.AddUpdate(equDic);
        }
    }
}
