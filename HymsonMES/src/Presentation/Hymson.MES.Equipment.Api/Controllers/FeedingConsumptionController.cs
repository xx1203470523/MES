using Hymson.MES.EquipmentServices.Request.FeedingConsumption;
using Hymson.MES.EquipmentServices.Services.FeedingConsumption;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Equipment.Api.Controllers
{
    /// <summary>
    /// 上报物料消耗
    /// </summary>
    [Route("api/v1/[controller]")]
    [ApiController]
    public class FeedingConsumptionController : ControllerBase
    {
        private readonly IFeedingConsumptionService _FeedingConsumptionService;
        private readonly ILogger<FeedingConsumptionController> _logger;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="FeedingConsumptionService"></param>
        /// <param name="logger"></param>
        public FeedingConsumptionController(IFeedingConsumptionService FeedingConsumptionService, ILogger<FeedingConsumptionController> logger)
        {
            _FeedingConsumptionService = FeedingConsumptionService;
            _logger = logger;
        }

        /// <summary>
        /// 上报物料消耗
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("FeedingConsumption")]
        public async Task FeedingConsumptionAsync(FeedingConsumptionRequest request)
        {
            await _FeedingConsumptionService.FeedingConsumptionAsync(request);
        }
    }
}
