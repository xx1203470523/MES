using Hymson.MES.EquipmentServices.Services.OutBound;
using Hymson.MES.EquipmentServices.Services.SfcCirculation;
using Hymson.MES.Services.Dtos.Integrated;
using Hymson.MES.Services.Services.Integrated.InteSFCBox;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Hymson.MES.EquipmentServicesTests.Services.PDA
{
    [TestClass()]
    public class NGCopeWithTest : BaseTest
    {
        private readonly ISfcCirculationService _sfcCirculationService;
        private readonly IInteSFCBoxService inteSFCBoxService;
        public NGCopeWithTest()
        {
            _sfcCirculationService = ServiceProvider.GetRequiredService<ISfcCirculationService>();
            inteSFCBoxService = ServiceProvider.GetRequiredService<IInteSFCBoxService>();
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

        /// <summary>
        /// PDA批次校验
        /// </summary>
        /// <returns></returns>
        [TestMethod()]
        public async Task SFCValidate()
        {
            var quer = new InteSFCBoxValidateQuery()
            {
                //BoxCode = "20100000109623073103100317",
                BoxCode = "20400000171823090903100040",
                WorkOrderCode = "TEST006"
            };
            try
            {
                await inteSFCBoxService.SFCValidate(quer);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            Assert.IsTrue(true);
        }

    }
}
