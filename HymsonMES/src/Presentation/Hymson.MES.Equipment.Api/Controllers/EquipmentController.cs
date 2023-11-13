using Hymson.MES.EquipmentServices.Dtos;
using Hymson.MES.EquipmentServices.Dtos.InBound;
using Hymson.MES.EquipmentServices.Services.Manufacture;
using Hymson.MES.EquipmentServices.Services.SfcBinding;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Equipment.Api.Controllers
{
    /// <summary>
    /// 控制器（设备）
    /// </summary>
    [ApiController]
    //[AllowAnonymous]
    [Route("EquipmentService/api/v1/[controller]")]
    public class EquipmentController : ControllerBase
    {
        /// <summary>
        /// 日志
        /// </summary>
        private readonly ILogger<EquipmentController> _logger;

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
        /// <param name="logger"></param>
        /// <param name="manufactureService"></param>
        /// <param name="sfcBindingService"></param>
        public EquipmentController(ILogger<EquipmentController> logger,
            IManufactureService manufactureService,
            ISfcBindingService sfcBindingService)
        {
            _logger = logger;
            _manufactureService = manufactureService;
            _sfcBindingService = sfcBindingService;
        }


        /// <summary>
        ///条码绑定
        /// </summary>
        /// <param name="sfcBindingDto"></param> 
        /// <returns></returns>
        [HttpPost]
        [Route("SfcBinding")]
        public async Task SfcBindingAsync(SfcBindingDto sfcBindingDto)
        {
            await _sfcBindingService.SfcBindingAsync(sfcBindingDto);
        }


        /// <summary>
        /// 进站 HY-MES-EQU-015
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("InBound")]
        public async Task InBoundAsync(InBoundDto request)
        {
            await _manufactureService.InBoundAsync(request);
        }

        /// <summary>
        /// 进站（多个）HY-MES-EQU-016
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("InBoundMore")]
        public async Task InBoundMoreAsync(InBoundMoreDto request)
        {
            await _manufactureService.InBoundMoreAsync(request);
        }

        /// <summary>
        /// 出站 HY-MES-EQU-017
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("OutBound")]
        public async Task OutBoundAsync(OutBoundDto request)
        {
            await _manufactureService.OutBoundAsync(request);
        }

        /// <summary>
        /// 出站（多个） HY-MES-EQU-018
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("OutBoundMore")]
        public async Task OutBoundMoreAsync(OutBoundMoreDto request)
        {
            await _manufactureService.OutBoundMoreAsync(request);
        }


    }
}
