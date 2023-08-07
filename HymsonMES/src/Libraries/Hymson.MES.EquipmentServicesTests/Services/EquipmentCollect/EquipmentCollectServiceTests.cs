using Hymson.MES.Core.Enums;
using Hymson.MES.Data.Repositories.Equipment.EquEquipment;
using Hymson.MES.Data.Repositories.Equipment.EquEquipment.Query;
using Hymson.MES.EquipmentServices.Dtos.EquipmentCollect;
using Hymson.MES.EquipmentServicesTests;
using Hymson.MES.EquipmentServicesTests.Dtos;
using Hymson.Utils;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Hymson.MES.EquipmentServices.Services.EquipmentCollect.Tests
{
    [TestClass]
    public class EquipmentCollectServiceTests : BaseTest
    {
        private readonly IEquEquipmentRepository _equEquipmentRepository;//设备
        private readonly IEquipmentCollectService _equipmentCollectService;
        public EquipmentCollectServiceTests()
        {
            _equEquipmentRepository = ServiceProvider.GetRequiredService<IEquEquipmentRepository>();
            _equipmentCollectService = ServiceProvider.GetRequiredService<IEquipmentCollectService>();
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
        /// 模拟上报设备状态
        /// </summary>
        [TestMethod]
        public async Task EquipmentAlarmAsyncTest()
        {
            long siteId = CurrentEquipmentInfo.EquipmentInfoDic.Value["SiteId"].ParseToLong();
            var equEquipmentEntities = await _equEquipmentRepository.GetEntitiesAsync(new EquEquipmentQuery
            {
                SiteId = siteId
            });
            Random random = new Random();
            foreach (var item in equEquipmentEntities)
            {
                //设置当前模拟设备名称
                SetEquInfoAsync(new EquipmentInfoDto { Id = item.Id, FactoryId = item.WorkCenterFactoryId, Code = item.EquipmentCode, Name = item.EquipmentName });
                int stateValue = random.Next(0, 5);
                await _equipmentCollectService.EquipmentStateAsync(new EquipmentStateDto
                {
                    LocalTime = HymsonClock.Now().AddMinutes(Random.Shared.Next(-50, 10)),
                    StateCode = (EquipmentStateEnum)stateValue
                });
            }
            Assert.IsTrue(true);
        }
    }
}