using Hymson.MES.BackgroundServices.Dtos.EquHeartbeat;
using Hymson.MES.BackgroundServicesTests;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Hymson.MES.BackgroundServices.Services.EquHeartbeat.Tests
{
    [TestClass]
    public class EquHeartbeatServiceTests : BaseTest
    {
        private IEquHeartbeatService _equHeartbeatService;
        public EquHeartbeatServiceTests()
        {
            _equHeartbeatService = ServiceProvider.GetRequiredService<IEquHeartbeatService>();
        }

        [TestInitialize]
        public void TestInitialize()
        {
        }

        [TestCleanup]
        public void TestCleanup()
        {
        }

        /// <summary>
        /// 测试删除几个月之前数据
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task DeleteMonthsBeforeAsyncTest()
        {
            var result = await _equHeartbeatService.DeleteMonthsBeforeAsync(3);
            Assert.IsTrue(result >= 0);
        }

        /// <summary>
        /// 设备心跳更新
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task EquipmentHeartbeatUpdateAsyncTest()
        {
            await _equHeartbeatService.EquipmentHeartbeatUpdateAsync(new EquipmentHeartbeatUpdateDto { IntervalSeconds = 10 });
            Assert.IsTrue(true);
        }
    }
}