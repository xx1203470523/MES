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
    [Route("[controller]")]
    [AllowAnonymous]
    public class ProcessController : ControllerBase
    {
        /// <summary>
        /// 接口（物料）
        /// </summary>
        private readonly IProcMaterialService _procMaterialService;

        /// <summary>
        /// 接口（BOM）
        /// </summary>
        private readonly IProcBomService _procBomService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="procMaterialService"></param>
        /// <param name="procBomService"></param>
        public ProcessController(IProcMaterialService procMaterialService, IProcBomService procBomService)
        {
            _procMaterialService = procMaterialService;
            _procBomService = procBomService;
        }

        /// <summary>
        /// 物料信息（同步）
        /// </summary>
        /// <returns></returns>
        [HttpPost("Material/sync")]
        [ProducesResponseType(typeof(ResultDto), 200)]
        [LogDescription("物料信息（同步）", BusinessType.INSERT)]
        public async Task SyncMaterialAsync(IEnumerable<MaterialDto> requestDtos)
        {
            _ = await _procMaterialService.SyncMaterialAsync(requestDtos);
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