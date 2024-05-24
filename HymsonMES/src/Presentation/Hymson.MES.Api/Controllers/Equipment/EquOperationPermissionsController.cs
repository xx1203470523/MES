using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Equipment;
using Hymson.MES.Services.Services.Equipment;
using Hymson.Web.Framework.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.Equipment
{
    /// <summary>
    /// 控制器（设备维保权限）
    /// @author User
    /// @date 2024-04-03 04:49:49
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class EquOperationPermissionsController : ControllerBase
    {
        /// <summary>
        /// 日志
        /// </summary>
        private readonly ILogger<EquOperationPermissionsController> _logger;
        /// <summary>
        /// 服务接口（设备维保权限）
        /// </summary>
        private readonly IEquOperationPermissionsService _equOperationPermissionsService;

        /// <summary>
        /// 构造函数（设备维保权限）
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="equOperationPermissionsService"></param>
        public EquOperationPermissionsController(ILogger<EquOperationPermissionsController> logger, IEquOperationPermissionsService equOperationPermissionsService)
        {
            _logger = logger;
            _equOperationPermissionsService = equOperationPermissionsService;
        }

        /// <summary>
        /// 添加（设备维保权限）
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("create")]
        [LogDescription("设备维保权限", BusinessType.INSERT)]
        [PermissionDescription("equ:equOperationPermissions:insert")]
        public async Task AddAsync([FromBody] EquOperationPermissionsSaveDto saveDto)
        {
             await _equOperationPermissionsService.CreateAsync(saveDto);
        }

        /// <summary>
        /// 更新（设备维保权限）
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("update")]
        [LogDescription("设备维保权限", BusinessType.UPDATE)]
        [PermissionDescription("equ:equOperationPermissions:update")]
        public async Task UpdateAsync([FromBody] EquOperationPermissionsSaveDto saveDto)
        {
             await _equOperationPermissionsService.ModifyAsync(saveDto);
        }

        /// <summary>
        /// 删除（设备维保权限）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("delete")]
        [LogDescription("设备维保权限", BusinessType.DELETE)]
        [PermissionDescription("equ:equOperationPermissions:delete")]
        public async Task DeleteAsync([FromBody] long[] ids)
        {
            await _equOperationPermissionsService.DeletesAsync(ids);
        }

        /// <summary>
        /// 查询详情（设备维保权限）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<EquOperationPermissionsDto?> QueryByIdAsync(long id)
        {
            return await _equOperationPermissionsService.QueryByIdAsync(id);
        }

        /// <summary>
        /// 分页查询列表（设备维保权限）
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("pagelist")]
        public async Task<PagedInfo<EquOperationPermissionsDto>> QueryPagedListAsync([FromQuery] EquOperationPermissionsQueryDto pagedQueryDto)
        {
            return await _equOperationPermissionsService.GetPagedListAsync(pagedQueryDto);
        }
    }
}