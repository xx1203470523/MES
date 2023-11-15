using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Common;
using Hymson.MES.Services.Dtos.Process;
using Hymson.MES.Services.Services.Process.LabelTemplate;
using Hymson.Web.Framework.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.Process
{
    /// <summary>
    /// 控制器（仓库标签模板）
    /// @author wxk
    /// @date 2023-03-09 02:51:26
    /// </summary>

    [ApiController]
    [Route("api/v1/[controller]")]
    public class ProcLabelTemplateController : ControllerBase
    {
        /// <summary>
        /// 接口（仓库标签模板）
        /// </summary>
        private readonly IProcLabelTemplateService _procLabelTemplateService;
        private readonly ILogger<ProcLabelTemplateController> _logger;

        /// <summary>
        /// 构造函数（仓库标签模板）
        /// </summary>
        /// <param name="procLabelTemplateService"></param>
        /// <param name="logger"></param>
        public ProcLabelTemplateController(IProcLabelTemplateService procLabelTemplateService, ILogger<ProcLabelTemplateController> logger)
        {
            _procLabelTemplateService = procLabelTemplateService;
            _logger = logger;
        }

        /// <summary>
        /// 分页查询列表（仓库标签模板）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("pagelist")]
        public async Task<PagedInfo<ProcLabelTemplateDto>> QueryPagedProcLabelTemplateAsync([FromQuery] ProcLabelTemplatePagedQueryDto parm)
        {
            return await _procLabelTemplateService.GetPageListAsync(parm);
        }

        /// <summary>
        /// 查询详情（仓库标签模板）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ProcLabelTemplateDto> QueryProcLabelTemplateByIdAsync(long id)
        {
            return await _procLabelTemplateService.QueryProcLabelTemplateByIdAsync(id);
        }

        /// <summary>
        /// 添加（仓库标签模板）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("create")]
        [LogDescription("标签模板", BusinessType.INSERT)]
        [PermissionDescription("proc:labelTemplate:insert")]
        public async Task AddProcLabelTemplateAsync([FromBody] ProcLabelTemplateCreateDto parm)
        {
            await _procLabelTemplateService.CreateProcLabelTemplateAsync(parm);
        }

        /// <summary>
        /// 更新（仓库标签模板）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("update")]
        [LogDescription("标签模板", BusinessType.UPDATE)]
        [PermissionDescription("proc:labelTemplate:update")]
        public async Task UpdateProcLabelTemplateAsync([FromBody] ProcLabelTemplateModifyDto parm)
        {
            await _procLabelTemplateService.ModifyProcLabelTemplateAsync(parm);
        }

        /// <summary>
        /// 删除（仓库标签模板）
        /// </summary>
        /// <param name="deleteDto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("delete")]
        [LogDescription("标签模板", BusinessType.DELETE)]
        [PermissionDescription("proc:labelTemplate:delete")]
        public async Task DeleteProcLabelTemplateAsync(DeleteDto deleteDto)
        {
            await _procLabelTemplateService.DeletesProcLabelTemplateAsync(deleteDto.Ids);
        }
        [HttpGet]
        [Route("preview/{id}")]
        public async Task<PreviewImageDataDto> PreviewProcLabelTemplateAsync(long id)
        {
            var foo = await _procLabelTemplateService.PreviewProcLabelTemplateAsync(id);
            return new PreviewImageDataDto() { base64Str = foo.base64Str, result = foo.result };
        }


        /// <summary>
        /// 查询详情（仓库标签模板的打印设计）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("getRelationByLabelTemplateId/{id}")]
        [AllowAnonymous]
        public async Task<ProcLabelTemplateRelationDto?> QueryProcLabelTemplateRelationByLabelTemplateIdAsync(long id)
        {
            return await _procLabelTemplateService.QueryProcLabelTemplateRelationByLabelTemplateIdAsync(id);
        }

    }
}