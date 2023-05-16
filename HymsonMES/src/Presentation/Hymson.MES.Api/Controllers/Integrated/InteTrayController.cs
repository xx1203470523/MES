/*
 *creator: Karl
 *
 *describe: 托盘信息    控制器 | 代码由框架生成  
 *builder:  chenjianxiong
 *build datetime: 2023-05-16 10:57:03
 */
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
        /// 接口（托盘信息）
        /// </summary>
        private readonly IInteTrayService _inteTrayService;
        private readonly ILogger<InteTrayController> _logger;

        /// <summary>
        /// 构造函数（托盘信息）
        /// </summary>
        /// <param name="inteTrayService"></param>
        public InteTrayController(IInteTrayService inteTrayService, ILogger<InteTrayController> logger)
        {
            _inteTrayService = inteTrayService;
            _logger = logger;
        }

        #region 框架生成方法

        /// <summary>
        /// 分页查询列表（托盘信息）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("pagelist")]
        public async Task<PagedInfo<InteTrayDto>> QueryPagedInteTrayAsync([FromQuery] InteTrayPagedQueryDto parm)
        {
            return await _inteTrayService.GetPagedListAsync(parm);
        }

        /// <summary>
        /// 查询详情（托盘信息）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<InteTrayDto> QueryInteTrayByIdAsync(long id)
        {
            return await _inteTrayService.QueryInteTrayByIdAsync(id);
        }

        /// <summary>
        /// 添加（托盘信息）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("create")]
        public async Task AddInteTrayAsync([FromBody] InteTrayCreateDto parm)
        {
             await _inteTrayService.CreateInteTrayAsync(parm);
        }

        /// <summary>
        /// 更新（托盘信息）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("update")]
        public async Task UpdateInteTrayAsync([FromBody] InteTrayModifyDto parm)
        {
             await _inteTrayService.ModifyInteTrayAsync(parm);
        }

        /// <summary>
        /// 删除（托盘信息）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("delete")]
        public async Task DeleteInteTrayAsync([FromBody] long[] ids)
        {
            await _inteTrayService.DeletesInteTrayAsync(ids);
        }

        #endregion
    }
}