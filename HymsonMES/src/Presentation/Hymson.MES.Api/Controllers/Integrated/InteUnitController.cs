using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Integrated;
using Hymson.MES.Services.Services.Integrated;
using Hymson.Web.Framework.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.Integrated
{
    /// <summary>
    /// 控制器（单位维护）
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class InteUnitController : ControllerBase
    {
        /// <summary>
        /// 日志
        /// </summary>
        private readonly ILogger<InteUnitController> _logger;
        /// <summary>
        /// 服务接口（单位维护）
        /// </summary>
        private readonly IInteUnitService _inteUnitService;


        /// <summary>
        /// 构造函数（单位维护）
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="inteUnitService"></param>
        public InteUnitController(ILogger<InteUnitController> logger, IInteUnitService inteUnitService)
        {
            _logger = logger;
            _inteUnitService = inteUnitService;
        }

        /// <summary>
        /// 添加（单位维护）
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
       
        
        [HttpPost]
        [Route("create")]
        [LogDescription("单位维护", BusinessType.INSERT)]
        public async Task<long> AddInteUnitAsync([FromBody] InteUnitSaveDto saveDto)
        {
           return  await _inteUnitService.CreateInteUnitAsync(saveDto);
        }

        /// <summary>
        /// 更新（单位维护）
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("update")]
        [LogDescription("单位维护", BusinessType.UPDATE)]
        public async Task UpdateInteUnitAsync([FromBody] InteUnitSaveDto saveDto)
        {
             await _inteUnitService.ModifyInteUnitAsync(saveDto);
        }

        /// <summary>
        /// 删除（单位维护）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("delete")]
        [LogDescription("单位维护", BusinessType.DELETE)]
        public async Task DeleteInteUnitAsync([FromBody] long[] ids)
        {
            await _inteUnitService.DeletesInteUnitAsync(ids);
        }

        /// <summary>
        /// 查询详情（单位维护）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<InteUnitDto?> QueryInteUnitByIdAsync(long id)
        {
            return await _inteUnitService.QueryInteUnitByIdAsync(id);
        }

        /// <summary>
        /// 分页查询列表（单位维护）
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("pagelist")]
        public async Task<PagedInfo<InteUnitDto>> QueryPagedInteUnitAsync([FromQuery] InteUnitPagedQueryDto pagedQueryDto)
        {
            return await _inteUnitService.GetPagedListAsync(pagedQueryDto);
        }

    }
}