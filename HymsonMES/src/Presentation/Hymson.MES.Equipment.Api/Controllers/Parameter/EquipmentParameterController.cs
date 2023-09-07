using Hymson.MES.EquipmentServices.Dtos.Parameter;
using Hymson.MES.EquipmentServices.Services.Parameter.ProductProcessCollection;
using Hymson.Web.Framework.Filters.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Equipment.Api.Controllers.Parameter
{
    /// <summary>
    /// 控制器（参数）
    /// @author wangkeming
    /// @date 2023-08-07
    /// </summary>

    [ApiController]
    //[AllowAnonymous]
    [Route("EquipmentService/api/v1/[controller]")]
    public class EquipmentParameterController : ControllerBase
    {
        /// <summary>
        /// 参数采集
        /// </summary>
        private readonly IProductProcessCollectionService _productProcessCollectionService;

        /// <summary>
        /// 参数采集
        /// </summary>
        /// <param name="productProcessCollectionService"></param>
        public EquipmentParameterController(IProductProcessCollectionService productProcessCollectionService)
        {
            _productProcessCollectionService = productProcessCollectionService;
        }

        /// <summary>
        ///设备过程参数采集
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("collection")]
        [ProducesDefaultResponseType(typeof(ResultDto))]
        public async Task Collection(IEnumerable<EquipmentProductProcessParameterDto> param)
        {
             await _productProcessCollectionService.EquipmentCollectionAsync(param);
        }
    }
}
