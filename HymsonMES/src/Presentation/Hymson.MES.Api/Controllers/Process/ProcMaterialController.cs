using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;
using Hymson.MES.Services.Dtos.Common;
using Hymson.MES.Services.Dtos.Process;
using Hymson.MES.Services.Services.Process;
using Hymson.Web.Framework.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.Process
{
    /// <summary>
    /// 控制器（物料维护）
    /// @author Karl
    /// @date 2023-02-08 04:47:44
    /// </summary>

    [ApiController]
    [Route("api/v1/[controller]")]
    public class ProcMaterialController : ControllerBase
    {
        /// <summary>
        /// 接口（物料维护）
        /// </summary>
        private readonly IProcMaterialService _procMaterialService;
        private readonly ILogger<ProcMaterialController> _logger;

        /// <summary>
        /// 构造函数（物料维护）
        /// </summary>
        /// <param name="procMaterialService"></param>
        public ProcMaterialController(IProcMaterialService procMaterialService, ILogger<ProcMaterialController> logger)
        {
            _procMaterialService = procMaterialService;
            _logger = logger;
        }

        /// <summary>
        /// 分页查询列表（物料维护）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("pagelist")]
        public async Task<PagedInfo<ProcMaterialDto>> QueryPagedProcMaterialAsync([FromQuery] ProcMaterialPagedQueryDto parm)
        {
            return await _procMaterialService.GetPageListAsync(parm);
        }

        /// <summary>
        /// 查询列表（物料维护）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpGet("listforgroup")]
        public async Task<PagedInfo<ProcMaterialDto>> QueryProcMaterialForGroupAsync([FromQuery] ProcMaterialPagedQueryDto parm)
        {
            return await _procMaterialService.GetPageListForGroupAsync(parm);
        }

        /// <summary>
        /// 查询详情（物料维护）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ProcMaterialViewDto> QueryProcMaterialByIdAsync(long id)
        {
            return await _procMaterialService.QueryProcMaterialByIdAsync(id);
        }

        /// <summary>
        /// 查询物料关联的供应商信息（物料维护）
        /// </summary>
        /// <param name="materialId"></param>
        /// <returns></returns>
        [HttpGet("materialSupplier/{materialId}")]
        public async Task<List<ProcMaterialSupplierViewDto>> QueryProcMaterialSupplierByMaterialIdAsync(long materialId)
        {
            return await _procMaterialService.QueryProcMaterialSupplierByMaterialIdAsync(materialId);
        }

        /// <summary>
        /// 添加（物料维护）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("create")]
        [LogDescription("物料维护", BusinessType.INSERT)]
        [PermissionDescription("proc:material:insert")]
        public async Task AddProcMaterialAsync([FromBody] ProcMaterialCreateDto parm)
        {
             await _procMaterialService.CreateProcMaterialAsync(parm);
        }

        /// <summary>
        /// 更新（物料维护）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("update")]
        [LogDescription("物料维护", BusinessType.UPDATE)]
        [PermissionDescription("proc:material:update")]
        public async Task UpdateProcMaterialAsync([FromBody] ProcMaterialModifyDto parm)
        {
             await _procMaterialService.ModifyProcMaterialAsync(parm);
        }

        /// <summary>
        /// 删除（物料维护）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("delete")]
        [LogDescription("物料维护", BusinessType.DELETE)]
        [PermissionDescription("proc:material:delete")]
        public async Task<int> DeleteProcMaterialAsync([FromBody] long[] ids)
        {
            //long[] idsArr = StringExtension.SpitLongArrary(ids);
           return await _procMaterialService.DeletesProcMaterialAsync(ids);
        }


        #region 状态变更
        /// <summary>
        /// 启用（物料维护）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("updateStatusEnable")]
        [LogDescription("物料维护", BusinessType.UPDATE)]
        [PermissionDescription("proc:material:updateStatusEnable")]
        public async Task UpdateStatusEnable([FromBody] long id)
        {
            await _procMaterialService.UpdateStatusAsync(new ChangeStatusDto {Id=id,Status= SysDataStatusEnum.Enable });
        }

        /// <summary>
        /// 保留（物料维护）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("updateStatusRetain")]
        [LogDescription("物料维护", BusinessType.UPDATE)]
        [PermissionDescription("proc:material:updateStatusRetain")]
        public async Task UpdateStatusRetain([FromBody] long id)
        {
            await _procMaterialService.UpdateStatusAsync(new ChangeStatusDto { Id = id, Status = SysDataStatusEnum.Retain });
        }

        /// <summary>
        /// 废除（物料维护）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("updateStatusAbolish")]
        [LogDescription("物料维护", BusinessType.UPDATE)]
        [PermissionDescription("proc:material:updateStatusAbolish")]
        public async Task UpdateStatusAbolish([FromBody] long id)
        {
            await _procMaterialService.UpdateStatusAsync(new ChangeStatusDto { Id = id, Status = SysDataStatusEnum.Abolish });
        }

        #endregion

    }
}