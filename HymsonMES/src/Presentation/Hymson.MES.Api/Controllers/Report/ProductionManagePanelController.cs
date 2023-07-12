using Hymson.MES.Services.Dtos.Report;
using Hymson.MES.Services.Services.Report;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.Report
{
    /// <summary>
    /// 生产管理看板控制器
    /// </summary>
    [AllowAnonymous]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ProductionManagePanelController : ControllerBase
    {
        private readonly IProductTraceReportService _productTraceReportService;
        public ProductionManagePanelController(IProductTraceReportService productTraceReportService)
        {
            _productTraceReportService = productTraceReportService;
        }
        /// <summary>
        /// 获取综合信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("getOverallInfo")]
        public Task<ProductionManagePanelReportDto> GetOverallInfo()
        {
            //模拟数据
            return Task.FromResult(new ProductionManagePanelReportDto()
            {
                CompletedQty = 100,
                CompletedRate = 80,
                DayConsume = 120,
                DayShift = false,
                InputQty = 120,
                OverallPlanAchievingRate = 10,
                OverallYieldRate = 90,
                ProcessRouteCode = "1111",
                ProcessRouteName = "工艺路线",
                ProductCode = "AAA",
                ProductName = "产品",
                UnqualifiedQty = 10,
                UnqualifiedRate = 10,
                WorkLineName = "ajkfaw",
                WorkOrderCode = "W292421AAFA",
                WorkOrderDownTime = new DateTime(),
                WorkOrderQty = 200
            });
        }
    }
}
