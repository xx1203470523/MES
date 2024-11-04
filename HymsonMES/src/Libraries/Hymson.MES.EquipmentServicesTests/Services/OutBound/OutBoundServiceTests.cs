using Hymson.MES.EquipmentServices.Dtos.OutBound;
using Hymson.MES.EquipmentServicesTests;
using Hymson.MES.Services.Services.Equipment.EquEquipment;
using Hymson.Utils;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Hymson.MES.EquipmentServices.Services.OutBound.Tests
{
    [TestClass()]
    public class OutBoundServiceTests : BaseTest
    {
        private readonly IOutBoundService _outBoundService;
        private readonly IEquEquipmentService _equEquipmentService;
        public OutBoundServiceTests()
        {
            _outBoundService = ServiceProvider.GetRequiredService<IOutBoundService>();
            _equEquipmentService = ServiceProvider.GetRequiredService<IEquEquipmentService>();
        }

        [TestCleanup]
        public void TestCleanup()
        {
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
        /// 出站测试
        /// </summary>
        [TestMethod()]
        public async Task OutBoundTestAsync()
        {

            //string request = "{\"SFC\":\"ES01B20020048202309220052\",\"Passed\":1,\"ParamList\":null,\"BindFeedingCodes\":null,\"NG\":null,\"IsPassingStation\":true,\"ResourceCode\":\"YTLPACK01ER025\",\"EquipmentCode\":\"YTLPACK01AE025\",\"LocalTime\":\"2023-09-27T08:37:47\"}";
            ////电芯OCV
            //string resourceCode = "YTLPACK01ER004";
            //string equipmentCode = "YTLPACK01AE004";

            ////激光刻码
            //string resourceCode = "YTLPACK01ER014";
            //string equipmentCode = "YTLPACK01AE014";

            //CCS安装
            string resourceCode = "YTLPACK01ER016";
            string equipmentCode = "YTLPACK01AE016";

            ////CCS焊接
            //string resourceCode = "YTLPACK01ER017";
            //string equipmentCode = "YTLPACK01AE017";

            ////模组OCV
            //string resourceCode = "YTLPACK01ER018";
            //string equipmentCode = "YTLPACK01AE018";

            ////焊缝清洗
            //string resourceCode = "YTLPACK01ER026";
            //string equipmentCode = "YTLPACK01VIR01";

            ////气密性
            //string resourceCode = "YTLPACK01ER021";
            //string equipmentCode = "YTLPACK01AE021";

            ////涂胶
            //string resourceCode = "YTLPACK01ER022";
            //string equipmentCode = "YTLPACK01AE022";

            List<OutBoundParam> outBoundParams = new List<OutBoundParam>();
            for (int i = 1; i <= 1; i++)
            {
                outBoundParams.Add(new OutBoundParam
                {
                    ParamCode = "单元测试出站参数" + i,
                    ParamValue = i.ToString()
                });
            }

            await SetEquInfoAsync(equipmentCode);
            await _outBoundService.OutBoundAsync(new OutBoundDto
            {
                ResourceCode = resourceCode,
                Passed = 1,
                LocalTime = HymsonClock.Now(),
                //SFC = "CELL20241024002",
                SFC = "YTLSM202410280084A",
                ParamList = outBoundParams.ToArray(),

                //过站测试
                IsPassingStation = true
            });

            Assert.IsTrue(true);
        }

        /// <summary>
        /// 多条出站测试
        /// </summary>
        /// <returns></returns>
        [TestMethod()]
        public async Task OutBoundMoreTestAsync()
        {
            string resourceCode = "YTLPACK01AE025";
            string equipmentCode = "YTLPACK01ER025";
            string prefix = "ES01340010000000442407120171";

            List<OutBoundParam> outBoundParams = new List<OutBoundParam>();
            for (int i = 1; i <= 50; i++)
            {
                outBoundParams.Add(new OutBoundParam
                {
                    ParamCode = "单元测试出站参数" + i,
                    ParamValue = i.ToString()
                });
            }
            List<OutBoundSFCDto> sfcs = new List<OutBoundSFCDto>();
            for (int i = 0; i < 10; i++)
            {
                sfcs.Add(new OutBoundSFCDto
                {
                    SFC = prefix + i,
                    Passed = i,
                    ParamList = outBoundParams.ToArray()
                });
            }
            await SetEquInfoAsync(equipmentCode);
            await _outBoundService.OutBoundMoreAsync(new OutBoundMoreDto
            {
                ResourceCode = resourceCode,
                LocalTime = HymsonClock.Now(),
                SFCs = sfcs.ToArray()

            });
            Assert.IsTrue(true);
        }

        /// <summary>
        /// 出站测试
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task OutBoundTestAsync2()
        {
            string resourceCode = "YTLPACK01ER031";
            string equipmentCode = "YTLPACK01VIR06";
            string prefix = "ES01340010000000442407120182";
            List<OutBoundParam> param = new();

            OutBoundSFCDto sFCDto = new OutBoundSFCDto()
            {
                SFC = prefix,
                Passed = 1,
                ParamList = param.ToArray(),
                IsPassingStation = true
            };

            await SetEquInfoAsync(equipmentCode);
            await _outBoundService.OutBoundMoreAsync(new OutBoundMoreDto
            {
                ResourceCode = resourceCode,
                LocalTime = HymsonClock.Now(),
                SFCs = new OutBoundSFCDto[] { sFCDto }

            });
            Assert.IsTrue(true);
        }
    }
}