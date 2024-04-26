using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Process;
using Hymson.MES.Services.Services.Process;
using Hymson.Web.Framework.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.Process
{
    /// <summary>
    /// 控制器（设备组关联设备表）
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ProcProcessEquipmentGroupRelationController : ControllerBase
    {
        /// <summary>
        /// 日志
        /// </summary>
        private readonly ILogger<ProcProcessEquipmentGroupRelationController> _logger;
        /// <summary>
        /// 服务接口（设备组关联设备表）
        /// </summary>
        private readonly IProcProcessEquipmentGroupRelationService _procProcessEquipmentGroupRelationService;


        /// <summary>
        /// 构造函数（设备组关联设备表）
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="procProcessEquipmentGroupRelationService"></param>
        public ProcProcessEquipmentGroupRelationController(ILogger<ProcProcessEquipmentGroupRelationController> logger, IProcProcessEquipmentGroupRelationService procProcessEquipmentGroupRelationService)
        {
            _logger = logger;
            _procProcessEquipmentGroupRelationService = procProcessEquipmentGroupRelationService;
        }

        /// <summary>
        /// 添加（设备组关联设备表）
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("create")]
        [LogDescription("设备组关联设备表", BusinessType.INSERT)]
        public async Task AddProcProcessEquipmentGroupRelationAsync([FromBody] ProcProcessEquipmentGroupRelationSaveDto saveDto)
        {
             await _procProcessEquipmentGroupRelationService.CreateProcProcessEquipmentGroupRelationAsync(saveDto);
        }

        /// <summary>
        /// 更新（设备组关联设备表）
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("update")]
        [LogDescription("设备组关联设备表", BusinessType.UPDATE)]
        public async Task UpdateProcProcessEquipmentGroupRelationAsync([FromBody] ProcProcessEquipmentGroupRelationSaveDto saveDto)
        {
             await _procProcessEquipmentGroupRelationService.ModifyProcProcessEquipmentGroupRelationAsync(saveDto);
        }

        /// <summary>
        /// 删除（设备组关联设备表）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("delete")]
        [LogDescription("设备组关联设备表", BusinessType.DELETE)]
        public async Task DeleteProcProcessEquipmentGroupRelationAsync([FromBody] long[] ids)
        {
            await _procProcessEquipmentGroupRelationService.DeletesProcProcessEquipmentGroupRelationAsync(ids);
        }

        /// <summary>
        /// 查询详情（设备组关联设备表）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ProcProcessEquipmentGroupRelationDto?> QueryProcProcessEquipmentGroupRelationByIdAsync(long id)
        {
            return await _procProcessEquipmentGroupRelationService.QueryProcProcessEquipmentGroupRelationByIdAsync(id);
        }

        /// <summary>
        /// 分页查询列表（设备组关联设备表）
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("pagelist")]
        public async Task<PagedInfo<ProcProcessEquipmentGroupRelationDto>> QueryPagedProcProcessEquipmentGroupRelationAsync([FromQuery] ProcProcessEquipmentGroupRelationPagedQueryDto pagedQueryDto)
        {
            return await _procProcessEquipmentGroupRelationService.GetPagedListAsync(pagedQueryDto);
        }

    }
}