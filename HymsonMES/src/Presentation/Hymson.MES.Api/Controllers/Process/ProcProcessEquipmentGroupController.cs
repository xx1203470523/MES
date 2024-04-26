using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Process;
using Hymson.MES.Services.Services.Process;
using Hymson.Web.Framework.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.Process
{
    /// <summary>
    /// 控制器（工艺设备组）
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ProcProcessEquipmentGroupController : ControllerBase
    {
        /// <summary>
        /// 日志
        /// </summary>
        private readonly ILogger<ProcProcessEquipmentGroupController> _logger;
        /// <summary>
        /// 服务接口（工艺设备组）
        /// </summary>
        private readonly IProcProcessEquipmentGroupService _procProcessEquipmentGroupService;


        /// <summary>
        /// 构造函数（工艺设备组）
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="procProcessEquipmentGroupService"></param>
        public ProcProcessEquipmentGroupController(ILogger<ProcProcessEquipmentGroupController> logger, IProcProcessEquipmentGroupService procProcessEquipmentGroupService)
        {
            _logger = logger;
            _procProcessEquipmentGroupService = procProcessEquipmentGroupService;
        }

        /// <summary>
        /// 添加（工艺设备组）
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("create")]
        [LogDescription("工艺设备组", BusinessType.INSERT)]
        public async Task<long> AddProcProcessEquipmentGroupAsync([FromBody] ProcProcessEquipmentGroupSaveDto saveDto)
        {
            return await _procProcessEquipmentGroupService.CreateProcProcessEquipmentGroupAsync(saveDto);
        }

        /// <summary>
        /// 更新（工艺设备组）
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("update")]
        [LogDescription("工艺设备组", BusinessType.UPDATE)]
        public async Task UpdateProcProcessEquipmentGroupAsync([FromBody] ProcProcessEquipmentGroupSaveDto saveDto)
        {
             await _procProcessEquipmentGroupService.ModifyProcProcessEquipmentGroupAsync(saveDto);
        }

        /// <summary>
        /// 删除（工艺设备组）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("delete")]
        [LogDescription("工艺设备组", BusinessType.DELETE)]
        public async Task DeleteProcProcessEquipmentGroupAsync([FromBody] long[] ids)
        {
            await _procProcessEquipmentGroupService.DeletesProcProcessEquipmentGroupAsync(ids);
        }

        /// <summary>
        /// 查询详情（工艺设备组）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ProcProcessEquipmentGroupDto?> QueryProcProcessEquipmentGroupByIdAsync(long id)
        {
            return await _procProcessEquipmentGroupService.QueryProcProcessEquipmentGroupByIdAsync(id);
        }

        /// <summary>
        /// 分页查询列表（工艺设备组）
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("pagelist")]
        public async Task<PagedInfo<ProcProcessEquipmentGroupListDto>> QueryPagedProcProcessEquipmentGroupAsync([FromQuery] ProcProcessEquipmentGroupPagedQueryDto pagedQueryDto)
        {
            return await _procProcessEquipmentGroupService.GetPagedListAsync(pagedQueryDto);
        }

    }
}