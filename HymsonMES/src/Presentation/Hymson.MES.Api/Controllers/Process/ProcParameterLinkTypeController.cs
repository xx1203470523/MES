using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Process;
using Hymson.MES.Services.Services.Process;
using Hymson.Web.Framework.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.Process
{
    /// <summary>
    /// 控制器（标准参数关联类型表）
    /// @author Karl
    /// @date 2023-02-15 03:53:38
    /// </summary>

    [ApiController]
    [Route("api/v1/[controller]")]
    public class ProcParameterLinkTypeController : ControllerBase
    {
        /// <summary>
        /// 接口（标准参数关联类型表）
        /// </summary>
        private readonly IProcParameterLinkTypeService _procParameterLinkTypeService;
        private readonly ILogger<ProcParameterLinkTypeController> _logger;

        /// <summary>
        /// 构造函数（标准参数关联类型表）
        /// </summary>
        /// <param name="procParameterLinkTypeService"></param>
        public ProcParameterLinkTypeController(IProcParameterLinkTypeService procParameterLinkTypeService, ILogger<ProcParameterLinkTypeController> logger)
        {
            _procParameterLinkTypeService = procParameterLinkTypeService;
            _logger = logger;
        }

        /// <summary>
        /// 分页查询列表（标准参数关联类型表）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("pagelist")]
        public async Task<PagedInfo<ProcParameterLinkTypeViewDto>> QueryPagedProcParameterLinkTypeAsync([FromQuery] ProcParameterLinkTypePagedQueryDto parm)
        {
            return await _procParameterLinkTypeService.GetPageListAsync(parm);
        }

        /// <summary>
        /// 分页查询详情（设备/产品参数）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("detail")]
        public async Task<PagedInfo<ProcParameterLinkTypeViewDto>> QueryPagedProcParameterLinkTypeByTypeAsync([FromQuery] ProcParameterDetailPagerQueryDto parm)
        {
            return await _procParameterLinkTypeService.QueryPagedProcParameterLinkTypeByTypeAsync(parm);
        }

        /// <summary>
        /// 添加（标准参数关联类型表）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("create")]
        [LogDescription("关联参数", BusinessType.INSERT)]
       // [PermissionDescription("proc:parameterLink:insert")]
        public async Task AddProcParameterLinkTypeAsync([FromBody] ProcParameterLinkTypeCreateDto parm)
        {
             await _procParameterLinkTypeService.CreateProcParameterLinkTypeAsync(parm);
        }

        /// <summary>
        /// 更新（标准参数关联类型表）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("update")]
        [LogDescription("关联参数", BusinessType.UPDATE)]
       // [PermissionDescription("proc:parameterLink:update")]
        public async Task UpdateProcParameterLinkTypeAsync([FromBody] ProcParameterLinkTypeModifyDto parm)
        {
             await _procParameterLinkTypeService.ModifyProcParameterLinkTypeAsync(parm);
        }

        /// <summary>
        /// 删除（标准参数关联类型表）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("delete")]
        [LogDescription("关联参数", BusinessType.DELETE)]
       // [PermissionDescription("proc:parameterLink:delete")]
        public async Task<int> DeleteProcParameterLinkTypeAsync(long[] ids)
        {
            //long[] idsArr = StringExtension.SpitLongArrary(ids);
            return await _procParameterLinkTypeService.DeletesProcParameterLinkTypeAsync(ids);
        }

    }
}