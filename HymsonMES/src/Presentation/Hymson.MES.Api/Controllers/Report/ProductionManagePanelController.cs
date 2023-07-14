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
                DayShift = 0,
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

        /// <summary>
        /// 获取当天Pack达成数据
        /// </summary>
        /// <returns></returns>
        [Route("getPackAchievingInfo")]
        public Task<IEnumerable<ProductionManagePanelPackDto>> GetPackAchievingInfo()
        {
            IEnumerable<ProductionManagePanelPackDto> result = new List<ProductionManagePanelPackDto>
            {
                new ProductionManagePanelPackDto()
                {
                    Sort=1,
                    DateTimeRange = "8:30-10:30",
                    InputQty = 40,
                    TargetQty = 100,
                    AchievingRate = 25,
                },
                new ProductionManagePanelPackDto()
                {
                    Sort=2,
                    DateTimeRange = "10:30-12:30",
                    InputQty = 20,
                    TargetQty = 50,
                    AchievingRate = 15,
                },
                new ProductionManagePanelPackDto()
                {
                    Sort=3,
                    DateTimeRange = "12:30-14:30",
                    InputQty = 90,
                    TargetQty = 90,
                    AchievingRate = 90,
                },
                new ProductionManagePanelPackDto()
                {
                    Sort=4,
                    DateTimeRange = "14:30-16:30",
                    InputQty = 10,
                    TargetQty = 100,
                    AchievingRate = 10,
                },
                new ProductionManagePanelPackDto()
                {
                    Sort=5,
                    DateTimeRange = "16:30-18:30",
                    InputQty = 0,
                    TargetQty = 100,
                    AchievingRate = 0,
                },
            };
            return Task.FromResult(result);
        }
    }
}
