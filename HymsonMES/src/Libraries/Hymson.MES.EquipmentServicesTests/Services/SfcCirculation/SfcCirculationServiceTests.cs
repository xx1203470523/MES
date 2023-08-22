using Hymson.MES.EquipmentServices.Services.SfcCirculation;
using Hymson.MES.EquipmentServices.Dtos.SfcCirculation;
using Hymson.MES.EquipmentServicesTests;
using Hymson.MES.Services.Services.Equipment.EquEquipment;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Hymson.MES.EquipmentServices.Services.SfcCirculation.Tests
{
    [TestClass()]
    public class SfcCirculationServiceTests : BaseTest
    {
        private readonly ISfcCirculationService _sfcCirculationService;
        private readonly IEquEquipmentService _equEquipmentService;
        public SfcCirculationServiceTests()
        {
            _sfcCirculationService = ServiceProvider.GetRequiredService<ISfcCirculationService>();
            _equEquipmentService = ServiceProvider.GetRequiredService<IEquEquipmentService>();
        }

        [TestInitialize]
        public void TestInitialize()
        {
            //设置测试Site信息
            Dictionary<string, object> siteInfo = new()
            {
                { "SiteId", 123456 },
                {"SiteName","单元测试站点"}
            };
            CurrentEquipmentInfo.AddUpdate(siteInfo);
        }

        /// <summary>
        /// 设置当前测试设备信息
        /// </summary>
        /// <param name="equIpmentCode"></param>
        public async Task SetEquInfoAsync(string equIpmentCode)
        {
            var equEquipment = await _equEquipmentService.GetByEquipmentEntityCodeAsync(equIpmentCode) ?? throw new Exception($"设备编码不存在:{equIpmentCode}");
            Dictionary<string, object> equDic = new()
            {
                { "Id", equEquipment.Id },
                { "FactoryId", equEquipment.WorkCenterFactoryId },
                { "Code", equEquipment.EquipmentCode },
                { "Name", equEquipment.EquipmentName }
            };
            CurrentEquipmentInfo.AddUpdate(equDic);
        }

        /// <summary>
        /// 测试获取当前待绑定CCS
        /// </summary>
        /// <returns></returns>
        [TestMethod()]
        public async Task GetBindCCSLocationAsyncTest()
        {
            var bindCCSLocationDto = await _sfcCirculationService.GetBindCCSLocationAsync("AAATESTSFC2308091");
            Assert.IsTrue(!string.IsNullOrEmpty(bindCCSLocationDto.CurrentLocation));
        }

        /// <summary>
        /// 测试CCS绑定A
        /// </summary>
        /// <returns></returns>
        [TestMethod()]
        public async Task SfcCirculationCCSBindAsyncTest()
        {
            string equipmentCode = "QAEM002";
            await SetEquInfoAsync(equipmentCode);
            SfcCirculationCCSBindDto sfcCirculationCCS = new SfcCirculationCCSBindDto()
            {
                SFC = "AAATESTSFC2308091",
                BindSFCs = new CirculationCCSDto[] {
                    new CirculationCCSDto{
                        SFC="CCS0001",
                        Location="A",
                        Name="CCS01"
                    }
                }
            };
            await _sfcCirculationService.SfcCirculationCCSBindAsync(sfcCirculationCCS);
            Assert.IsTrue(true);
        }

        /// <summary>
        /// 测试CCS绑定B
        /// </summary>
        /// <returns></returns>
        [TestMethod()]
        public async Task SfcCirculationCCSBindAsyncTest2()
        {
            string equipmentCode = "QAEM002";
            await SetEquInfoAsync(equipmentCode);
            SfcCirculationCCSBindDto sfcCirculationCCS = new SfcCirculationCCSBindDto()
            {
                ResourceCode = "QAEMZY002",
                SFC = "AAATESTSFC2308091",
                ModelCode="TEST",
                BindSFCs = new CirculationCCSDto[] {
                    new CirculationCCSDto{
                        SFC="CCS0002",
                        Location="B",
                        Name="CCS02"
                    }
                }
            };
            await _sfcCirculationService.SfcCirculationCCSBindAsync(sfcCirculationCCS);
            Assert.IsTrue(true);
        }

        /// <summary>
        /// 测试解绑CCS
        /// </summary>
        [TestMethod()]
        public async Task SfcCirculationCCSUnBindAsyncTest()
        {
            string equipmentCode = "QAEM002";
            await SetEquInfoAsync(equipmentCode);
            SfcCirculationCCSUnBindDto circulationCCSUnBindDto = new SfcCirculationCCSUnBindDto
            {
                ResourceCode = "QAEMZY002",
                SFC = "AAATESTSFC2308091",
                UnBindSFCs = new string[] { "CCS0004" }
            };
            await _sfcCirculationService.SfcCirculationCCSUnBindAsync(circulationCCSUnBindDto);
            Assert.IsTrue(true);
        }

        /// <summary>
        /// CCS设置NG测试
        /// </summary>
        [TestMethod()]
        public async Task SfcCirculationCCSNgSetAsyncTest()
        {
            string equipmentCode = "QAEM002";
            await SetEquInfoAsync(equipmentCode);
            SfcCirculationCCSNgSetDto sfcCirculationCCSNgSetDto = new SfcCirculationCCSNgSetDto
            {
                ResourceCode = "QAEMZY002",
                SFC = "AAATESTSFC2308091",
                Locations = new string[] { "B" }
                //BindSFCs = new string[] { "CCS0002" }
            };
            await _sfcCirculationService.SfcCirculationCCSNgSetAsync(sfcCirculationCCSNgSetDto);
            Assert.IsTrue(true);
        }

        /// <summary>
        /// CCS状态确认测试
        /// </summary>
        [TestMethod()]
        public async Task SfcCirculationCCSConfirmAsyncTest()
        {
            string equipmentCode = "QAEM002";
            await SetEquInfoAsync(equipmentCode);
            SfcCirculationCCSConfirmDto sfcCirculationCCSConfirmDto = new SfcCirculationCCSConfirmDto
            {
                ResourceCode = "QAEMZY002",
                SFC = "AAATESTSFC2308091",
                Location = "A"
            };
            await _sfcCirculationService.SfcCirculationCCSConfirmAsync(sfcCirculationCCSConfirmDto);
            Assert.IsTrue(true);
        }

        /// <summary>
        /// CCS状态确认测试
        /// </summary>
        [TestMethod()]
        public async Task SfcCirculationCCSConfirmAsyncTest2()
        {
            string equipmentCode = "QAEM002";
            await SetEquInfoAsync(equipmentCode);
            SfcCirculationCCSConfirmDto sfcCirculationCCSConfirmDto = new SfcCirculationCCSConfirmDto
            {
                ResourceCode = "QAEMZY002",
                Location = "B"
            };
            await _sfcCirculationService.SfcCirculationCCSConfirmAsync(sfcCirculationCCSConfirmDto);
            Assert.IsTrue(true);
        }

        /// <summary>
        /// 测试绑定条码获取
        /// </summary>
        [TestMethod()]
        public async Task GetCirculationBindSfcsAsyncTest()
        {
            string equipmentCode = "QAEM002";
            await SetEquInfoAsync(equipmentCode);
            var circulationBinds = await _sfcCirculationService.GetCirculationBindSfcsAsync("AAA2103841XCE12A023");
            Assert.IsTrue(circulationBinds.Any());
        }
    }
}