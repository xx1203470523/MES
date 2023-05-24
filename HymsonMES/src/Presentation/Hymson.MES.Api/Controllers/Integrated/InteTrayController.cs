using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Integrated;
using Hymson.MES.Services.Services.Integrated;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.Integrated
{
    /// <summary>
    /// 控制器（托盘信息）
    /// @author chenjianxiong
    /// @date 2023-05-16 10:57:03
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class InteTrayController : ControllerBase
    {
        /// <summary>
        /// 日志
        /// </summary>
        private readonly ILogger<InteTrayController> _logger;
        /// <summary>
        /// 接口（托盘信息）
        /// </summary>
        private readonly IInteTrayService _inteTrayService;

        /// <summary>
        /// 构造函数（托盘信息）
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="inteTrayService"></param>
        public InteTrayController(ILogger<InteTrayController> logger, IInteTrayService inteTrayService)
        {
            _logger = logger;
            _inteTrayService = inteTrayService;
        }


        /// <summary>
        /// 分页查询列表（托盘信息）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("page")]
        public async Task<PagedInfo<InteTrayDto>> GetPagedListAsync([FromQuery] InteTrayPagedQueryDto parm)
        {
            return await _inteTrayService.GetPagedListAsync(parm);
        }

        /// <summary>
        /// 查询详情（托盘信息）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<InteTrayDto> QueryByIdAsync(long id)
        {
            return await _inteTrayService.QueryByIdAsync(id);
        }

        /// <summary>
        /// 添加（托盘信息）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task CreateAsync([FromBody] InteTraySaveDto parm)
        {
            await _inteTrayService.CreateAsync(parm);
        }

        /// <summary>
        /// 更新（托盘信息）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task ModifyAsync([FromBody] InteTraySaveDto parm)
        {
            await _inteTrayService.ModifyAsync(parm);
        }

        /// <summary>
        /// 删除（托盘信息）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task DeletesAsync([FromBody] long[] ids)
        {
            await _inteTrayService.DeletesAsync(ids);
        }

    }
}