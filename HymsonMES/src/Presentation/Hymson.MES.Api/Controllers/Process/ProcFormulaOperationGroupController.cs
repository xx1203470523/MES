using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Process;
using Hymson.MES.Services.Services.Process;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.Process
{
    /// <summary>
    /// 控制器（配方操作组）
    /// @author hjy
    /// @date 2023-12-15 02:09:27
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ProcFormulaOperationGroupController : ControllerBase
    {
        /// <summary>
        /// 日志
        /// </summary>
        private readonly ILogger<ProcFormulaOperationGroupController> _logger;
        /// <summary>
        /// 服务接口（配方操作组）
        /// </summary>
        private readonly IProcFormulaOperationGroupService _procFormulaOperationGroupService;


        /// <summary>
        /// 构造函数（配方操作组）
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="procFormulaOperationGroupService"></param>
        public ProcFormulaOperationGroupController(ILogger<ProcFormulaOperationGroupController> logger, IProcFormulaOperationGroupService procFormulaOperationGroupService)
        {
            _logger = logger;
            _procFormulaOperationGroupService = procFormulaOperationGroupService;
        }

        /// <summary>
        /// 添加（配方操作组）
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("create")]
        public async Task AddProcFormulaOperationGroupAsync([FromBody] AddFormulaOperationGroupDto saveDto)
        {
             await _procFormulaOperationGroupService.CreateProcFormulaOperationGroupAsync(saveDto);
        }

        /// <summary>
        /// 更新（配方操作组）
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("update")]
        public async Task UpdateProcFormulaOperationGroupAsync([FromBody] AddFormulaOperationGroupDto saveDto)
        {
             await _procFormulaOperationGroupService.ModifyProcFormulaOperationGroupAsync(saveDto);
        }

        /// <summary>
        /// 删除（配方操作组）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("delete")]
        public async Task DeleteProcFormulaOperationGroupAsync([FromBody] long[] ids)
        {
            await _procFormulaOperationGroupService.DeletesProcFormulaOperationGroupAsync(ids);
        }

        /// <summary>
        /// 查询详情（配方操作组）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ProcFormulaOperationGroupDto?> QueryProcFormulaOperationGroupByIdAsync(long id)
        {
            return await _procFormulaOperationGroupService.QueryProcFormulaOperationGroupByIdAsync(id);
        }

        /// <summary>
        /// 分页查询列表（配方操作组）
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("pagelist")]
        public async Task<PagedInfo<ProcFormulaOperationGroupDto>> QueryPagedProcFormulaOperationGroupAsync([FromQuery] ProcFormulaOperationGroupPagedQueryDto pagedQueryDto)
        {
            return await _procFormulaOperationGroupService.GetPagedListAsync(pagedQueryDto);
        }

        /// <summary>
        /// 获取配方操作信息
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("operation/list")]
        public async Task<PagedInfo<ProcFormulaOperationDto>> GetFormulaOperationListAsync([FromQuery] OperationGroupGetOperationPagedQueryDto pagedQueryDto)
        {
            return await _procFormulaOperationGroupService.GetFormulaOperationListAsync(pagedQueryDto);
        }

    }
}