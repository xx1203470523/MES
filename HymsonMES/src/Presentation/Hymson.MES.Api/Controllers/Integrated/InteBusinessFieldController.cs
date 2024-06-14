using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Integrated;
using Hymson.MES.Services.Services.Integrated;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.Integrated
{
    /// <summary>
    /// 控制器（字段定义）
    /// @author User
    /// @date 2024-06-13 03:04:06
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class InteBusinessFieldController : ControllerBase
    {
        /// <summary>
        /// 日志
        /// </summary>
        private readonly ILogger<InteBusinessFieldController> _logger;
        /// <summary>
        /// 服务接口（字段定义）
        /// </summary>
        private readonly IInteBusinessFieldService _inteBusinessFieldService;


        /// <summary>
        /// 构造函数（字段定义）
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="inteBusinessFieldService"></param>
        public InteBusinessFieldController(ILogger<InteBusinessFieldController> logger, IInteBusinessFieldService inteBusinessFieldService)
        {
            _logger = logger;
            _inteBusinessFieldService = inteBusinessFieldService;
        }

        /// <summary>
        /// 添加（字段定义）
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("create")]
        public async Task AddAsync([FromBody] InteBusinessFieldSaveDto saveDto)
        {
             await _inteBusinessFieldService.CreateAsync(saveDto);
        }

        /// <summary>
        /// 更新（字段定义）
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("update")]
        public async Task UpdateAsync([FromBody] InteBusinessFieldSaveDto saveDto)
        {
             await _inteBusinessFieldService.ModifyAsync(saveDto);
        }

        /// <summary>
        /// 删除（字段定义）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("delete")]
        public async Task DeleteAsync([FromBody] long[] ids)
        {
            await _inteBusinessFieldService.DeletesAsync(ids);
        }

        /// <summary>
        /// 查询详情（字段定义）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<InteBusinessFieldDto?> QueryByIdAsync(long id)
        {
            return await _inteBusinessFieldService.QueryByIdAsync(id);
        }

        /// <summary>
        /// 分页查询列表（字段定义）
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("pagelist")]
        public async Task<PagedInfo<InteBusinessFieldDto>> QueryPagedListAsync([FromQuery] InteBusinessFieldPagedQueryDto pagedQueryDto)
        {
            return await _inteBusinessFieldService.GetPagedListAsync(pagedQueryDto);
        }

    }
}