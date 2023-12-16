using Hymson.MES.CoreServices.Bos.Common;
using Hymson.MES.EquipmentServices.Services;
using Hymson.Web.Framework.Filters.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Equipment.Api.Controllers
{
    /// <summary>
    /// 控制器（参数）
    [ApiController]
    [Route("EquipmentService/api/v1/Tracing")]
    public class TracingSourceController : ControllerBase
    {
        /// <summary>
        /// 条码追溯服务接口
        /// </summary>
        private readonly ITracingSourceSFCService _tracingSFCService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="tracingSFCService"></param>
        public TracingSourceController(ITracingSourceSFCService tracingSFCService)
        {
            _tracingSFCService = tracingSFCService;
        }


        /// <summary>
        /// 条码追溯（反向）
        /// </summary>
        /// <param name="sfc"></param>
        /// <returns></returns>
        [HttpGet("from/{sfc}")]
        [ProducesDefaultResponseType(typeof(ResultDto))]
        public async Task<NodeSourceBo> SourceAsync(string sfc)
        {
            return await _tracingSFCService.SourceAsync(sfc);
        }

        /// <summary>
        /// 条码追溯（正向）
        /// </summary>
        /// <param name="sfc"></param>
        /// <returns></returns>
        [HttpGet("to/{sfc}")]
        [ProducesDefaultResponseType(typeof(ResultDto))]
        public async Task<NodeSourceBo> DestinationAsync(string sfc)
        {
            return await _tracingSFCService.DestinationAsync(sfc);
        }

    }
}
