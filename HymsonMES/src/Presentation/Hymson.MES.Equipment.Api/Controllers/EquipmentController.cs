using Hymson.MES.EquipmentServices.Dtos.InBound;
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
        /// 条码绑定
        /// </summary>
        private readonly ISfcBindingService _sfcBindingService;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sfcBindingService"></param>
        public EquipmentController(ISfcBindingService sfcBindingService)
        {
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
    }
}
