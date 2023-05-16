using Hymson.MES.EquipmentServices.Request.OutPutQty;
using Hymson.MES.EquipmentServices.Services.OutPutQty;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Equipment.Api.Controllers
{
    /// <summary>
    ///产出上报数量
    /// </summary>
    [Route("api/v1/[controller]")]
    [ApiController]
    public class OutPutQtyController : ControllerBase
    {
        private readonly IOutPutQtyService _OutPutQtyService;
        private readonly ILogger<OutPutQtyController> _logger;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="OutPutQtyService"></param>
        /// <param name="logger"></param>
        public OutPutQtyController(IOutPutQtyService OutPutQtyService, ILogger<OutPutQtyController> logger)
        {
            _OutPutQtyService = OutPutQtyService;
            _logger = logger;
        }

        /// <summary>
        ///产出上报数量
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("OutPutQty")]
        public async Task OutPutQtyAsync(OutPutQtyRequest request)
        {
            await _OutPutQtyService.OutPutQtyAsync(request);
        }
    }
}
