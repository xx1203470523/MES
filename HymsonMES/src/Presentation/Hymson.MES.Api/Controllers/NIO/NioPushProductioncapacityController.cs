using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.NIO;
using Hymson.MES.Services.Services.NIO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.NIO
{
    /// <summary>
    /// 控制器（合作伙伴精益与生产能力）
    /// @author User
    /// @date 2024-08-30 09:59:11
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class NioPushProductioncapacityController : ControllerBase
    {
        /// <summary>
        /// 日志
        /// </summary>
        private readonly ILogger<NioPushProductioncapacityController> _logger;
        /// <summary>
        /// 服务接口（合作伙伴精益与生产能力）
        /// </summary>
        private readonly INioPushProductioncapacityService _nioPushProductioncapacityService;


        /// <summary>
        /// 构造函数（合作伙伴精益与生产能力）
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="nioPushProductioncapacityService"></param>
        public NioPushProductioncapacityController(ILogger<NioPushProductioncapacityController> logger, INioPushProductioncapacityService nioPushProductioncapacityService)
        {
            _logger = logger;
            _nioPushProductioncapacityService = nioPushProductioncapacityService;
        }

        /// <summary>
        /// 添加（合作伙伴精益与生产能力）
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("create")]
        public async Task AddAsync([FromBody] NioPushProductioncapacitySaveDto saveDto)
        {
             await _nioPushProductioncapacityService.CreateAsync(saveDto);
        }

        /// <summary>
        /// 更新（合作伙伴精益与生产能力）
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("update")]
        public async Task UpdateAsync([FromBody] NioPushProductioncapacitySaveDto saveDto)
        {
             await _nioPushProductioncapacityService.ModifyAsync(saveDto);
        }

        /// <summary>
        /// 删除（合作伙伴精益与生产能力）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("delete")]
        public async Task DeleteAsync([FromBody] long[] ids)
        {
            await _nioPushProductioncapacityService.DeletesAsync(ids);
        }

        /// <summary>
        /// 查询详情（合作伙伴精益与生产能力）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<NioPushProductioncapacityDto?> QueryByIdAsync(long id)
        {
            return await _nioPushProductioncapacityService.QueryByIdAsync(id);
        }

        /// <summary>
        /// 分页查询列表（合作伙伴精益与生产能力）
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("pagelist")]
        public async Task<PagedInfo<NioPushProductioncapacityDto>> QueryPagedListAsync([FromQuery] NioPushProductioncapacityPagedQueryDto pagedQueryDto)
        {
            return await _nioPushProductioncapacityService.GetPagedListAsync(pagedQueryDto);
        }

    }
}