using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Integrated;
using Hymson.MES.Services.Services.Integrated.InteIntefaceLog;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.Integrated
{
    /// <summary>
    /// 控制器（接口日志查询）
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class InterfaceLogController : ControllerBase
    {
        /// <summary>
        /// 日志
        /// </summary>
        private readonly ILogger<InterfaceLogController> _logger;

        private readonly IInteIntefaceLogService _inteIntefaceLogService;

        public InterfaceLogController(ILogger<InterfaceLogController> logger, IInteIntefaceLogService inteIntefaceLogService)
        {
            _logger = logger;
            _inteIntefaceLogService = inteIntefaceLogService;
        }

        /// <summary>
        /// 分页查询列表（接口日志）
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("pagelist")]
        public async Task<PagedInfo<InteIntefaceLogDto>> QueryPagedListAsync([FromQuery] InteIntefaceLogPagedQueryDto pagedQueryDto)
        {
            return await _inteIntefaceLogService.GetPagedListAsync(pagedQueryDto);
        }

        /// <summary>
        /// 获取详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<InteIntefaceLogDto> QueryByIdAsync(long id)
        {
            var query = new InteIntefaceLogPagedQueryDto() { Id = id.ToString(), PageSize = 10, PageIndex = 1 };
            var dtos = await _inteIntefaceLogService.GetPagedListAsync(query);
            return dtos.Data.FirstOrDefault() ?? new InteIntefaceLogDto();
        }

    }
}
