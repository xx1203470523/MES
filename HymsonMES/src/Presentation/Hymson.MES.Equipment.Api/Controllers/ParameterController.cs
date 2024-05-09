using Hymson.MES.EquipmentServices;
using Hymson.MES.EquipmentServices.Dtos.Parameter;
using Hymson.MES.EquipmentServices.Services.Parameter.ProcessCollection;
using Hymson.Web.Framework.Attributes;
using Hymson.Web.Framework.Filters.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Equipment.Api.Controllers
{
    /// <summary>
    /// 控制器（参数）
    [ApiController]
    //[AllowAnonymous]
    [Route("EquipmentService/api/v1/Collection")]
    public class ParameterController : ControllerBase
    {
        /// <summary>
        /// 过程参数采集
        /// </summary>
        private readonly IProcessCollectionService _processCollectionService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="processCollectionService"></param>
        public ParameterController(IProcessCollectionService processCollectionService)
        {
            _processCollectionService = processCollectionService;
        }


        /// <summary>
        /// 产品过程参数采集
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("product")]
        [ProducesDefaultResponseType(typeof(ResultDto))]
        [LogDescription("产品过程参数采集", BusinessType.OTHER, "ProductCollection", ReceiverTypeEnum.MES)]
        public async Task ProductCollectionAsync(ProductProcessParameterDto request)
        {
            await _processCollectionService.ProductCollectionAsync(request);
        }

        /// <summary>
        /// 设备过程参数采集
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("equipment")]
        [LogDescription("设备过程参数采集", BusinessType.OTHER, "EquipmentCollection", ReceiverTypeEnum.MES)]
        [ProducesDefaultResponseType(typeof(ResultDto))]
        public async Task EquipmentCollectionAsync(IEnumerable<EquipmentProcessParameterDto> request)
        {
            await _processCollectionService.EquipmentCollectionAsync(request);
        }

        /// <summary>
        /// 获取参数名
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetParamTableName")]
        [LogDescription("获取参数名", BusinessType.OTHER, "GetParamName", ReceiverTypeEnum.MES)]
        [AllowAnonymous]
        public async Task<List<string>> GetParamNameAsync(GetParamNameDto dto)
        {
            return await _processCollectionService.GetParamTableNameAsync(dto);
        }
    }
}
