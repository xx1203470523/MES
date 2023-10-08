using Hymson.MES.Services.Dtos.Report;
using Hymson.MES.Services.Services.Report;
using Hymson.MES.SystemServices.Dtos.Manufacture;
using Hymson.MES.SystemServicesTests;
using Hymson.Minio;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Hymson.MES.SystemServices.Services.Manufacture.Tests
{
    [TestClass()]
    public class ManuSfcCirculationServiceTests : BaseTest
    {
        private readonly IManuSfcCirculationService _manuSfcCirculationService;
        private readonly IProductTraceReportService _productTraceReportService;

        public ManuSfcCirculationServiceTests()
        {
            _manuSfcCirculationService = ServiceProvider.GetRequiredService<IManuSfcCirculationService>();
            //var _iMinioService = ServiceProvider.GetRequiredService<IMinioService>();
            _productTraceReportService = ServiceProvider.GetRequiredService<IProductTraceReportService>();
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
        /// <summary>
        /// 测试追溯层级查询
        /// </summary>
        /// <returns></returns>
        [TestMethod()]
        public async Task GetSfcStepPagedListAsyncTest()
        {
            ManuSfcStepPagedQueryDto param = new ManuSfcStepPagedQueryDto()
            {
                SFC = "121AsakiA4A4",
                PageSize = 2000,
                PageIndex = 1
            };
            var manuSfcCirculation = await _productTraceReportService.GetSfcStepPagedListAsync(param);
            Assert.IsTrue(manuSfcCirculation != null);
        }
    }
}