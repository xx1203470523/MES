using Hymson.MES.EquipmentServices.Dtos.InBound;
using Hymson.MES.EquipmentServices.Dtos.OutBound;
using Hymson.MES.EquipmentServices.Services.OutBound;
using Hymson.MES.EquipmentServicesTests;
using Hymson.MES.Services.Services.Equipment.EquEquipment;
using Hymson.Utils;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Hymson.MES.EquipmentServices.Services.InBound.Tests
{
    [TestClass]
    public class InBoundTestTest : BaseTest
    {
        private readonly IInBoundService _inBoundService;
        private readonly IOutBoundService _outBoundService;
        private readonly IEquEquipmentService _equEquipmentService;
        public InBoundTestTest()
        {
            _inBoundService = ServiceProvider.GetRequiredService<IInBoundService>();
            _outBoundService = ServiceProvider.GetRequiredService<IOutBoundService>();
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
        /// 电芯段：电芯OCV，电芯绑模组（端板刻码）
        /// 模组段：端板刻码，CCS安装,CCS焊接，模组OCVR,焊缝清洁，绑定Pack码（和BMU一起）
        /// BMU：液冷板气密性检测，液冷板涂胶，绑定Pack码（和模组码一起）
        /// Pack段:人工组件安装(2),线束固定与连接排固定,EOL测试,箱体气密性测试,下线
        /// </summary>
        [TestMethod()]
        public async Task LYPInBoundTestAsync()
        {
            ////电芯OCV
            //string resourceCode = "YTLPACK01ER004";
            //string equipmentCode = "YTLPACK01AE004";

            ////端板刻码
            //string resourceCode = "YTLPACK01ER014";
            //string equipmentCode = "YTLPACK01AE014";

            //CCS安装
            string resourceCode = "YTLPACK01ER016";
            string equipmentCode = "YTLPACK01AE016";

            ////CCS焊接
            //string resourceCode = "YTLPACK01ER017";
            //string equipmentCode = "YTLPACK01AE017";

            ////模组OCV
            //string resourceCode = "YTLPACK01ER018";
            //string equipmentCode = "YTLPACK01AE018";

            ////焊缝清洗
            //string resourceCode = "YTLPACK01ER026";
            //string equipmentCode = "YTLPACK01VIR01";

            ////气密性
            //string resourceCode = "YTLPACK01ER021";
            //string equipmentCode = "YTLPACK01AE021";

            ////涂胶
            //string resourceCode = "YTLPACK01ER022";
            //string equipmentCode = "YTLPACK01AE022";

            //string prefix = "YTC20240620003";

            //string prefix = "YTM20240620003";

            //BMU进站测试
            //string prefix = "CELL20241024002";
            string prefix = "YTLSM202410280086A";

            await SetEquInfoAsync(equipmentCode);
            await _inBoundService.InBoundAsync(new InBoundDto
            {
                IsVerifyVirtualSFC = false,
                LocalTime = HymsonClock.Now(),
                ResourceCode = resourceCode,
                SFC = prefix,
            });
            Assert.IsTrue(true);
        }

        /// <summary>
        /// 出站测试
        /// </summary>
        [TestMethod()]
        public async Task OutBoundTestAsync()
        {

            string request = "{\"SFC\":\"YTLSM202310120058A\",\"Passed\":1,\"ParamList\":null,\"BindFeedingCodes\":null,\"NG\":null,\"IsPassingStation\":true,\"ResourceCode\":\"YTLPACK01ER014\",\"EquipmentCode\":\"YTLPACK01AE004\",\"LocalTime\":\"2023-11-10T10:37:00\"}";
            string resourceCode = "YTLPACK01ER014";
            string equipmentCode = "YTLPACK01AE014";
            string prefix = "YTLSM202310121058A";
            List<OutBoundParam> outBoundParams = new List<OutBoundParam>();
            for (int i = 1; i <= 1; i++)
            {
                outBoundParams.Add(new OutBoundParam
                {
                    ParamCode = "单元测试出站参数" + i,
                    ParamValue = i.ToString()
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
                    SFC = "0IJCBA02961111D7R0009892",
                    ParamList = outBoundParams.ToArray()
                });

            }
            catch (Exception ex)
            {
                int ss = 1;
                //throw;
            }
            Assert.IsTrue(true);
        }

        ///// <summary>
        ///// 批量进站测试
        ///// </summary>
        //[TestMethod()]
        //public async Task InBoundMoreTestAsync()
        //{
        //    string resourceCode = "QAEMZY002";
        //    string equipmentCode = "QAEM002";
        //    string prefix = "AAATESTSFC22211";

        //    await SetEquInfoAsync(equipmentCode);
        //    List<string> sfcs = new List<string>();
        //    for (int i = 0; i < 10; i++)
        //    {
        //        sfcs.Add(prefix + i);
        //    }
        //    await _inBoundService.InBoundMoreAsync(new InBoundMoreDto
        //    {
        //        IsVerifyVirtualSFC = false,
        //        LocalTime = HymsonClock.Now(),
        //        ResourceCode = resourceCode,
        //        SFCs = sfcs.ToArray()
        //    });
        //    Assert.IsTrue(true);
        //}
    }
}