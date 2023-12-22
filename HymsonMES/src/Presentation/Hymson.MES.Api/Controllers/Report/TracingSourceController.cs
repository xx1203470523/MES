using Hymson.MES.Services.Dtos.Report;
using Hymson.MES.Services.Services;
using Hymson.Web.Framework.Filters.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.Report
{
    /// <summary>
    /// 控制器（条码追溯）
    [ApiController]
    [Route("api/v1/[controller]")]
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
        public async Task<NodeSourceDto> SourceAsync(string sfc)
        {
            return await _tracingSFCService.SourceAsync(sfc);
        }

        /// <summary>
        /// 条码追溯（正向）
        /// </summary>
        /// <param name="sfc"></param>
        /// <returns></returns>
        [HttpGet("to/{sfc}")]
        public async Task<NodeSourceDto> DestinationAsync(string sfc)
        {
            return await _tracingSFCService.DestinationAsync(sfc);
        }

    }
}
