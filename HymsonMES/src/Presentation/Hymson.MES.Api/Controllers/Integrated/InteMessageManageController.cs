using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Integrated;
using Hymson.MES.Services.Services.Integrated;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.Integrated
{
    /// <summary>
    /// 控制器（消息管理）
    /// @author Czhipu
    /// @date 2023-08-15 08:47:52
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class InteMessageManageController : ControllerBase
    {
        /// <summary>
        /// 日志
        /// </summary>
        private readonly ILogger<InteMessageManageController> _logger;
        /// <summary>
        /// 服务接口（消息管理）
        /// </summary>
        private readonly IInteMessageManageService _inteMessageManageService;


        /// <summary>
        /// 构造函数（消息管理）
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="inteMessageManageService"></param>
        public InteMessageManageController(ILogger<InteMessageManageController> logger, IInteMessageManageService inteMessageManageService)
        {
            _logger = logger;
            _inteMessageManageService = inteMessageManageService;
        }

        /// <summary>
        /// 添加（消息管理）
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("create")]
        public async Task AddAsync([FromBody] InteMessageManageSaveDto saveDto)
        {
             await _inteMessageManageService.CreateAsync(saveDto);
        }

        /// <summary>
        /// 更新（消息管理）
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("update")]
        public async Task UpdateAsync([FromBody] InteMessageManageSaveDto saveDto)
        {
             await _inteMessageManageService.ModifyAsync(saveDto);
        }

        /// <summary>
        /// 删除（消息管理）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("delete")]
        public async Task DeleteAsync([FromBody] long[] ids)
        {
            await _inteMessageManageService.DeletesAsync(ids);
        }

        /// <summary>
        /// 查询详情（消息管理）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<InteMessageManageDto?> QueryByIdAsync(long id)
        {
            return await _inteMessageManageService.QueryByIdAsync(id);
        }

        /// <summary>
        /// 分页查询列表（消息管理）
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("pagelist")]
        public async Task<PagedInfo<InteMessageManageDto>> QueryPagedListAsync([FromQuery] InteMessageManagePagedQueryDto pagedQueryDto)
        {
            return await _inteMessageManageService.GetPagedListAsync(pagedQueryDto);
        }

    }
}