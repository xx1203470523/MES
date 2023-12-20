using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Equipment;
using Hymson.MES.Services.Services.Equipment;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.Equipment
{
    /// <summary>
    /// 控制器（设备故障解决措施）
    /// @author Czhipu
    /// @date 2023-12-19 07:11:01
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class EquFaultSolutionController : ControllerBase
    {
        /// <summary>
        /// 日志
        /// </summary>
        private readonly ILogger<EquFaultSolutionController> _logger;
        /// <summary>
        /// 服务接口（设备故障解决措施）
        /// </summary>
        private readonly IEquFaultSolutionService _equFaultSolutionService;


        /// <summary>
        /// 构造函数（设备故障解决措施）
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="equFaultSolutionService"></param>
        public EquFaultSolutionController(ILogger<EquFaultSolutionController> logger, IEquFaultSolutionService equFaultSolutionService)
        {
            _logger = logger;
            _equFaultSolutionService = equFaultSolutionService;
        }

        /// <summary>
        /// 添加（设备故障解决措施）
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("create")]
        public async Task AddAsync([FromBody] EquFaultSolutionSaveDto saveDto)
        {
             await _equFaultSolutionService.CreateAsync(saveDto);
        }

        /// <summary>
        /// 更新（设备故障解决措施）
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("update")]
        public async Task UpdateAsync([FromBody] EquFaultSolutionSaveDto saveDto)
        {
             await _equFaultSolutionService.ModifyAsync(saveDto);
        }

        /// <summary>
        /// 删除（设备故障解决措施）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("delete")]
        public async Task DeleteAsync([FromBody] long[] ids)
        {
            await _equFaultSolutionService.DeletesAsync(ids);
        }

        /// <summary>
        /// 查询详情（设备故障解决措施）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<EquFaultSolutionDto?> QueryByIdAsync(long id)
        {
            return await _equFaultSolutionService.QueryByIdAsync(id);
        }

        /// <summary>
        /// 分页查询列表（设备故障解决措施）
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("pagelist")]
        public async Task<PagedInfo<EquFaultSolutionDto>> QueryPagedListAsync([FromQuery] EquFaultSolutionPagedQueryDto pagedQueryDto)
        {
            return await _equFaultSolutionService.GetPagedListAsync(pagedQueryDto);
        }

    }
}