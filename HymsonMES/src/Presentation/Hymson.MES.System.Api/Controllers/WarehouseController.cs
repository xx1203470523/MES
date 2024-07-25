using Hymson.MES.SystemServices.Dtos.Warehouse;
using Hymson.MES.SystemServices.Services.Warehouse.WhMaterialPicking;
using Hymson.MES.SystemServices.Services.Warehouse.WhMaterialReturn;
using Hymson.Web.Framework.Attributes;
using Hymson.Web.Framework.Filters.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.System.Api.Controllers
{
    /// <summary>
    /// 库存
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class WarehouseController : ControllerBase
    {
        /// <summary>
        /// 业务接口（生产领料）
        /// </summary>
        private readonly IWhMaterialReturnService _whMaterialReturnService;
        private readonly IWhMaterialPickingService _whMaterialPickingService;
        /// <summary>
        /// 构造函数
        /// </summary>
        public WarehouseController(IWhMaterialReturnService whMaterialReturnService, IWhMaterialPickingService whMaterialPickingService)
        {
            _whMaterialReturnService = whMaterialReturnService;
            _whMaterialPickingService = whMaterialPickingService;
        }

        /// <summary>
        /// 生产退料单结果反馈
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("materialReturnConfirm")]
        [ProducesResponseType(typeof(ResultDto), 200)]
        [LogDescription("生产退料确认", BusinessType.INSERT)]
        public async Task WhMaterialReturnConfirmAsync([FromBody] WhMaterialReturnConfirmDto param)
        {
            await _whMaterialReturnService.WhMaterialReturnConfirmAsync(param);
        }


        /// <summary>
        /// 领料单物料接收
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("materialPickingReceive")]
        [ProducesResponseType(typeof(ResultDto), 200)]
        [LogDescription("领料单物料接收", BusinessType.INSERT)]
        public async Task MaterialPickingReceiveAsync([FromBody] WhMaterialPickingReceiveDto param)
        {
            await _whMaterialPickingService.MaterialPickingReceiveAsync(param);
        }
    }
}