using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Integrated;
using Hymson.MES.Services.Services.Integrated.InteClass;
using Hymson.Web.Framework.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.Integrated
{
    /// <summary>
    /// 控制器（班制维护）
    /// @author 陈志谱
    /// @date 2023-02-10 08:55:55
    /// </summary>
    
    [ApiController]
    [Route("api/v1/[controller]")]
    public class InteClassController : ControllerBase
    {
        /// <summary>
        /// 接口（班制维护）
        /// </summary>
        private readonly IInteClassService _inteClassService;
        private readonly ILogger<InteClassController> _logger;

        /// <summary>
        /// 构造函数（班制维护）
        /// </summary>
        /// <param name="inteClassService"></param>
        /// <param name="logger"></param>
        public InteClassController(IInteClassService inteClassService, ILogger<InteClassController> logger)
        {
            _inteClassService = inteClassService;
            _logger = logger;
        }

        /// <summary>
        /// 添加（班制维护）
        /// </summary>
        /// <param name="createDto"></param>
        /// <returns></returns>
        [HttpPost]
        [LogDescription("班制维护", BusinessType.INSERT)]
        [PermissionDescription("inte:class:insert")]
        public async Task CreateAsync(InteClassSaveDto createDto)
        {
            await _inteClassService.CreateAsync(createDto);
        }

        /// <summary>
        /// 更新（班制维护）
        /// </summary>
        /// <param name="modifyDto"></param>
        /// <returns></returns>
        [HttpPut]
        [LogDescription("班制维护", BusinessType.UPDATE)]
        [PermissionDescription("inte:class:update")]
        public async Task ModifyAsync(InteClassSaveDto modifyDto)
        {
            await _inteClassService.ModifyAsync(modifyDto);
        }

        /// <summary>
        /// 删除（班制维护）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpDelete]
        [LogDescription("班制维护", BusinessType.DELETE)]
        [PermissionDescription("inte:class:delete")]
        public async Task DeletesAsync(long[] ids)
        {
            await _inteClassService.DeletesAsync(ids);
        }

        /// <summary>
        /// 查询列表（班制维护）
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("page")]
        //[PermissionDescription("inte:class:list")]
        public async Task<PagedInfo<InteClassDto>> GetPagedListAsync([FromQuery] InteClassPagedQueryDto pagedQueryDto)
        {
            return await _inteClassService.GetPagedListAsync(pagedQueryDto);
        }

        /// <summary>
        /// 查询详情（班制维护）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<InteClassWithDetailDto> GetDetailAsync(long id)
        {
            return await _inteClassService.GetDetailAsync(id);
        }

    }
}