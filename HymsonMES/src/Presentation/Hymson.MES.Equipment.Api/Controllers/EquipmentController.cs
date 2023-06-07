using Hymson.Infrastructure;
using Hymson.MES.EquipmentServices;
using Hymson.MES.EquipmentServices.Dtos.InBound;
using Hymson.MES.EquipmentServices.Services.Manufacture.InStation;
using Hymson.MES.EquipmentServices.Services.SfcBinding;
using IdGen;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.Manufacture
{
    /// <summary>
    /// 控制器（设备）
    /// @author pengxin
    /// @date 2023-05-31
    /// </summary>

    [ApiController]
    //[AllowAnonymous]
    [Route("EquipmentService/api/v1/[controller]")]
    public class EquipmentController : ControllerBase
    {

        /// <summary>
        /// 进站
        /// </summary>
        private readonly IInStationService _InStationService;

        /// <summary>
        /// 条码绑定
        /// </summary>
        private readonly ISfcBindingService _sfcBindingService;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="manuInStationService"></param>
        /// <param name="sfcBindingService"></param>
        public EquipmentController(IInStationService manuInStationService, ISfcBindingService sfcBindingService)
        {
            _InStationService = manuInStationService;
            _sfcBindingService = sfcBindingService;
        }


        /// <summary>
        ///进站
        /// </summary>
        /// <param name="inStationDto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("InStation")]
        public async Task InStationAsync(InStationDto inStationDto)
        {
            await _InStationService.InStationAsync(inStationDto);
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
    }
}
