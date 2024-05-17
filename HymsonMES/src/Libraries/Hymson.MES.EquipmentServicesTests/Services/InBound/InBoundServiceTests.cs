using Hymson.MES.EquipmentServices.Dtos.InBound;
using Hymson.MES.EquipmentServicesTests;
using Hymson.MES.Services.Services.Equipment.EquEquipment;
using Hymson.Utils;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Hymson.MES.EquipmentServices.Services.InBound.Tests
{
    [TestClass]
    public class InBoundServiceTests : BaseTest
    {
        private readonly IInBoundService _inBoundService;
        private readonly IEquEquipmentService _equEquipmentService;
        public InBoundServiceTests()
        {
            _inBoundService = ServiceProvider.GetRequiredService<IInBoundService>();
            _equEquipmentService = ServiceProvider.GetRequiredService<IEquEquipmentService>();
        }

        [TestCleanup]
        public void TestCleanup()
        {
        }
        /// <summary>
        /// 初始化
        /// </summary>
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
        /// 进站测试
        /// </summary>
        [TestMethod()]
        public async Task InBoundTestAsync()
        {
            string resourceCode = "YTLPACK01ER012";
            string equipmentCode = "YTLPACK01AE028";
           
            await SetEquInfoAsync(equipmentCode);
            await _inBoundService.InBoundAsync(new InBoundDto
            {
                IsVerifyVirtualSFC = false,
                LocalTime = HymsonClock.Now(),
                ResourceCode = resourceCode,
                SFC = "YTLSMTEST"
            });
            Assert.IsTrue(true);
        }

        /// <summary>
        /// 批量进站测试
        /// </summary>
        [TestMethod()]
        public async Task InBoundMoreTestAsync()
        {
            string resourceCode = "QAEMZY002";
            string equipmentCode = "QAEM002";
            string prefix = "AAATESTSFC22211";

            await SetEquInfoAsync(equipmentCode);
            List<string> sfcs = new List<string>();
            for (int i = 0; i < 10; i++)
            {
                sfcs.Add(prefix + i);
            }
            await _inBoundService.InBoundMoreAsync(new InBoundMoreDto
            {
                IsVerifyVirtualSFC = false,
                LocalTime = HymsonClock.Now(),
                ResourceCode = resourceCode,
                SFCs = sfcs.ToArray()
            });
            Assert.IsTrue(true);
        }
    }
}