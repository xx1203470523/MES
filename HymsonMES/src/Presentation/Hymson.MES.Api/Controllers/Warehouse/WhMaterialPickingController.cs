using Hymson.MES.Services.Dtos.Manufacture.WhMaterialPicking;
using Hymson.MES.Services.Services.Warehouse.WhMaterialPicking;
using Hymson.Web.Framework.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.Warehouse
{
    /// <summary>
    /// 控制器（物料库存）
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class WhMaterialPickingController : ControllerBase
    {
        /// <summary>
        /// 接口（物料库存）
        /// </summary>
        private readonly IWhMaterialPickingService _whMaterialPickingService;
        private readonly ILogger<WhMaterialInventoryController> _logger;

        /// <summary>
        /// 构造函数（物料库存）
        /// </summary>
        /// <param name="whMaterialPickingService"></param>
        /// <param name="logger"></param>
        public WhMaterialPickingController(IWhMaterialPickingService  whMaterialPickingService, ILogger<WhMaterialInventoryController> logger)
        {
            _whMaterialPickingService = whMaterialPickingService;
            _logger = logger;
        }

        /// <summary>
        /// 领料
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPost]
        [LogDescription("领料", BusinessType.INSERT)]
        public async Task MaterialReturnAsync([FromBody] PickMaterialDto parm)
        {
            await _whMaterialPickingService.PickMaterialsRequestAsync(parm);
        }
    }
}