using Hymson.MES.Core.Enums;
using Hymson.MES.SystemServices.Dtos.Manufacture;
using Hymson.MES.SystemServices.Dtos.ProductTraceReport.Query;
using Hymson.MES.SystemServices.Services.ProductTrace;
using Hymson.MES.SystemServicesTests;
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
            var manuSfcCirculation = await _manuSfcCirculationService.GetRelationShipByPackAsync("123");
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
                SFC = "YTLSM202309130001A",
                PageSize = 2000,
                PageIndex = 1
            };
            var manuSfcCirculation = await _productTraceReportService.GetSfcStepPagedListAsync(param);
            Assert.IsTrue(manuSfcCirculation != null);
        }

        /// <summary>
        /// 按成品条码/模组条码查询参数信息
        /// </summary>
        /// <returns></returns>
        [TestMethod()]
        public async Task GetProductPrameterPagedListAsyncTest()
        {
            ManuProductPrameterPagedQueryDto param = new ManuProductPrameterPagedQueryDto()
            {
                SFC = "YTLSM202309130001A",
                ParameterType = ParameterTypeEnum.Product,
                PageSize = 2000,
                PageIndex = 1
            };
            var result = await _productTraceReportService.GetProductPrameterPagedListAsync(param);
            Assert.IsTrue(result != null);
        }

        /// <summary>
        /// 按成品条码/模组条码反查层级信息
        /// </summary>
        /// <returns></returns>
        [TestMethod()]
        public async Task GetProductTracePagedListAsyncTest()
        {
            ProductTracePagedQueryDto param = new ProductTracePagedQueryDto()
            {
                SFC = "YTLSM202309130001A",
                TraceDirection = false,
                PageSize = 2000,
                PageIndex = 1
            };
            var result = await _productTraceReportService.GetProductTracePagedListAsync(param);
            Assert.IsTrue(result != null);
        }
    }
}