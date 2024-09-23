using Hymson.MES.SystemServices.Dtos;
using Hymson.MES.SystemServices.Services.Quality;
using Hymson.Utils;
using Hymson.Web.Framework.Attributes;
using Hymson.Web.Framework.Filters.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.System.Api.Controllers
{
    /// <summary>
    /// 质量
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class QualityController : ControllerBase
    {
        /// <summary>
        /// 日志
        /// </summary>
        private readonly ILogger<QualityController> _logger;

        /// <summary>
        /// 服务接口（来料检验）
        /// </summary>
        private readonly IQualIQCService _qualIQCService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="qualIQCService"></param>
        public QualityController(ILogger<QualityController> logger, IQualIQCService qualIQCService)
        {
            _logger = logger;
            _qualIQCService = qualIQCService;
        }

        /// <summary>
        /// 提交（来料检验）
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost("iqc/submit")]
        [ProducesResponseType(typeof(ResultDto), 200)]
        [LogDescription("来料检验", BusinessType.INSERT)]
        public async Task SubmitIncomingAsync([FromBody] WhMaterialReceiptDto dto)
        {
            _logger.LogDebug($"来料检验 -> Request: {dto.ToSerialize()}");

            await _qualIQCService.SubmitIncomingAsync(dto);
        }

    }
}