using Hymson.MES.Core.Constants.Common;
using Hymson.MES.SystemServices.Dtos;
using Hymson.MES.SystemServices.Services.Quality;
using Hymson.Web.Framework.Attributes;
using Hymson.Web.Framework.Filters.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.System.Api.Controllers
{
    /// <summary>
    /// 蔚来
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class NIOController : ControllerBase
    {
        /// <summary>
        /// 接口（NIO）
        /// </summary>
        private readonly INIOService _nioService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="INIOService"></param>
        public NIOController(INIOService INIOService)
        {
            _nioService = INIOService;
        }

        /// <summary>
        /// 队列（添加）
        /// </summary>
        /// <param name="requestDtos"></param>
        /// <returns></returns>
        [HttpPost("Queue/add")]
        [ProducesResponseType(typeof(ResultDto), 200)]
        [LogDescription("队列（添加）", BusinessType.INSERT)]
        public async Task AddQueueAsync(IEnumerable<NIOAddDto> requestDtos)
        {
            _ = await _nioService.AddQueueAsync(requestDtos);
        }

        /// <summary>
        /// MES更新内容
        /// </summary>
        /// <returns></returns>
        [Route("MesUpdateContent")]
        [HttpGet]
        [AllowAnonymous]
        public string MesUpdateContentApi()
        {
            return MesUpdateContent.GetMesUpdateContent();

        }
    }
}