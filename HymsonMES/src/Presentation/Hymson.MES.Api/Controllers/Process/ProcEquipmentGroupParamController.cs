using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;
using Hymson.MES.Services.Dtos.Common;
using Hymson.MES.Services.Dtos.Process;
using Hymson.MES.Services.Services.Process;
using Hymson.Web.Framework.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.Process
{
    /// <summary>
    /// 控制器（设备参数组）
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ProcEquipmentGroupParamController : ControllerBase
    {
        /// <summary>
        /// 接口（设备参数组）
        /// </summary>
        private readonly IProcEquipmentGroupParamService _procEquipmentGroupParamService;
        private readonly ILogger<ProcEquipmentGroupParamController> _logger;

        /// <summary>
        /// 构造函数（设备参数组）
        /// </summary>
        /// <param name="procEquipmentGroupParamService"></param>
        /// <param name="logger"></param>
        public ProcEquipmentGroupParamController(IProcEquipmentGroupParamService procEquipmentGroupParamService, ILogger<ProcEquipmentGroupParamController> logger)
        {
            _procEquipmentGroupParamService = procEquipmentGroupParamService;
            _logger = logger;
        }

        #region 框架生成方法

        /// <summary>
        /// 分页查询列表（设备参数组）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("pagelist")]
        public async Task<PagedInfo<ProcEquipmentGroupParamViewDto>> QueryPagedProcEquipmentGroupParamAsync([FromQuery] ProcEquipmentGroupParamPagedQueryDto parm)
        {
            return await _procEquipmentGroupParamService.GetPagedListAsync(parm);
        }

        /// <summary>
        /// 查询详情（设备参数组）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ProcEquipmentGroupParamViewDto> QueryProcEquipmentGroupParamByIdAsync(long id)
        {
            return await _procEquipmentGroupParamService.QueryProcEquipmentGroupParamByIdAsync(id);
        }

        /// <summary>
        /// 添加（设备参数组）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("create")]
        [LogDescription("设备参数组", BusinessType.INSERT)]
        [PermissionDescription("proc:equipmentGroupParam:insert")]
        public async Task<long> AddProcEquipmentGroupParamAsync([FromBody] ProcEquipmentGroupParamCreateDto parm)
        {
            return await _procEquipmentGroupParamService.CreateAsync(parm);
        }

        /// <summary>
        /// 更新（设备参数组）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("update")]
        [LogDescription("设备参数组", BusinessType.UPDATE)]
        [PermissionDescription("proc:equipmentGroupParam:update")]
        public async Task UpdateProcEquipmentGroupParamAsync([FromBody] ProcEquipmentGroupParamModifyDto parm)
        {
             await _procEquipmentGroupParamService.ModifyAsync(parm);
        }

        /// <summary>
        /// 删除（设备参数组）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("delete")]
        [LogDescription("设备参数组", BusinessType.DELETE)]
        [PermissionDescription("proc:equipmentGroupParam:delete")]
        public async Task DeleteProcEquipmentGroupParamAsync([FromBody] long[] ids)
        {
            await _procEquipmentGroupParamService.DeletesAsync(ids);
        }

        #endregion



        /// <summary>
        /// 查询详情（设备组参数详情维护）
        /// </summary>
        /// <param name="recipeId"></param>
        /// <returns></returns>
        [HttpGet("getProcEquGroupParamDetail/{recipeId}")]
        public async Task<IEnumerable<ProcEquipmentGroupParamDetailDto>> QueryProcEquipmentGroupParamDetailByRecipeIdAsync(long recipeId)
        {
            return await _procEquipmentGroupParamService.QueryProcEquipmentGroupParamDetailByRecipeIdAsync(recipeId);
        }


        /// <summary>
        /// 分页查询详情的参数
        /// </summary>
        /// <param name="queryDto"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("getDetailParamByProductIdAndProcedureIdPagelist")]
        public async Task<PagedInfo<ProcParameterDto>> GetDetailParamByProductIdAndProcedureIdPagedAsync([FromQuery] ProcEquipmentGroupParamDetailParamPagedQueryDto queryDto) 
        {
            return await _procEquipmentGroupParamService.GetDetailParamByProductIdAndProcedureIdPagedAsync(queryDto);
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
            await _procEquipmentGroupParamService.DownloadImportTemplateAsync(stream);
            return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"Recipe参数导入模板.xlsx");
        }

        /// <summary>
        /// 导入客户数据
        /// </summary>
        /// <param name="formFile"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("import")]
        public async Task ImportEquipmentGroupParamAsync([FromForm(Name = "file")] IFormFile formFile)
        {

            await _procEquipmentGroupParamService.ImportInteEquipmentGroupParamAsync(formFile);
        }

        #region 状态变更
        /// <summary>
        /// 启用（设备组参数详情维护）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("updateStatusEnable")]
        [LogDescription("设备组参数详情维护", BusinessType.UPDATE)]
        [PermissionDescription("proc:equipmentGroupParam:updateStatusEnable")]
        public async Task UpdateStatusEnable([FromBody] long id)
        {
            await _procEquipmentGroupParamService.UpdateStatusAsync(new ChangeStatusDto { Id = id, Status = SysDataStatusEnum.Enable });
        }

        /// <summary>
        /// 保留（设备组参数详情维护）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("updateStatusRetain")]
        [LogDescription("设备组参数详情维护", BusinessType.UPDATE)]
        [PermissionDescription("proc:equipmentGroupParam:updateStatusRetain")]
        public async Task UpdateStatusRetain([FromBody] long id)
        {
            await _procEquipmentGroupParamService.UpdateStatusAsync(new ChangeStatusDto { Id = id, Status = SysDataStatusEnum.Retain });
        }

        /// <summary>
        /// 废除（设备组参数详情维护）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("updateStatusAbolish")]
        [LogDescription("设备组参数详情维护", BusinessType.UPDATE)]
        [PermissionDescription("proc:equipmentGroupParam:updateStatusAbolish")]
        public async Task UpdateStatusAbolish([FromBody] long id)
        {
            await _procEquipmentGroupParamService.UpdateStatusAsync(new ChangeStatusDto { Id = id, Status = SysDataStatusEnum.Abolish });
        }

        #endregion
    }
}