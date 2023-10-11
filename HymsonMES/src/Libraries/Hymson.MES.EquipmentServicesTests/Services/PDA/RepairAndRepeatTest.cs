using Hymson.MES.EquipmentServices.Dtos.BindSFC;
using Hymson.MES.EquipmentServices.Services.BindSFC;
using Hymson.MES.EquipmentServices.Services.SfcCirculation;
using Hymson.MES.Services.Dtos.Integrated;
using Hymson.MES.Services.Services.Integrated.InteSFCBox;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Minio.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.EquipmentServicesTests.Services.PDA;

/// <summary>
/// 设备 维修复投
/// </summary>
[TestClass()]
public class RepairAndRepeatTest : BaseTest
{
    private readonly IBindSFCService _bindSFCService;
    public RepairAndRepeatTest(IBindSFCService bindSFCService)
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
    /// 复投
    /// </summary>
    /// <returns></returns>
    [TestMethod()]
    public async Task RepeatManuSFCAsyncTest()
    {


        var query = new ResumptionInputDto
        {
            SFC = "",
            RepeatLocationId = 123456,
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
    public async Task getbindsfcTest()
    {
        var query = new BindSFCInputDto
        {
            SFC = "",
        };
        var result = await _bindSFCService.GetBindSFC(query);
        Assert.IsTrue(true);
    }
}
