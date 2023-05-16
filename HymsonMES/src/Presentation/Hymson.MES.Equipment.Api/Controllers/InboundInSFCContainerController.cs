using Hymson.MES.EquipmentServices.Request.InboundInSFCContainer;
using Hymson.MES.EquipmentServices.Services.InboundInSFCContainer;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Equipment.Api.Controllers
{
    /// <summary>
    /// 进站-电芯和托盘-装盘2
    /// </summary>
    [Route("api/v1/[controller]")]
    [ApiController]
    public class InboundInSFCContainerController : ControllerBase
    {
        private readonly IInboundInSFCContainerService _InboundInSFCContainerService;
        private readonly ILogger<InboundInSFCContainerController> _logger;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="InboundInSFCContainerService"></param>
        /// <param name="logger"></param>
        public InboundInSFCContainerController(IInboundInSFCContainerService InboundInSFCContainerService, ILogger<InboundInSFCContainerController> logger)
        {
            _InboundInSFCContainerService = InboundInSFCContainerService;
            _logger = logger;
        }

        /// <summary>
        ///进站-电芯和托盘-装盘2
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("InboundInSFCContainer")]
        public async Task InboundInSFCContainerAsync(InboundInSFCContainerRequest request)
        {
            await _InboundInSFCContainerService.InboundInSFCContainerAsync(request);
        }
    }
}
