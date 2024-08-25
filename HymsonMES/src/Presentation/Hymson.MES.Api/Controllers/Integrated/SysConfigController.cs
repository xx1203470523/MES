using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Integrated;
using Hymson.MES.Services.Services.Integrated;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.Integrated
{
    /// <summary>
    /// 控制器（系统配置）
    /// @author Yxx
    /// @date 2024-08-25 10:06:18
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class SysConfigController : ControllerBase
    {
        /// <summary>
        /// 日志
        /// </summary>
        private readonly ILogger<SysConfigController> _logger;
        /// <summary>
        /// 服务接口（系统配置）
        /// </summary>
        private readonly ISysConfigService _sysConfigService;


        /// <summary>
        /// 构造函数（系统配置）
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="sysConfigService"></param>
        public SysConfigController(ILogger<SysConfigController> logger, ISysConfigService sysConfigService)
        {
            _logger = logger;
            _sysConfigService = sysConfigService;
        }

        /// <summary>
        /// 添加（系统配置）
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("create")]
        public async Task AddAsync([FromBody] SysConfigSaveDto saveDto)
        {
             await _sysConfigService.CreateAsync(saveDto);
        }

        /// <summary>
        /// 更新（系统配置）
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("update")]
        public async Task UpdateAsync([FromBody] SysConfigSaveDto saveDto)
        {
             await _sysConfigService.ModifyAsync(saveDto);
        }

        /// <summary>
        /// 删除（系统配置）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("delete")]
        public async Task DeleteAsync([FromBody] long[] ids)
        {
            await _sysConfigService.DeletesAsync(ids);
        }

        /// <summary>
        /// 查询详情（系统配置）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<SysConfigDto?> QueryByIdAsync(long id)
        {
            return await _sysConfigService.QueryByIdAsync(id);
        }

        /// <summary>
        /// 分页查询列表（系统配置）
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("pagelist")]
        public async Task<PagedInfo<SysConfigDto>> QueryPagedListAsync([FromQuery] SysConfigPagedQueryDto pagedQueryDto)
        {
            return await _sysConfigService.GetPagedListAsync(pagedQueryDto);
        }

    }
}