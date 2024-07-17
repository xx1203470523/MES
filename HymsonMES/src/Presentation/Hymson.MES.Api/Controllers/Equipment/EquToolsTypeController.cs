using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Equipment;
using Hymson.MES.Services.Dtos.Process;
using Hymson.MES.Services.Services.Equipment;
using Hymson.Web.Framework.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.Equipment
{
    /// <summary>
    /// 控制器（工具类型管理）
    /// @author zhaoqing
    /// @date 2024-07-09 11:38:03
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class EquToolsTypeController : ControllerBase
    {
        /// <summary>
        /// 日志
        /// </summary>
        private readonly ILogger<EquToolsTypeController> _logger;
        /// <summary>
        /// 服务接口（工具类型管理）
        /// </summary>
        private readonly IEquToolsTypeService _equToolsTypeService;


        /// <summary>
        /// 构造函数（工具类型管理）
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="equToolsTypeService"></param>
        public EquToolsTypeController(ILogger<EquToolsTypeController> logger, IEquToolsTypeService equToolsTypeService)
        {
            _logger = logger;
            _equToolsTypeService = equToolsTypeService;
        }

        /// <summary>
        /// 添加（工具类型管理）
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("create")]
        [LogDescription("工具类型", BusinessType.INSERT)]
        public async Task AddAsync([FromBody] EquToolsTypeSaveDto saveDto)
        {
            await _equToolsTypeService.CreateAsync(saveDto);
        }

        /// <summary>
        /// 更新（工具类型管理）
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("update")]
        [LogDescription("工具类型", BusinessType.UPDATE)]
        public async Task UpdateAsync([FromBody] EquToolsTypeSaveDto saveDto)
        {
            await _equToolsTypeService.ModifyAsync(saveDto);
        }

        /// <summary>
        /// 删除（工具类型管理）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("delete")]
        [LogDescription("工具类型", BusinessType.DELETE)]
        public async Task DeleteAsync([FromBody] long[] ids)
        {
            await _equToolsTypeService.DeletesAsync(ids);
        }

        /// <summary>
        /// 查询详情（工具类型管理）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<EquToolsTypeDto?> QueryByIdAsync(long id)
        {
            return await _equToolsTypeService.QueryByIdAsync(id);
        }

        /// <summary>
        /// 获取设备组
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("equipment/{id}")]
        public async Task<EquToolsTypeCofigEquipmentGroupDto> GetEquipmentRelationAsync(long id)
        {
            return await _equToolsTypeService.GetEquipmentRelationAsync(id);
        }

        /// <summary>
        /// 获取物料
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("material/{id}")]
        public async Task<EquToolsTypeCofigMaterialDto> GetMaterialRelationAsync(long id)
        {
            return await _equToolsTypeService.GetMaterialRelationAsync(id);
        }

        /// <summary>
        /// 分页查询列表（工具类型管理）
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("pagelist")]
        public async Task<PagedInfo<EquToolsTypeDto>> QueryPagedListAsync([FromQuery] EquToolsTypePagedQueryDto pagedQueryDto)
        {
            return await _equToolsTypeService.GetPagedListAsync(pagedQueryDto);
        }

        /// <summary>
        /// 获取设备组
        /// </summary>
        /// <param name="queryDto"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("equGrouplist")]
        public async Task<IEnumerable<EquEquipmentGroupListDto>> GetEquipmentsAsync([FromQuery] EquToolsTypeQueryDto queryDto)
        {
            return await _equToolsTypeService.GetEquipmentsAsync(queryDto.Id);
        }

        /// <summary>
        /// 获取物料列表
        /// </summary>
        /// <param name="queryDto"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("materialslist")]
        public async Task<IEnumerable<ProcMaterialDto>> GetMaterialsAsync([FromQuery] EquToolsTypeQueryDto queryDto)
        {
            return await _equToolsTypeService.GetMaterialsAsync(queryDto.Id);
        }

        /// <summary>
        /// 下载导入模板
        /// </summary>
        /// <returns></returns>
        [HttpGet("downloadImportTemplate")]
        //[PermissionDescription("equ:toolsType:download")]
        [LogDescription("导入模板下载", BusinessType.EXPORT, IsSaveRequestData = false, IsSaveResponseData = false)]
        public async Task<IActionResult> DownloadTemplateExcel()
        {
            using MemoryStream stream = new();
            var worksheetName = await _equToolsTypeService.DownloadImportTemplateAsync(stream);
            return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"{worksheetName}导入模板.xlsx");
        }

        /// <summary>
        /// 导入
        /// </summary>
        /// <param name="formFile"></param>
        /// <returns></returns>
        [HttpPost("import")]
        //[PermissionDescription("equ:toolingManage:import")]
        [LogDescription("导入模板下载", BusinessType.IMPORT, IsSaveRequestData = false, IsSaveResponseData = false)]
        public async Task ImportAsync([FromForm(Name = "file")] IFormFile formFile)
        {
            await _equToolsTypeService.ImportAsync(formFile);
        }
    }
}