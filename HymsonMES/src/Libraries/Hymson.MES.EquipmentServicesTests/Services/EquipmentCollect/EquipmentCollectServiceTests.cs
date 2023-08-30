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
        public async Task EquipmentStateAsyncTest()
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

        /// <summary>
        /// 模拟设备心跳
        /// </summary>
        [TestMethod()]
        public async Task EquipmentHeartbeatAsyncTest()
        {
            long siteId = CurrentEquipmentInfo.EquipmentInfoDic.Value["SiteId"].ParseToLong();
            var equEquipmentEntities = await _equEquipmentRepository.GetEntitiesAsync(new EquEquipmentQuery
            {
                SiteId = siteId
            });
            foreach (var item in equEquipmentEntities)
            {
                //设置当前模拟设备名称
                SetEquInfoAsync(new EquipmentInfoDto { Id = item.Id, FactoryId = item.WorkCenterFactoryId, Code = item.EquipmentCode, Name = item.EquipmentName });
                await _equipmentCollectService.EquipmentHeartbeatAsync(new EquipmentHeartbeatDto
                {
                    IsOnline = Random.Shared.Next(0, 2) == 1,
                    LocalTime = HymsonClock.Now(),
                    ResourceCode = item.EquipmentCode
                });
            }
            Assert.IsTrue(true);
        }

        /// <summary>
        /// 模拟设备故障上报
        /// </summary>
        [TestMethod()]
        public async Task EquipmentAlarmAsyncTest()
        {
            long siteId = CurrentEquipmentInfo.EquipmentInfoDic.Value["SiteId"].ParseToLong();
            var equEquipmentEntities = await _equEquipmentRepository.GetEntitiesAsync(new EquEquipmentQuery
            {
                SiteId = siteId
            });
            foreach (var item in equEquipmentEntities)
            {
                //设置当前模拟设备名称
                SetEquInfoAsync(new EquipmentInfoDto { Id = item.Id, FactoryId = item.WorkCenterFactoryId, Code = item.EquipmentCode, Name = item.EquipmentName });
                await _equipmentCollectService.EquipmentAlarmAsync(new EquipmentAlarmDto
                {
                    Status = EquipmentAlarmStatusEnum.Trigger,//触发
                    AlarmCode = "testAlert" + (char)Random.Shared.Next(97, 122),
                    AlarmMsg = "测试故障上报" + (char)Random.Shared.Next(97, 122),
                    LocalTime = HymsonClock.Now(),
                    ResourceCode = item.EquipmentCode
                });
                Thread.Sleep(TimeSpan.FromSeconds(2));
                await _equipmentCollectService.EquipmentAlarmAsync(new EquipmentAlarmDto
                {
                    Status = EquipmentAlarmStatusEnum.Recover,//恢复
                    AlarmCode = "testAlert" + (char)Random.Shared.Next(97, 122),
                    AlarmMsg = "测试故障上报" + (char)Random.Shared.Next(97, 122),
                    LocalTime = HymsonClock.Now(),
                    ResourceCode = item.EquipmentCode
                });
            }
            Assert.IsTrue(true);
        }

        /// <summary>
        /// 设备产品参数上报
        /// </summary>
        [TestMethod()]
        public async Task EquipmentProductProcessParamAsyncTest()
        {
            string resourceCode = "QAEMZY002";
            long siteId = CurrentEquipmentInfo.EquipmentInfoDic.Value["SiteId"].ParseToLong();
            var equEquipmentEntities = await _equEquipmentRepository.GetEntitiesAsync(new EquEquipmentQuery
            {
                SiteId = siteId,
                EquipmentCodes = new[] { "QAEM002" }
            });
            var equEquipmentEntitie = equEquipmentEntities.First();
            //设置当前模拟设备名称
            SetEquInfoAsync(new EquipmentInfoDto { Id = equEquipmentEntitie.Id, FactoryId = equEquipmentEntitie.WorkCenterFactoryId, Code = equEquipmentEntitie.EquipmentCode, Name = equEquipmentEntitie.EquipmentName });
            await _equipmentCollectService.EquipmentProductProcessParamAsync(new EquipmentProductProcessParamDto
            {
                EquipmentCode = equEquipmentEntitie.EquipmentCode,
                LocalTime = HymsonClock.Now(),
                ResourceCode = resourceCode,
                SFCParams = new EquipmentProductProcessParamSFCDto[] {
                    new EquipmentProductProcessParamSFCDto(){
                        SFC="AAATEST01",
                        NgList = new Ng[] {
                            new Ng{NGCode="NGCODE1" }
                        },
                        ParamList=new  EquipmentProcessParamInfoDto[]{
                            new EquipmentProcessParamInfoDto{
                                ParamCode="TEST001",
                                ParamValue="TEST00001"
                            }
                        }
                    },
                    new EquipmentProductProcessParamSFCDto(){
                        SFC="AAATEST02",
                        NgList = new Ng[] {
                            new Ng{NGCode="NGCODE2" }
                        },
                        ParamList=new  EquipmentProcessParamInfoDto[]{
                            new EquipmentProcessParamInfoDto{
                                ParamCode="TEST002",
                                ParamValue="TEST00002"
                            }
                        }
                    }
                }
            });

            Assert.IsTrue(true);
        }

        [TestMethod()]
        public async Task EquipmentProductNgAsyncTest()
        {
            string resourceCode = "QAEMZY002";
            long siteId = CurrentEquipmentInfo.EquipmentInfoDic.Value["SiteId"].ParseToLong();
            var equEquipmentEntities = await _equEquipmentRepository.GetEntitiesAsync(new EquEquipmentQuery
            {
                SiteId = siteId,
                EquipmentCodes = new[] { "QAEM002" }
            });
            var equEquipmentEntitie = equEquipmentEntities.First();
            //设置当前模拟设备名称
            SetEquInfoAsync(new EquipmentInfoDto { Id = equEquipmentEntitie.Id, FactoryId = equEquipmentEntitie.WorkCenterFactoryId, Code = equEquipmentEntitie.EquipmentCode, Name = equEquipmentEntitie.EquipmentName });
            await _equipmentCollectService.EquipmentProductNgAsync(new EquipmentProductNgDto
            {
                EquipmentCode = equEquipmentEntitie.EquipmentCode,
                LocalTime = HymsonClock.Now(),
                ResourceCode = resourceCode,
                SFCParams = new EquipmentProductNgSFCDto[] {
                    new EquipmentProductNgSFCDto{ 
                        SFC="AAA2103841XCE12A060",
                        NgList =new Ng[]{ 
                            new Ng{ NGCode="NGCODE1" }
                        }
                    }
                }
            });
            Assert.IsTrue(true);
        }
    }
}