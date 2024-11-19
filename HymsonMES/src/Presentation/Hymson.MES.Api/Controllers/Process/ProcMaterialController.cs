using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;
using Hymson.MES.CoreServices.Dtos.Common;
using Hymson.MES.Services.Dtos.Common;
using Hymson.MES.Services.Dtos.Process;
using Hymson.MES.Services.Services.Process;
using Hymson.Web.Framework.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.Process
{
    /// <summary>
    /// 控制器（物料维护）
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
        /// <param name="logger"></param>
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
        /// 分页查询列表（物料维护）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("pagelistByWms")]
        public async Task<PagedInfo<ProcMaterialDto>> QueryPagedProcMaterialByWmsAsync([FromQuery] ProcMaterialPagedQueryDto parm)
        {
            return await _procMaterialService.GetPageListByWmsAsync(parm);
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
        /// 查询物料关联的供应商信息（物料维护）
        /// </summary>
        /// <param name="materialId"></param>
        /// <returns></returns>
        [HttpGet("suppliers/{materialId}")]
        public async Task<IEnumerable<SelectOptionDto>> QuerySuppliersAsync(long materialId)
        {
            return await _procMaterialService.QuerySuppliersAsync(materialId);
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
        public async Task<long> AddProcMaterialAsync([FromBody] ProcMaterialCreateDto parm)
        {
            return await _procMaterialService.CreateProcMaterialAsync(parm);
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
        public async Task DeleteProcMaterialAsync([FromBody] long[] ids)
        {
            await _procMaterialService.DeletesProcMaterialAsync(ids);
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
            await _procMaterialService.UpdateStatusAsync(new ChangeStatusDto { Id = id, Status = SysDataStatusEnum.Enable });
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

        /// <summary>
        /// 导入物料数据
        /// </summary>
        /// <param name="formFile"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("import")]
        [LogDescription("导入物料数据", BusinessType.EXPORT)]
        [PermissionDescription("proc:material:import")]
        public async Task ImportProcMaterialAsync([FromForm(Name = "file")] IFormFile formFile)
        {
            await _procMaterialService.ImportProcMaterialAsync(formFile);
        }

        /// <summary>
        /// 导入模板下载
        /// </summary>
        /// <returns></returns>
        [HttpGet("downloadImportTemplate")]
        [LogDescription("导入模板下载", BusinessType.EXPORT, IsSaveRequestData = false, IsSaveResponseData = false)]
        public async Task<IActionResult> DownloadTemplateExcel()
        {
            using MemoryStream stream = new MemoryStream();
            await _procMaterialService.DownloadImportTemplateAsync(stream);
            return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"物料导入模板.xlsx");
        }

        /// <summary>
        /// 导出物料信息
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("export")]
        [PermissionDescription("proc:material:export")]
        public async Task<ProcMaterialExportResultDto> ExprotProcMaterialListAsync([FromQuery] ProcMaterialPagedQueryDto param)
        {
            return await _procMaterialService.ExprotProcMaterialListAsync(param);
        }

    }
}