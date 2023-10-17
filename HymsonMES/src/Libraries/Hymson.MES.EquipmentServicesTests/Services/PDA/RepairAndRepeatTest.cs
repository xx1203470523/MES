using Hymson.MES.Core.Enums;
using Hymson.MES.EquipmentServices.Dtos.BindSFC;
using Hymson.MES.EquipmentServices.Services.BindSFC;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Reflection.Emit;


namespace Hymson.MES.EquipmentServicesTests.Services.PDA;

/// <summary>
/// 设备 维修复投
/// </summary>
[TestClass()]
public class RepairAndRepeatTest : BaseTest
{
    private readonly IBindSFCService _bindSFCService;
    public RepairAndRepeatTest()
    {
        _bindSFCService = ServiceProvider.GetRequiredService<IBindSFCService>();
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
    /// 换绑
    /// </summary>
    /// <returns></returns>
    [TestMethod()]
    public async Task SwitchBindSFCAsyncTest()
    {
        var query = new SwitchBindInputDto
        {
            SFC = "YTLSM202309110004A",
            OldBindSFC = "0IJCBA05011111D7E0002162",
            NewBindSFC = "0IJCBA05011111D7E0005097"
        };

        try
        {
            await _bindSFCService.SwitchBindSFCAsync(query);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }
        Assert.IsTrue(true);
    }

    

    /// <summary>
    /// 复投
    /// </summary>
    /// <returns></returns>
    [TestMethod()]
    public async Task RepeatManuSFCAsyncTest()
    {
        var query = new ResumptionInputDto
        {
            SFC = "YTLSM202309130020A",
            RepeatLocationId = 20033521880395776,
            NGLocationId = 0
        };

        try
        {
            await _bindSFCService.RepeatManuSFCAsync(query);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }
        Assert.IsTrue(true);
    }

    /// <summary>
    /// PDA获取绑定
    /// </summary>
    /// <returns></returns>
    [TestMethod()]
    public async Task GetbindsfcTest()
    {
        var query = new BindSFCInputDto
        {
            //SFC = "YTLSM202309110004A",
            SFC = "YTLSM202309150063A",
            OperateType= RepairOperateTypeEnum.query,
        };
        var result = await _bindSFCService.GetBindSFC(query);
        Assert.IsTrue(true);
    }


    /// <summary>
    /// 全部解绑测试
    /// </summary>
    /// <returns></returns>
    [TestMethod()]
    public async Task UnBindPDAAsyncTest()
    {
        var query = new UnBindSFCInput
        {
            //SFC = "YTLSM202309110004A",
            SFC = "YTLSM202309110005A",

        
        };
     await _bindSFCService.UnBindPDAAsync(query);
        Assert.IsTrue(true);
    }

    
}
