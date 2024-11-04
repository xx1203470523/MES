using Hymson.MES.EquipmentServices.Services.SfcCirculation;
using Hymson.MES.EquipmentServices.Dtos.SfcCirculation;
using Hymson.MES.EquipmentServicesTests;
using Hymson.MES.Services.Services.Equipment.EquEquipment;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Hymson.Authentication.JwtBearer;

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
            var bindCCSLocationDto = await _sfcCirculationService.GetBindCCSLocationAsync("YTLSM202310150292A");
            Assert.IsTrue(!string.IsNullOrEmpty(bindCCSLocationDto.CurrentLocation));
        }
        /// <summary>
        /// 测试CCS绑定A
        /// </summary>
        /// <returns></returns>
        [TestMethod()]
        public async Task SfcCirculationBindAsyncTest()
        {
            string equipmentCode = "YTLPACK01AE014";
            //string equipmentCode = "YTLPACK01AE023";
            await SetEquInfoAsync(equipmentCode);

            SfcCirculationBindDto sfcCirculationCCS = new()
            {
                EquipmentCode = "YTLPACK01AE014",
                ResourceCode = "YTLPACK01ER014",
                SFC = "MODULE20241028004",
                ModelCode = "",
                BindSFCs = new CirculationBindDto[] {
                    new(){Location="0",SFC= "CELL20241028003",Name= null},
                    new(){ Location="1",SFC="CELL20241028004",Name =null }
                },
                LocalTime = DateTime.Now
            };


            //SfcCirculationBindDto sfcCirculationCCS = new SfcCirculationBindDto()
            //{
            //    SFC = "TESTM001",
            //    ModelCode = "12S",  //ModelCode
            //    //IsVirtualSFC=false,
            //    BindSFCs = new CirculationBindDto[] {
            //        //new CirculationBindDto{
            //        //    SFC="YTC20240620003",
            //        //    Location="0",
            //        //    //Name="CCS01"
            //        //},

            //        new CirculationBindDto{
            //            SFC="TESTSFC01",
            //            Location="0",
            //            //Name="CCS01"
            //        },

            //        //new CirculationBindDto{
            //        //    SFC="YTM20240620003",
            //        //    Location="1",
            //        //    //Name="CCS01"
            //        //}
            //    },
            //    EquipmentCode = "YTLPACK01AE014",
            //    ResourceCode = "YTLPACK01ER014",

            //    //EquipmentCode = "YTLPACK01AE023",
            //    //ResourceCode = "YTLPACK01ER023",
            //    LocalTime = DateTime.Now
            //};
            await _sfcCirculationService.SfcCirculationBindAsync(sfcCirculationCCS);
            Assert.IsTrue(true);
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
            string equipmentCode = "YTLPACK01AE023";
            await SetEquInfoAsync(equipmentCode);
            SfcCirculationCCSBindDto sfcCirculationCCS = new SfcCirculationCCSBindDto()
            {
                ResourceCode = "YTLPACK01ER023",
                SFC = "ES01340010000000572409180096",
                ModelCode = "",
                BindSFCs = new CirculationCCSDto[] {
                    new(){Location="1",SFC= "YTLSM202409230258A",Name= null}
                    ,new(){Location="2",SFC="YTLSM202409230256A",Name=null}
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
            string equipmentCode = "YTLPACK01AE018";
            await SetEquInfoAsync(equipmentCode);
            try
            {
                SfcCirculationCCSNgSetDto sfcCirculationCCSNgSetDto = new SfcCirculationCCSNgSetDto
                {
                    ResourceCode = "YTLPACK01ER018",
                    SFC = "YTLSM202309270168A",
                    Locations = new string[] { "C" }
                    //BindSFCs = new string[] { "CCS0002" }
                };
                await _sfcCirculationService.SfcCirculationCCSNgSetAsync(sfcCirculationCCSNgSetDto);

            }
            catch (Exception ex)
            {
                int ss = 1;
                throw;
            }
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

        /// <summary>
        /// 测试模组条码查询信息
        /// </summary>
        /// <returns></returns>
        [TestMethod()]
        public async Task GetCirculationModuleCCSInfoTest()
        {
            string equipmentCode = "YTLPACK01AE019";
            try
            {
                await SetEquInfoAsync(equipmentCode);
                var moduleCCSInfoDto = await _sfcCirculationService.GetCirculationModuleCCSInfoAsync("YTLSM202309140004A");
                Assert.IsTrue(moduleCCSInfoDto != null);
            }
            catch (Exception ex)
            {
                int s = 1;
                throw;
            }

        }

        /// <summary>
        /// 测试模组条码查询信息
        /// </summary>
        /// <returns></returns>
        [TestMethod()]
        public async Task GetCirculationModuleCCSInfoAsyncTest()
        {
            //        return await _sfcCirculationService.GetCirculationModuleCCSInfoAsync(sfc);
            string equipmentCode = "YTLPACK01VIR06";
            try
            {
                await SetEquInfoAsync(equipmentCode);
                var moduleCCSInfoDto = await _sfcCirculationService.GetCirculationModuleCCSInfoAsync("21AsakiA4A4");
                Assert.IsTrue(moduleCCSInfoDto != null);
            }
            catch (Exception ex)
            {
                int s = 1;
                throw;
            }

        }
    }
}