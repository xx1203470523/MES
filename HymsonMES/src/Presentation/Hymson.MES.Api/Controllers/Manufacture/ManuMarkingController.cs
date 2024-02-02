using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Manufacture;
using Hymson.MES.Services.Services.Manufacture.ManuMarking;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.Manufacture
{
    /// <summary>
    /// Marking标识
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ManuMarkingController : ControllerBase
    {
        private readonly IManuMarkingService _manuMarkingService;

        public ManuMarkingController(IManuMarkingService manuMarkingService)
        {
            _manuMarkingService = manuMarkingService;
        }

        /// <summary>
        /// Marking录入校验
        /// </summary>
        /// <param name="checkDto"></param>
        /// <returns></returns>
        [HttpGet("checkMarkingEnter")]
        public async Task<MarkingEnterViewDto> GetBarcodePagedListAsync([FromQuery] ManuMarkingCheckDto checkDto)
        {
            return await _manuMarkingService.CheckMarkingEnterAsync(checkDto);
        }
    }
}
