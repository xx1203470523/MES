using Hymson.MES.EquipmentServices;
using Hymson.MES.EquipmentServices.Dtos;
using Hymson.MES.EquipmentServices.Dtos.InBound;
using Hymson.MES.EquipmentServices.Services.Manufacture;
using Hymson.MES.EquipmentServices.Services.SfcBinding;
using Hymson.Web.Framework.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Equipment.Api.Controllers
{
    /// <summary>
    /// 控制器（设备）
    /// </summary>
    [ApiController]
    //[AllowAnonymous]
    [Route("EquipmentService/api/v1")]
    public partial class EquipmentController : ControllerBase
    {
        /// <summary>
        /// 生产服务接口
        /// </summary>
        private readonly IManufactureService _manufactureService;

        /// <summary>
        /// 条码绑定
        /// </summary>
        private readonly ISfcBindingService _sfcBindingService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="manufactureService"></param>
        /// <param name="sfcBindingService"></param>
        public EquipmentController(IManufactureService manufactureService,
            ISfcBindingService sfcBindingService)
        {
            _manufactureService = manufactureService;
            _sfcBindingService = sfcBindingService;
        }

        /// <summary>
        /// 创建条码（半成品）
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("CreateBarCode")]
        [LogDescription("创建条码", BusinessType.OTHER, "CreateBarCode", ReceiverTypeEnum.MES)]
        public async Task<IEnumerable<string>> CreateBarCodeBySemiProductAsync(BaseDto dto)
        {
            return await _manufactureService.CreateBarcodeBySemiProductIdAsync(dto);
        }

        /// <summary>
        /// 创建条码（电芯）
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("CreateCellBarCode")]
        [LogDescription("创建电芯条码", BusinessType.OTHER, "CreateCellBarCode", ReceiverTypeEnum.MES)]
        public async Task<IEnumerable<string>> CreateCellBarCodeAsync(BaseDto dto)
        {
            return await _manufactureService.CreateCellBarCodeAsync(dto);
        }

        /// <summary>
        ///条码绑定
        /// </summary>
        /// <param name="sfcBindingDto"></param> 
        /// <returns></returns>
        [HttpPost]
        [Route("SfcBinding")]
        [LogDescription("条码绑定", BusinessType.OTHER, "SFCBinding", ReceiverTypeEnum.MES)]
        public async Task SfcBindingAsync(SfcBindingDto sfcBindingDto)
        {
            await _sfcBindingService.SfcCirculationBindAsync(sfcBindingDto);
        }

        /// <summary>
        /// 进站 HY-MES-EQU-015
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("InStation")]
        [LogDescription("进站", BusinessType.OTHER, "InStation", ReceiverTypeEnum.MES)]
        public async Task InBoundAsync(InBoundDto request)
        {
            await _manufactureService.InBoundAsync(request);
        }

        /// <summary>
        /// 进站（多个）HY-MES-EQU-016
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("InStationMore")]
        [LogDescription("多个进站", BusinessType.OTHER, "InStationMore", ReceiverTypeEnum.MES)]
        public async Task InBoundMoreAsync(InBoundMoreDto request)
        {
            await _manufactureService.InBoundMoreAsync(request);
        }

        /// <summary>
        /// 出站 HY-MES-EQU-017
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("OutStation")]
        [LogDescription("出站", BusinessType.OTHER, "OutStation", ReceiverTypeEnum.MES)]
        public async Task OutBoundAsync(OutBoundDto request)
        {
            await _manufactureService.OutBoundAsync(request);
        }

        /// <summary>
        /// 出站（多个） HY-MES-EQU-018
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("OutStationMore")]
        [LogDescription("多个出站", BusinessType.OTHER, "OutStationMore", ReceiverTypeEnum.MES)]
        public async Task OutBoundMoreAsync(OutBoundMoreDto request)
        {
            await _manufactureService.OutBoundMoreAsync(request);
        }

        /// <summary>
        /// 载具进站
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("InBoundCarrier")]
        [LogDescription("载具进站", BusinessType.OTHER, "InBoundCarrier", ReceiverTypeEnum.MES)]
        public async Task InBoundCarrierAsync(InBoundCarrierDto request)
        {
            await _manufactureService.InBoundCarrierAsync(request);
        }

        /// <summary>
        /// 载具出站
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("OutBoundCarrier")]
        [LogDescription("载具出站", BusinessType.OTHER, "OutBoundCarrier", ReceiverTypeEnum.MES)]
        public async Task OutBoundCarrierAsync(OutBoundCarrierDto request)
        {
            await _manufactureService.OutBoundCarrierAsync(request);
        }
    }
}
