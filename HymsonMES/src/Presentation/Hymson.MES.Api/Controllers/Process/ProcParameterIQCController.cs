using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Process;
using Hymson.MES.Services.Services.Process;
using Hymson.Web.Framework.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.Process
{
    /// <summary>
    /// 控制器（质量参数）
    /// @author zhaoqing
    /// @date 2023-06-25 08:57:38
    /// </summary>

    [ApiController]
    [Route("api/v1/[controller]")]
    public class ProcParameterIQCController : ControllerBase
    {
        /// <summary>
        /// 接口（标准参数关联类型表）
        /// </summary>
        private readonly IProcParameterLinkTypeService _procParameterLinkTypeService;
        private readonly ILogger<ProcParameterLinkTypeController> _logger;

        /// <summary>
        /// 构造函数（质量参数）
        /// </summary>
        /// <param name="procParameterLinkTypeService"></param>
        public ProcParameterIQCController(IProcParameterLinkTypeService procParameterLinkTypeService, ILogger<ProcParameterLinkTypeController> logger)
        {
            _procParameterLinkTypeService = procParameterLinkTypeService;
            _logger = logger;
        }


        /// <summary>
        /// 添加（质量参数）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("create")]
        [LogDescription("质量参数", BusinessType.INSERT)]
        [PermissionDescription("proc:parameterIQC:insert")]
        public async Task AddProcParameterLinkTypeAsync([FromBody] ProcParameterLinkTypeCreateDto parm)
        {
             await _procParameterLinkTypeService.CreateProcParameterLinkTypeAsync(parm);
        }

        /// <summary>
        /// 更新（质量参数）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("update")]
        [LogDescription("质量参数", BusinessType.UPDATE)]
        [PermissionDescription("proc:parameterIQC:update")]
        public async Task UpdateProcParameterLinkTypeAsync([FromBody] ProcParameterLinkTypeModifyDto parm)
        {
             await _procParameterLinkTypeService.ModifyProcParameterLinkTypeAsync(parm);
        }

        /// <summary>
        /// 删除（质量参数）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("delete")]
        [LogDescription("质量参数", BusinessType.DELETE)]
        [PermissionDescription("proc:parameterIQC:delete")]
        public async Task<int> DeleteProcParameterLinkTypeAsync(long[] ids)
        {
            return await _procParameterLinkTypeService.DeletesProcParameterLinkTypeAsync(ids);
        }

    }
}