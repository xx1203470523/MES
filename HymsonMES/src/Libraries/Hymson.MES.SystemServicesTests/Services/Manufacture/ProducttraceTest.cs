using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.SystemServicesTests.Services.Manufacture
{
    [TestClass()]
    public class ProducttraceTest
    {
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
    }
}
