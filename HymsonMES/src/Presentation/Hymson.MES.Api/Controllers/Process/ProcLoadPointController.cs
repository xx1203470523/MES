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
    /// 控制器（上料点表）
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ProcLoadPointController : ControllerBase
    {
        /// <summary>
        /// 接口（上料点表）
        /// </summary>
        private readonly IProcLoadPointService _procLoadPointService;
        private readonly ILogger<ProcLoadPointController> _logger;

        /// <summary>
        /// 构造函数（上料点表）
        /// </summary>
        /// <param name="procLoadPointService"></param>
        /// <param name="logger"></param>
        public ProcLoadPointController(IProcLoadPointService procLoadPointService, ILogger<ProcLoadPointController> logger)
        {
            _procLoadPointService = procLoadPointService;
            _logger = logger;
        }


        /// <summary>
        /// 分页查询列表（上料点表）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("pagelist")]
        public async Task<PagedInfo<ProcLoadPointDto>> QueryPagedProcLoadPointAsync([FromQuery] ProcLoadPointPagedQueryDto parm)
        {
            return await _procLoadPointService.GetPageListAsync(parm);
        }

        /// <summary>
        /// 查询详情（上料点表）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ProcLoadPointDetailDto> QueryProcLoadPointByIdAsync(long id)
        {
            return await _procLoadPointService.QueryProcLoadPointByIdAsync(id);
        }

        /// <summary>
        /// 添加（上料点表）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("create")]
        [LogDescription("上料点维护", BusinessType.INSERT)]
        [PermissionDescription("proc:loadPoint:insert")]
        public async Task<long> AddProcLoadPointAsync([FromBody] ProcLoadPointCreateDto parm)
        {
           return  await _procLoadPointService.CreateProcLoadPointAsync(parm);
        }

        /// <summary>
        /// 更新（上料点表）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("update")]
        [LogDescription("上料点维护", BusinessType.UPDATE)]
        [PermissionDescription("proc:loadPoint:update")]
        public async Task UpdateProcLoadPointAsync([FromBody] ProcLoadPointModifyDto parm)
        {
             await _procLoadPointService.ModifyProcLoadPointAsync(parm);
        }

        /// <summary>
        /// 删除（上料点表）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("delete")]
        [LogDescription("上料点维护", BusinessType.DELETE)]
        [PermissionDescription("proc:loadPoint:delete")]
        public async Task DeleteProcLoadPointAsync([FromBody] long[] ids)
        {
            await _procLoadPointService.DeletesProcLoadPointAsync(ids);
        }

        #region 状态变更
        /// <summary>
        /// 启用（上料点维护）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("updateStatusEnable")]
        [LogDescription("上料点维护", BusinessType.UPDATE)]
        [PermissionDescription("proc:loadPoint:updateStatusEnable")]
        public async Task UpdateStatusEnable([FromBody] long id)
        {
            await _procLoadPointService.UpdateStatusAsync(new ChangeStatusDto { Id = id, Status = SysDataStatusEnum.Enable });
        }

        /// <summary>
        /// 保留（上料点维护）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("updateStatusRetain")]
        [LogDescription("上料点维护", BusinessType.UPDATE)]
        [PermissionDescription("proc:loadPoint:updateStatusRetain")]
        public async Task UpdateStatusRetain([FromBody] long id)
        {
            await _procLoadPointService.UpdateStatusAsync(new ChangeStatusDto { Id = id, Status = SysDataStatusEnum.Retain });
        }

        /// <summary>
        /// 废除（上料点维护）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("updateStatusAbolish")]
        [LogDescription("上料点维护", BusinessType.UPDATE)]
        [PermissionDescription("proc:loadPoint:updateStatusAbolish")]
        public async Task UpdateStatusAbolish([FromBody] long id)
        {
            await _procLoadPointService.UpdateStatusAsync(new ChangeStatusDto { Id = id, Status = SysDataStatusEnum.Abolish });
        }

        #endregion

        /// <summary>
        /// 导入模板下载
        /// </summary>
        /// <returns></returns>
        [HttpGet("downloadImportTemplate")]
        [LogDescription("导入模板下载", BusinessType.EXPORT, IsSaveRequestData = false, IsSaveResponseData = false)]
        public async Task<IActionResult> DownloadTemplateExcel()
        {
            using MemoryStream stream = new MemoryStream();
            await _procLoadPointService.DownloadImportTemplateAsync(stream);
            return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"上料点导入模板.xlsx");
        }

        /// <summary>
        /// 导入上料点数据
        /// </summary>
        /// <param name="formFile"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("importLoadPoint")]
        [LogDescription("导入上料点数据", BusinessType.EXPORT)]
        public async Task ImportCustomAsync([FromForm(Name = "file")] IFormFile formFile)
        {
            await _procLoadPointService.ImportLoadPointAsync(formFile);
        }

        /// <summary>
        /// 导出客户维护信息
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("export")]
        [PermissionDescription("proc:parameter:export")]
        public async Task<LoadPointExportResultDto> ExprotBomPageListAsync([FromQuery] ProcLoadPointPagedQueryDto param)
        {
            return await _procLoadPointService.ExprotLoadPointPageListAsync(param);
        }
    }
}