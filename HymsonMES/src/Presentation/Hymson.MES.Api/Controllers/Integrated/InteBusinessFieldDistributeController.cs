using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Integrated;
using Hymson.MES.Services.Services.Integrated;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.Integrated
{
    /// <summary>
    /// 控制器（字段分配管理）
    /// @author User
    /// @date 2024-06-13 03:31:20
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class InteBusinessFieldDistributeController : ControllerBase
    {
        /// <summary>
        /// 日志
        /// </summary>
        private readonly ILogger<InteBusinessFieldDistributeController> _logger;
        /// <summary>
        /// 服务接口（字段分配管理）
        /// </summary>
        private readonly IInteBusinessFieldDistributeService _inteBusinessFieldDistributeService;


        /// <summary>
        /// 构造函数（字段分配管理）
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="inteBusinessFieldDistributeService"></param>
        public InteBusinessFieldDistributeController(ILogger<InteBusinessFieldDistributeController> logger, IInteBusinessFieldDistributeService inteBusinessFieldDistributeService)
        {
            _logger = logger;
            _inteBusinessFieldDistributeService = inteBusinessFieldDistributeService;
        }

        /// <summary>
        /// 添加（字段分配管理）
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("create")]
        public async Task AddAsync([FromBody] InteBusinessFieldDistributeSaveDto saveDto)
        {
             await _inteBusinessFieldDistributeService.CreateAsync(saveDto);
        }

        /// <summary>
        /// 更新（字段分配管理）
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("update")]
        public async Task UpdateAsync([FromBody] InteBusinessFieldDistributeSaveDto saveDto)
        {
             await _inteBusinessFieldDistributeService.ModifyAsync(saveDto);
        }

        /// <summary>
        /// 删除（字段分配管理）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("delete")]
        public async Task DeleteAsync([FromBody] long[] ids)
        {
            await _inteBusinessFieldDistributeService.DeletesAsync(ids);
        }

        /// <summary>
        /// 查询详情（字段分配管理）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<InteBusinessFieldDistributeDto?> QueryByIdAsync(long id)
        {
            return await _inteBusinessFieldDistributeService.QueryByIdAsync(id);
        }

        /// <summary>
        /// 分页查询列表（字段分配管理）
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("pagelist")]
        public async Task<PagedInfo<InteBusinessFieldDistributeDto>> QueryPagedListAsync([FromQuery] InteBusinessFieldDistributePagedQueryDto pagedQueryDto)
        {
            return await _inteBusinessFieldDistributeService.GetPagedListAsync(pagedQueryDto);
        }

    }
}