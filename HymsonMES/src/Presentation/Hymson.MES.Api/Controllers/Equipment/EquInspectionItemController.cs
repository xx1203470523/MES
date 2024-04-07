using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Equipment;
using Hymson.MES.Services.Services.Equipment;
using Hymson.Web.Framework.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.Equipment
{
    /// <summary>
    /// 控制器（设备点检保养项目）
    /// @author User
    /// @date 2024-04-03 04:49:49
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class EquInspectionItemController : ControllerBase
    {
        /// <summary>
        /// 日志
        /// </summary>
        private readonly ILogger<EquInspectionItemController> _logger;
        /// <summary>
        /// 服务接口（设备点检保养项目）
        /// </summary>
        private readonly IEquInspectionItemService _equInspectionItemService;

        /// <summary>
        /// 构造函数（设备点检保养项目）
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="equInspectionItemService"></param>
        public EquInspectionItemController(ILogger<EquInspectionItemController> logger, IEquInspectionItemService equInspectionItemService)
        {
            _logger = logger;
            _equInspectionItemService = equInspectionItemService;
        }

        /// <summary>
        /// 添加（设备点检保养项目）
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        [HttpPost]
        [LogDescription("设备点检保养项目", BusinessType.INSERT)]
        [PermissionDescription("equ:equInspectionItem:insert")]
        public async Task AddAsync([FromBody] EquInspectionItemSaveDto saveDto)
        {
             await _equInspectionItemService.CreateAsync(saveDto);
        }

        /// <summary>
        /// 更新（设备点检保养项目）
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        [HttpPut]
        [LogDescription("设备点检保养项目", BusinessType.UPDATE)]
        [PermissionDescription("equ:equInspectionItem:update")]
        public async Task UpdateAsync([FromBody] EquInspectionItemSaveDto saveDto)
        {
             await _equInspectionItemService.ModifyAsync(saveDto);
        }

        /// <summary>
        /// 删除（设备点检保养项目）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpDelete]
        [LogDescription("设备点检保养项目", BusinessType.DELETE)]
        [PermissionDescription("equ:equInspectionItem:delete")]
        public async Task DeleteAsync([FromBody] long[] ids)
        {
            await _equInspectionItemService.DeletesAsync(ids);
        }

        /// <summary>
        /// 查询详情（设备点检保养项目）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<EquInspectionItemDto?> QueryByIdAsync(long id)
        {
            return await _equInspectionItemService.QueryByIdAsync(id);
        }

        /// <summary>
        /// 分页查询列表（设备点检保养项目）
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("pagelist")]
        public async Task<PagedInfo<EquInspectionItemDto>> QueryPagedListAsync([FromQuery] EquInspectionItemPagedQueryDto pagedQueryDto)
        {
            return await _equInspectionItemService.GetPagedListAsync(pagedQueryDto);
        }
    }
}