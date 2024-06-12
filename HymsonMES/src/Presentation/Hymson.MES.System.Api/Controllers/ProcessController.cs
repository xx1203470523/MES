using Hymson.MES.SystemServices.Dtos;
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
        /// 构造函数
        /// </summary>
        public ProcessController() { }

        /// <summary>
        /// 读取BOM信息
        /// </summary>
        /// <returns></returns>
        [HttpPost("Bom/sync")]
        [ProducesResponseType(typeof(ResultDto), 200)]
        [LogDescription("读取BOM信息", BusinessType.INSERT)]
        public async Task<IEnumerable<BomDto>> GetBomAsync()
        {
            return await Task.FromResult<IEnumerable<BomDto>>(default);
        }

    }
}