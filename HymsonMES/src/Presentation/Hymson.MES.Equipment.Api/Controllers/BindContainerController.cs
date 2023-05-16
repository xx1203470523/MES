using Hymson.MES.EquipmentServices.Request.BindContainer;
using Hymson.MES.EquipmentServices.Services.BindContainer;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Equipment.Api.Controllers
{
    /// <summary>
    /// 容器绑定
    /// </summary>
    [Route("api/v1/[controller]")]
    [ApiController]
    public class BindContainerController : ControllerBase
    {
        private readonly IBindContainerService _bindContainerService;
        private readonly ILogger<BindContainerController> _logger;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="bindContainerService"></param>
        /// <param name="logger"></param>
        public BindContainerController(IBindContainerService bindContainerService, ILogger<BindContainerController> logger)
        {
            _logger = logger;
            _bindContainerService = bindContainerService;
        }

        /// <summary>
        /// 容器绑定
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("BindContainer")]
        public async Task BindContainerAsync(BindContainerRequest request)
        {
            await _bindContainerService.BindContainerAsync(request);
        }

        /// <summary>
        /// 容器解绑
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("UnBindContainer")]
        public async Task UnBindContainerAsync(UnBindContainerRequest request)
        {
            await _bindContainerService.UnBindContainerAsync(request);
        }
    }
}
