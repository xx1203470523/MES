using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Manufacture;
using Hymson.MES.Services.Services.Manufacture.ManuMarking;
using Hymson.Web.Framework.Attributes;
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

        /// <summary>
        /// Marking关闭检索
        /// </summary>
        /// <param name="sfc"></param>
        /// <returns></returns>
        [HttpGet("markingCloseSearch/{sfc}")]
        public async Task<IEnumerable<MarkingCloseViewDto>> GetBarcodePagedListBySFCAsync(string sfc)
        {
            return await _manuMarkingService.GetBarcodePagedListBySFCAsync(sfc);
        }

        /// <summary>
        /// Marking关闭校验SFC
        /// </summary>
        /// <param name="sfc"></param>
        /// <returns></returns>
        [HttpGet("checkSFCMarkingClose/{sfc}")]
        public async Task CheckSFCMarkingCloseAsync(string sfc)
        {
             await _manuMarkingService.CheckSFCMarkingCloseAsync(sfc);
        }

        /// <summary>
        /// Marking关闭确认提交
        /// </summary>
        /// <param name="checkDto"></param>
        /// <returns></returns>
        [HttpPost("markingCloseConfirm")]
        [LogDescription("Marking关闭确认提交", BusinessType.OTHER)]
        public async Task MarkingCloseConfirmAsync([FromBody] MarkingCloseConfirmDto checkDto)
        {
             await _manuMarkingService.SaveMarkingCloseAsync(checkDto);
        }
    }
}
