    using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Quality;
using Hymson.MES.Services.Dtos.Warehouse;
using Hymson.MES.Services.Services.Warehouse;
using Hymson.Web.Framework.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.Warehouse
{
    /// <summary>
    /// 控制器（供应商）
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class WhSupplierController : ControllerBase
    {
        /// <summary>
        /// 接口（供应商）
        /// </summary>
        private readonly IWhSupplierService _whSupplierService;
        private readonly ILogger<WhSupplierController> _logger;

        /// <summary>
        /// 构造函数（供应商）
        /// </summary>
        /// <param name="whSupplierService"></param>
        /// <param name="logger"></param>
        public WhSupplierController(IWhSupplierService whSupplierService, ILogger<WhSupplierController> logger)
        {
            _whSupplierService = whSupplierService;
            _logger = logger;
        }

        /// <summary>
        /// 分页查询列表
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("pagelistNew")]
        public async Task<PagedInfo<WhSupplierDto>> GetPagedListAsync([FromQuery] WhSupplierPagedQueryDto pagedQueryDto)
        {
            return await _whSupplierService.GetPageListAsync(pagedQueryDto);
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("createNew")]
        [LogDescription("供应商管理", BusinessType.INSERT)]
        [PermissionDescription("integrated:inteSupplier:insert")]
        public async Task AddAsync([FromBody] WhSupplierCreateDto saveDto)
        {
            await _whSupplierService.CreateWhSupplierAsync(saveDto);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("updateNew")]
        [LogDescription("供应商管理", BusinessType.UPDATE)]
        [PermissionDescription("integrated:inteSupplier:update")]
        public async Task UpdateAsync([FromBody] WhSupplierModifyDto saveDto)
        {
            await _whSupplierService.ModifyWhSupplierAsync(saveDto);
        }



        /// <summary>
        /// 分页查询列表（供应商）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("pagelist")]
        [LogDescription("供应商管理", BusinessType.OTHER)]
        public async Task<PagedInfo<WhSupplierDto>> QueryPagedWhSupplierAsync(WhSupplierPagedQueryDto parm)
        {
            return await _whSupplierService.GetPageListAsync(parm);
        }

        /// <summary>
        /// 查询详情（供应商）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<WhSupplierDto> QueryWhSupplierByIdAsync(long id)
        {
            return await _whSupplierService.QueryWhSupplierByIdAsync(id);
        }

        /// <summary>
        /// 根据ID查询(更改供应商)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("updatedto/{id}")]
        public async Task<UpdateWhSupplierDto> QueryUpdateWhSupplierByIdAsync(long id)
        {
            return await _whSupplierService.QueryUpdateWhSupplierByIdAsync(id);
        }

        /// <summary>
        /// 添加（供应商）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("create")]
        [LogDescription("供应商管理", BusinessType.INSERT)]
        [PermissionDescription("wh:supplier:insert")]
        public async Task AddWhSupplierAsync([FromBody] WhSupplierCreateDto parm)
        {
            await _whSupplierService.CreateWhSupplierAsync(parm);
        }

        /// <summary>
        /// 更新（供应商）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("update")]
        [LogDescription("供应商管理", BusinessType.UPDATE)]
        [PermissionDescription("wh:supplier:update")]
        public async Task UpdateWhSupplierAsync([FromBody] WhSupplierModifyDto parm)
        {
            await _whSupplierService.ModifyWhSupplierAsync(parm);
        }

        /// <summary>
        /// 删除（供应商）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("delete")]
        [LogDescription("供应商管理", BusinessType.DELETE)]
        [PermissionDescription("wh:supplier:delete")]
        public async Task DeleteWhSupplierAsync([FromBody] long[] ids)
        {
            await _whSupplierService.DeletesWhSupplierAsync(ids);
        }

        /// <summary>
        /// 导入供应商数据
        /// </summary>
        /// <param name="formFile"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("import")]
        [LogDescription("导入供应商数据", BusinessType.EXPORT)]
        [PermissionDescription("wh:supplier:import")]
        public async Task ImportWhSupplierAsync([FromForm(Name = "file")] IFormFile formFile)
        {
            await _whSupplierService.ImportWhSupplierAsync(formFile);
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
            await _whSupplierService.DownloadImportTemplateAsync(stream);
            return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"供应商导入模板.xlsx");
        }

        /// <summary>
        /// 导出供应商信息
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("export")]
        [LogDescription("导出供应商信息", BusinessType.EXPORT)]
        [PermissionDescription("wh:supplier:export")]
        public async Task<WhSupplierExportResultDto> ExprotWhSupplierListAsync([FromQuery] WhSupplierPagedQueryDto param)
        {
            return await _whSupplierService.ExprotWhSupplierListAsync(param);
        }

    }
}