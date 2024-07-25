using Hymson.MES.SystemServices.Dtos;
using Hymson.MES.SystemServices.Dtos.Warehouse;
using Hymson.MES.SystemServices.Services;
using Hymson.MES.SystemServices.Services.Warehouse;
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
        private readonly IWhMaterialReturnService  _whMaterialReturnService;

        /// <summary>
        /// 构造函数
        /// </summary>
        public WarehouseController(IWhMaterialReturnService whMaterialReturnService) {
            _whMaterialReturnService= whMaterialReturnService;
        }

        /// <summary>
        /// 生产退料单结果反馈
        /// </summary>
        /// <param name="callBackDto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("materialReturnConfirm")]
        [ProducesResponseType(typeof(ResultDto), 200)]
        [LogDescription("生产退料确认", BusinessType.INSERT)]
        public async Task WhMaterialReturnConfirmAsync([FromBody] WhMaterialReturnConfirmDto callBackDto)
        {
            await _whMaterialReturnService.WhMaterialReturnConfirmAsync(callBackDto);
        }
    }
}