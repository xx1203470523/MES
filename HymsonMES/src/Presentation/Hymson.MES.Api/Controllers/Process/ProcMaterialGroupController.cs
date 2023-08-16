using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Process;
using Hymson.MES.Services.Services.Process;
using Hymson.Web.Framework.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.Process
{
    /// <summary>
    /// 控制器（物料组维护表）
    /// @author Karl
    /// @date 2023-02-10 03:54:07
    /// </summary>

    [ApiController]
    [Route("api/v1/[controller]")]
    public class ProcMaterialGroupController : ControllerBase
    {
        /// <summary>
        /// 接口（物料组维护表）
        /// </summary>
        private readonly IProcMaterialGroupService _procMaterialGroupService;
        private readonly ILogger<ProcMaterialGroupController> _logger;

        /// <summary>
        /// 构造函数（物料组维护表）
        /// </summary>
        /// <param name="procMaterialGroupService"></param>
        public ProcMaterialGroupController(IProcMaterialGroupService procMaterialGroupService, ILogger<ProcMaterialGroupController> logger)
        {
            _procMaterialGroupService = procMaterialGroupService;
            _logger = logger;
        }


        /// <summary>
        /// 分页查询列表（物料组维护表）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("pagelist")]
        public async Task<PagedInfo<ProcMaterialGroupDto>> QueryPagedProcMaterialGroupAsync([FromQuery] ProcMaterialGroupPagedQueryDto parm)
        {
            return await _procMaterialGroupService.GetPageListAsync(parm);
        }

        /// <summary>
        /// 分页查询自定义列表（物料组维护表）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("customlist")]
        public async Task<PagedInfo<CustomProcMaterialGroupViewDto>> QueryPagedCustomProcMaterialGroupAsync([FromQuery] CustomProcMaterialGroupPagedQueryDto parm)
        {
            return await _procMaterialGroupService.GetPageCustomListAsync(parm);
        }

        /// <summary>
        /// 查询详情（物料组维护表）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ProcMaterialGroupDto> QueryProcMaterialGroupByIdAsync(long id)
        {
            return await _procMaterialGroupService.QueryProcMaterialGroupByIdAsync(id);
        }

        /// <summary>
        /// 添加（物料组维护表）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("create")]
        [LogDescription("物料组维护", BusinessType.INSERT)]
        [PermissionDescription("proc:materialGroup:insert")]
        public async Task AddProcMaterialGroupAsync([FromBody] ProcMaterialGroupCreateDto parm)
        {
             await _procMaterialGroupService.CreateProcMaterialGroupAsync(parm);
        }

        /// <summary>
        /// 更新（物料组维护表）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("update")]
        [LogDescription("物料组维护", BusinessType.UPDATE)]
        [PermissionDescription("proc:materialGroup:update")]
        public async Task UpdateProcMaterialGroupAsync([FromBody] ProcMaterialGroupModifyDto parm)
        {
             await _procMaterialGroupService.ModifyProcMaterialGroupAsync(parm);
        }

        /// <summary>
        /// 删除（物料组维护表）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("delete")]
        [LogDescription("物料组维护", BusinessType.DELETE)]
        [PermissionDescription("proc:materialGroup:delete")]
        public async Task DeleteProcMaterialGroupAsync(long[] ids)
        {
            //long[] idsArr = StringExtension.SpitLongArrary(ids);
            await _procMaterialGroupService.DeletesProcMaterialGroupAsync(ids);
        }

    }
}