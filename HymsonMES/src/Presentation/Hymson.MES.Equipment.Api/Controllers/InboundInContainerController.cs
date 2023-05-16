using Hymson.MES.EquipmentServices.Request.InboundInContainer;
using Hymson.MES.EquipmentServices.Services.InboundInContainer;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Equipment.Api.Controllers
{
    /// <summary>
    /// 进站-容器
    /// </summary>
    [Route("api/v1/[controller]")]
    [ApiController]
    public class InboundInContainerController : ControllerBase
    {
        private readonly IInboundInContainerService _InboundInContainerService;
        private readonly ILogger<InboundInContainerController> _logger;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="InboundInContainerService"></param>
        /// <param name="logger"></param>
        public InboundInContainerController(IInboundInContainerService InboundInContainerService, ILogger<InboundInContainerController> logger)
        {
            _InboundInContainerService = InboundInContainerService;
            _logger = logger;
        }

        /// <summary>
        /// 进站-容器
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("InboundInContainer")]
        public async Task InboundInContainerAsync(InboundInContainerRequest request)
        {
            await _InboundInContainerService.InboundInContainerAsync(request);
        }
    }
}
