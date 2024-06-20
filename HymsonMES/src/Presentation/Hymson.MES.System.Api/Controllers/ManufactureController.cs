using Hymson.MES.SystemServices.Dtos;
using Hymson.MES.SystemServices.Services;
using Hymson.Web.Framework.Attributes;
using Hymson.Web.Framework.Filters.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.System.Api.Controllers
{
    /// <summary>
    /// 生产
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [AllowAnonymous]
    //[Authorize]
    public class ManufactureController : ControllerBase
    {
        /// <summary>
        /// 业务接口（生产领料）
        /// </summary>
        private readonly IManuRequistionOrderService _manuRequistionOrderService;
        /// <summary>
        /// 构造函数
        /// </summary>
        public ManufactureController(IManuRequistionOrderService manuRequistionOrderService) 
        { 
            _manuRequistionOrderService = manuRequistionOrderService;
        }
        /// <summary>
        /// 生产领料(人工在ERP领料单完成后)
        /// </summary>
        /// <param name="productionPickDto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("PickMaterials")]
        [ProducesResponseType(typeof(ResultDto), 200)]
        [LogDescription("接收生产领料信息", BusinessType.INSERT)]
        public async Task SavePickMaterialsAsync(ProductionPickDto productionPickDto)
        {
            await _manuRequistionOrderService.SavePickMaterialsAsync(productionPickDto);
        }
    }
}