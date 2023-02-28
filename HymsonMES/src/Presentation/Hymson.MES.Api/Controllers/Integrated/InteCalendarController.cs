using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Equipment;
using Hymson.MES.Services.Dtos.Integrated;
using Hymson.MES.Services.Services.Integrated.InteCalendar;
using Hymson.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.Integrated
{
    /// <summary>
    /// 控制器（日历维护）
    /// @author 陈志谱
    /// @date 2023-02-11 10:03:48
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class InteCalendarController : ControllerBase
    {
        /// <summary>
        /// 接口（日历维护）
        /// </summary>
        private readonly IInteCalendarService _inteCalendarService;
        private readonly ILogger<InteCalendarController> _logger;

        /// <summary>
        /// 构造函数（日历维护）
        /// </summary>
        /// <param name="inteCalendarService"></param>
        /// <param name="logger"></param>
        public InteCalendarController(IInteCalendarService inteCalendarService, ILogger<InteCalendarController> logger)
        {
            _inteCalendarService = inteCalendarService;
            _logger = logger;
        }


        /// <summary>
        /// 添加（日历维护）
        /// </summary>
        /// <param name="createDto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task CreateAsync([FromBody] InteCalendarCreateDto createDto)
        {
            await _inteCalendarService.CreateAsync(createDto);
        }

        /// <summary>
        /// 更新（日历维护）
        /// </summary>
        /// <param name="modifyDto"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task ModifyAsync([FromBody] InteCalendarModifyDto modifyDto)
        {
            await _inteCalendarService.ModifyAsync(modifyDto);
        }

        /// <summary>
        /// 删除（日历维护）
        /// </summary>
        /// <param name="deleteDto"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task DeletesAsync(InteCalendarDeleteDto deleteDto)
        {
            await _inteCalendarService.DeletesAsync(deleteDto.Ids);
        }

        /// <summary>
        /// 查询列表（日历维护）
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        [HttpGet("page")]
        public async Task<PagedInfo<InteCalendarDto>> GetPagedListAsync([FromQuery] InteCalendarPagedQueryDto pagedQueryDto)
        {
            return await _inteCalendarService.GetPagedListAsync(pagedQueryDto);
        }

        /// <summary>
        /// 查询详情（日历维护）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<InteCalendarDetailDto> GetDetailAsync(long id)
        {
            return await _inteCalendarService.GetDetailAsync(id);
        }

    }
}