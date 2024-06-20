using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Integrated;
using Hymson.MES.Services.Services.Integrated;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.Integrated
{
    /// <summary>
    /// 控制器（资质认证）
    /// @author zhaoqing
    /// @date 2024-06-18 06:00:07
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class InteQualificationAuthenticationController : ControllerBase
    {
        /// <summary>
        /// 日志
        /// </summary>
        private readonly ILogger<InteQualificationAuthenticationController> _logger;
        /// <summary>
        /// 服务接口（资质认证）
        /// </summary>
        private readonly IInteQualificationAuthenticationService _inteQualificationAuthenticationService;


        /// <summary>
        /// 构造函数（资质认证）
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="inteQualificationAuthenticationService"></param>
        public InteQualificationAuthenticationController(ILogger<InteQualificationAuthenticationController> logger, IInteQualificationAuthenticationService inteQualificationAuthenticationService)
        {
            _logger = logger;
            _inteQualificationAuthenticationService = inteQualificationAuthenticationService;
        }

        /// <summary>
        /// 添加（资质认证）
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("create")]
        public async Task AddAsync([FromBody] InteQualificationAuthenticationSaveDto saveDto)
        {
             await _inteQualificationAuthenticationService.CreateAsync(saveDto);
        }

        /// <summary>
        /// 更新（资质认证）
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("update")]
        public async Task UpdateAsync([FromBody] InteQualificationAuthenticationSaveDto saveDto)
        {
             await _inteQualificationAuthenticationService.ModifyAsync(saveDto);
        }

        /// <summary>
        /// 删除（资质认证）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("delete")]
        public async Task DeleteAsync([FromBody] long[] ids)
        {
            await _inteQualificationAuthenticationService.DeletesAsync(ids);
        }

        /// <summary>
        /// 查询详情（资质认证）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<InteQualificationAuthenticationDto?> QueryByIdAsync(long id)
        {
            return await _inteQualificationAuthenticationService.QueryByIdAsync(id);
        }

        /// <summary>
        /// 查询详情（资质认证）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("detail/{id}")]
        public async Task<IEnumerable<InteQualificationAuthenticationDetailsDto>> GetcDetailsAsync(long id)
        {
            return await _inteQualificationAuthenticationService.GetcDetailsAsync(id);
        }

        /// <summary>
        /// 分页查询列表（资质认证）
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("pagelist")]
        public async Task<PagedInfo<InteQualificationAuthenticationDto>> QueryPagedListAsync([FromQuery] InteQualificationAuthenticationPagedQueryDto pagedQueryDto)
        {
            return await _inteQualificationAuthenticationService.GetPagedListAsync(pagedQueryDto);
        }

    }
}