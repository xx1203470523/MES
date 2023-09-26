using Hymson.MES.EquipmentServices.Services.OutBound;
using Hymson.MES.EquipmentServices.Services.SfcCirculation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Hymson.MES.EquipmentServicesTests.Services.PDA
{
    [TestClass()]
    public class NGCopeWithTest : BaseTest
    {
        private readonly ISfcCirculationService _sfcCirculationService;
        public NGCopeWithTest()
        {
            _sfcCirculationService = ServiceProvider.GetRequiredService<ISfcCirculationService>();
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
        /// 获取补料的NG列表
        /// </summary>
        /// <returns></returns>
        [TestMethod()]
        public async Task GetReplenishNGDataTest()
        {
            string sfc = string.Empty;
            await _sfcCirculationService.GetReplenishNGDataAsync();
            Assert.IsTrue(true);
        }

        /// <summary>
        /// 补料确认
        /// </summary>
        /// <returns></returns>
        [TestMethod()]
        public async Task ReplenishNGConfirmTest()
        {
            string sfc = "AAA2308071851001";
            await _sfcCirculationService.ReplenishNGConfirmAsync(sfc);
            Assert.IsTrue(true);
        }
    }
}
