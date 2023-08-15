using Hymson.MES.SystemServicesTests;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Hymson.MES.SystemServices.Services.Manufacture.Tests
{
    [TestClass()]
    public class ManuSfcCirculationServiceTests : BaseTest
    {
        private readonly IManuSfcCirculationService _manuSfcCirculationService;
        public ManuSfcCirculationServiceTests()
        {
            _manuSfcCirculationService = ServiceProvider.GetRequiredService<IManuSfcCirculationService>();
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
                {"SiteName","单元测试站点"},
                {"Name","单元测试系统"},
                {"Id",0}
            };
            CurrentSystemInfo.AddUpdate(siteInfo);
        }
        /// <summary>
        /// 测试追溯层级查询
        /// </summary>
        /// <returns></returns>
        [TestMethod()]
        public async Task GetRelationShipByPackAsyncTest()
        {
            var manuSfcCirculation = await _manuSfcCirculationService.GetRelationShipByPackAsync("P2308140001");
            Assert.IsTrue(manuSfcCirculation != null);
        }

    }
}