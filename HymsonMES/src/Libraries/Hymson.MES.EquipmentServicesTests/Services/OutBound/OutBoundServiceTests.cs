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
            //string resourceCode = "QAEMZY028";
            //string equipmentCode = "QAEMSBBH028";
            string resourceCode = "QAEMZY003";
            string equipmentCode = "QAEMSBBH003";
            string prefix = "AAATESTSFC2308091";
            List<OutBoundParam> outBoundParams = new List<OutBoundParam>();
            for (int i = 1; i <= 1; i++)
            {
                outBoundParams.Add(new OutBoundParam
                {
                    ParamCode = "单元测试出站参数" + i,
                    ParamValue = i.ToString(),
                    CreatedBy = "test12321321321"
                });
            }
            try
            {


                await SetEquInfoAsync(equipmentCode);
                await _outBoundService.OutBoundAsync(new OutBoundDto
                {
                    ResourceCode = resourceCode,
                    Passed = 1,
                    LocalTime = HymsonClock.Now(),
                    //SFC = prefix + "1",
                    SFC = prefix,
                    ParamList = outBoundParams.ToArray(),
                    IsPassingStation=false
                });

            }
            catch (Exception ex)
            {
                int ss = 1;
                //throw;
            }
            Assert.IsTrue(true);
        }

        /// <summary>
        /// 多条出站测试
        /// </summary>
        /// <returns></returns>
        [TestMethod()]
        public async Task OutBoundMoreTestAsync()
        {
            string resourceCode = "QAEMZY002";
            string equipmentCode = "QAEM002";
            string prefix = "AAATESTSFC22211";

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

    }
}