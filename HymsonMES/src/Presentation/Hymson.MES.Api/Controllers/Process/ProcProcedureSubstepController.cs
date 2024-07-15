using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Common;
using Hymson.MES.Services.Dtos.Integrated;
using Hymson.MES.Services.Dtos.Process;
using Hymson.MES.Services.Services.Process;
using Hymson.MES.Services.Services.Process.Resource;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.Process
{
    /// <summary>
    /// 控制器（子步骤）
    /// @author zhaoqing
    /// @date 2024-07-02 04:28:03
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ProcProcedureSubstepController : ControllerBase
    {
        /// <summary>
        /// 日志
        /// </summary>
        private readonly ILogger<ProcProcedureSubstepController> _logger;
        /// <summary>
        /// 服务接口（子步骤）
        /// </summary>
        private readonly IProcProcedureSubstepService _procProcedureSubstepService;


        /// <summary>
        /// 构造函数（子步骤）
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="procProcedureSubstepService"></param>
        public ProcProcedureSubstepController(ILogger<ProcProcedureSubstepController> logger, IProcProcedureSubstepService procProcedureSubstepService)
        {
            _logger = logger;
            _procProcedureSubstepService = procProcedureSubstepService;
        }

        /// <summary>
        /// 添加（子步骤）
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("create")]
        public async Task AddAsync([FromBody] ProcProcedureSubstepSaveDto saveDto)
        {
             await _procProcedureSubstepService.CreateAsync(saveDto);
        }

        /// <summary>
        /// 更新（子步骤）
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("update")]
        public async Task UpdateAsync([FromBody] ProcProcedureSubstepSaveDto saveDto)
        {
             await _procProcedureSubstepService.ModifyAsync(saveDto);
        }

        /// <summary>
        /// 删除（子步骤）
        /// </summary>
        /// <param name="deleteDto"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("delete")]
        public async Task DeleteAsync(DeleteDto deleteDto)
        {
            await _procProcedureSubstepService.DeletesAsync(deleteDto.Ids);
        }

        /// <summary>
        /// 查询详情（子步骤）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ProcProcedureSubstepDto?> QueryByIdAsync(long id)
        {
            return await _procProcedureSubstepService.QueryByIdAsync(id);
        }

        /// <summary>
        /// 获取子步骤配置Job信息
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [Route("job/list")]
        [HttpGet]
        public async Task<PagedInfo<ProcedureJobReleationDto>> GetProcedureConfigJobLisAsynct([FromQuery] InteJobBusinessRelationPagedQueryDto query)
        {
            return await _procProcedureSubstepService.GetSubstepConfigJobListAsync(query);
        }

        /// <summary>
        /// 分页查询列表（子步骤）
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("pagelist")]
        public async Task<PagedInfo<ProcProcedureSubstepDto>> QueryPagedListAsync([FromQuery] ProcProcedureSubstepPagedQueryDto pagedQueryDto)
        {
            return await _procProcedureSubstepService.GetPagedListAsync(pagedQueryDto);
        }
    }
}