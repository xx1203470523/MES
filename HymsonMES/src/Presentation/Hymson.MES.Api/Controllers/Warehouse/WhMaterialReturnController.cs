using Hymson.MES.Services.Dtos.Manufacture.WhMaterialReturn;
using Hymson.MES.Services.Services.Warehouse.WhMaterialReturn;
using Hymson.Web.Framework.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.Warehouse
{
    /// <summary>
    /// 控制器（物料库存）
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class WhMaterialReturnController : ControllerBase
    {
        /// <summary>
        /// 接口（物料库存）
        /// </summary>
        private readonly IWhMaterialReturnService _whMaterialReturnService;
        private readonly ILogger<WhMaterialInventoryController> _logger;

        /// <summary>
        /// 构造函数（物料库存）
        /// </summary>
        /// <param name="whMaterialReturnService"></param>
        /// <param name="logger"></param>
        public WhMaterialReturnController(IWhMaterialReturnService whMaterialReturnService, ILogger<WhMaterialInventoryController> logger)
        {
            _whMaterialReturnService = whMaterialReturnService;
            _logger = logger;
        }

        /// <summary>
        /// 物料退料
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPost]
        [LogDescription("物料退料", BusinessType.INSERT)]
        public async Task MaterialReturnAsync([FromBody] MaterialReturnDto parm)
        {
            await _whMaterialReturnService.MaterialReturnAsync(parm);
        }
    }
}