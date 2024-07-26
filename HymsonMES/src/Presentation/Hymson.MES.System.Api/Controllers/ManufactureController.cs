using Hymson.MES.SystemServices.Dtos;
using Hymson.MES.SystemServices.Services;
using Hymson.MES.SystemServices.Services.Manufacture;
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
    [Route("[controller]")]
    [AllowAnonymous]
    //[Authorize]
    public class ManufactureController : ControllerBase
    {
        /// <summary>
        /// 业务接口（生产领料）
        /// </summary>
        private readonly IManuRequistionOrderService _manuRequistionOrderService;
        // private readonly IManuRotorService _manuRotorService;
        /// <summary>
        /// 构造函数, IManuRotorService manuRotorService
        /// </summary>
        public ManufactureController(IManuRequistionOrderService manuRequistionOrderService
            )
        {
            _manuRequistionOrderService = manuRequistionOrderService;
            // _manuRotorService = manuRotorService;
        }
        /// <summary>
        /// 生产领料(WMS领料单完成后)
        /// </summary>
        /// <param name="productionPickDto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("PickMaterials")]
        [ProducesResponseType(typeof(ResultDto), 200)]
        [LogDescription("接收生产领料信息", BusinessType.INSERT)]
        public async Task SavePickMaterialsAsync([FromBody] ProductionPickDto productionPickDto)
        {
            await _manuRequistionOrderService.SavePickMaterialsAsync(productionPickDto);
        }
        /// <summary>
        /// 生产领料单结果反馈
        /// </summary>
        /// <param name="callBackDto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("PickMaterialsCallBack")]
        [ProducesResponseType(typeof(ResultDto), 200)]
        [LogDescription("生产领料单结果反馈", BusinessType.INSERT)]
        public async Task PickMaterialsCallBackAsync([FromBody] ProductionPickCallBackDto callBackDto)
        {
            await _manuRequistionOrderService.PickMaterialsCallBackAsync(callBackDto);
        }
        /// <summary>
        /// 生产退料单结果反馈
        /// </summary>
        /// <param name="callBackDto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("ReturnMaterialsCallBack")]
        [ProducesResponseType(typeof(ResultDto), 200)]
        [LogDescription("生产退料单结果反馈", BusinessType.INSERT)]
        public async Task ReturnMaterialsCallBackAsync([FromBody] ProductionReturnCallBackDto callBackDto)
        {
            await _manuRequistionOrderService.ReturnMaterialsCallBackAsync(callBackDto);
        }
        /// <summary>
        /// 转子线过站上报
        /// </summary>
        /// <param name="callBackDto"></param>
        /// <returns></returns>
        //[HttpPost]
        //[Route("RotorUploadCrossingStationData")]
        //[ProducesResponseType(typeof(ResultDto), 200)]
        //[LogDescription("转子线过站上报", BusinessType.INSERT)]
        //public async Task RotorUploadCrossingStationDataAsync([FromBody] RotorCrossingStationData stationData)
        //{
        //    await _manuRotorService.UploadCrossingStationData(stationData);
        //}

        /// <summary>
        /// 成品入库单结果反馈
        /// </summary>
        /// <param name="callBackDto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("ProductReceiptCallBack")]
        [ProducesResponseType(typeof(ResultDto), 200)]
        [LogDescription("成品入库单结果反馈", BusinessType.INSERT)]
        public async Task<ResponseOutputDto> ProductReceiptCallBackAsync([FromBody] ProductionReturnCallBackDto callBackDto)
        {
            return await _manuRequistionOrderService.ProductReceiptCallBackAsync(callBackDto);
        }
    }
}