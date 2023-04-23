using Microsoft.VisualStudio.TestTools.UnitTesting;
using Hymson.MES.Services.Services.Manufacture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting.Server;
using FluentValidation;
using Microsoft.Extensions.Configuration;

namespace Hymson.MES.Services.Services.Manufacture.Tests
{
    [TestClass()]
    public class ManuContainerPackServiceTests
    {
        ManuContainerPackService? manuContainerPackService;
        
        public ManuContainerPackServiceTests()
        {
            var services = new ServiceCollection();
            ConfigurationManager configurationManager = new ConfigurationManager();
            services.AddAppService(configurationManager);
          
            var provider = services.BuildServiceProvider();
            manuContainerPackService = provider.GetService<ManuContainerPackService>();
            
        }
        [TestMethod()]
        public void DeleteAllByContainerBarCodeIdAsyncTest()
        {
            //ManuContainerPackService manuContainerPackService = new ManuContainerPackService();
            
            manuContainerPackService?.DeleteAllByContainerBarCodeIdAsync(9806332340469760);
            Assert.Fail();
        }

        [TestMethod()]
        public void DeletesManuContainerPackAsyncTest()
        {
            Assert.Fail();
        }
    }
}