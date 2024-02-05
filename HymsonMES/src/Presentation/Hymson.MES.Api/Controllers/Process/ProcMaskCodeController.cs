using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Process;
using Hymson.MES.Services.Services.Process.MaskCode;
using Hymson.Web.Framework.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.Process
{
    /// <summary>
    /// 控制器（掩码维护）
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ProcMaskCodeController : ControllerBase
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly IProcMaskCodeService _procMaskCodeService;
        private readonly ILogger<ProcMaskCodeController> _logger;

        /// <summary>
        /// 构造函数（掩码维护）
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="procMaskCodeService"></param>
        public ProcMaskCodeController(ILogger<ProcMaskCodeController> logger, IProcMaskCodeService procMaskCodeService)
        {
            _procMaskCodeService = procMaskCodeService;
            _logger = logger;
        }

        /// <summary>
        /// 添加（掩码维护）
        /// </summary>
        /// <param name="createDto"></param>
        /// <returns></returns>
        [HttpPost]
        [LogDescription("掩码维护", BusinessType.INSERT)]
        [PermissionDescription("proc:maskCode:insert")]
        public async Task CreateAsync(ProcMaskCodeSaveDto createDto)
        {
            await _procMaskCodeService.CreateAsync(createDto);
        }

        /// <summary>
        /// 更新（掩码维护）
        /// </summary>
        /// <param name="modifyDto"></param>
        /// <returns></returns>
        [HttpPut]
        [LogDescription("掩码维护", BusinessType.UPDATE)]
        [PermissionDescription("proc:maskCode:update")]
        public async Task ModifyAsync(ProcMaskCodeSaveDto modifyDto)
        {
            await _procMaskCodeService.ModifyAsync(modifyDto);
        }

        /// <summary>
        /// 删除（掩码维护）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpDelete]
        [LogDescription("掩码维护", BusinessType.DELETE)]
        [PermissionDescription("proc:maskCode:delete")]
        public async Task DeletesAsync(long[] ids)
        {
            await _procMaskCodeService.DeletesAsync(ids);
        }

        /// <summary>
        /// 获取分页数据（掩码维护）
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        [Route("page")]
        [HttpGet]
        public async Task<PagedInfo<ProcMaskCodeDto>> GetPagedListAsync([FromQuery] ProcMaskCodePagedQueryDto pagedQueryDto)
        {
            return await _procMaskCodeService.GetPagedListAsync(pagedQueryDto);
        }

        /// <summary>
        /// 查询详情（掩码维护）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ProcMaskCodeDto> GetDetailAsync(long id)
        {
            return await _procMaskCodeService.GetDetailAsync(id);
        }
    }
}