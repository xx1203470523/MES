using Hymson.MES.SystemServices.Dtos;
using Hymson.MES.SystemServices.Services.Process;
using Hymson.Web.Framework.Attributes;
using Hymson.Web.Framework.Filters.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.System.Api.Controllers
{
    /// <summary>
    /// 工艺
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ProcessController : ControllerBase
    {
        /// <summary>
        /// 接口（BOM）
        /// </summary>
        private readonly IProcBomService _procBomService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="procBomService"></param>
        public ProcessController(IProcBomService procBomService)
        {
            _procBomService = procBomService;
        }

        /// <summary>
        /// BOM信息（同步）
        /// </summary>
        /// <returns></returns>
        [HttpPost("Bom/sync")]
        [ProducesResponseType(typeof(ResultDto), 200)]
        [LogDescription("BOM信息（同步）", BusinessType.INSERT)]
        public async Task SyncBomAsync(IEnumerable<BomDto> requestDtos)
        {
            _ = await _procBomService.SyncBomAsync(requestDtos);
        }

    }
}