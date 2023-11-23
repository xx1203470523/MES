/*
 *creator: Karl
 *
 *describe: BOM表    控制器 | 代码由框架生成  
 *builder:  Karl
 *build datetime: 2023-02-14 10:04:25
 */
using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Services.Dtos.Common;
using Hymson.MES.Services.Dtos.Integrated;
using Hymson.MES.Services.Dtos.Process;
using Hymson.MES.Services.Services.Process;
using Hymson.Utils;
using Hymson.Web.Framework.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Extensions;

namespace Hymson.MES.Api.Controllers.Process
{
    /// <summary>
    /// 控制器（BOM表）
    /// @author Karl
    /// @date 2023-02-14 10:04:25
    /// </summary>
    
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ProcBomController : ControllerBase
    {
        /// <summary>
        /// 接口（BOM表）
        /// </summary>
        private readonly IProcBomService _procBomService;
        private readonly ILogger<ProcBomController> _logger;

        /// <summary>
        /// 构造函数（BOM表）
        /// </summary>
        /// <param name="procBomService"></param>
        /// <param name="logger"></param>
        public ProcBomController(IProcBomService procBomService, ILogger<ProcBomController> logger)
        {
            _procBomService = procBomService;
            _logger = logger;
        }

        /// <summary>
        /// 分页查询列表（BOM表）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("pagelist")]
        public async Task<PagedInfo<ProcBomDto>> QueryPagedProcBomAsync([FromQuery] ProcBomPagedQueryDto parm)
        {
            return await _procBomService.GetPageListAsync(parm);
        }

        /// <summary>
        /// 查询详情（BOM表）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ProcBomDto> QueryProcBomByIdAsync(long id)
        {
            return await _procBomService.QueryProcBomByIdAsync(id);
        }

        /// <summary>
        /// 查询Bom维护表详情
        /// </summary>
        /// <param name="bomId"></param>
        /// <returns></returns>
        [HttpGet("material/list")]
        public async Task<List<ProcBomDetailView>> GetProcBomMaterialAsync(long bomId)
        {
            return await _procBomService.GetProcBomMaterialAsync(bomId);
        }

        /// <summary>
        /// 添加（BOM表）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("create")]
        [LogDescription("BOM维护", BusinessType.INSERT)]
        [PermissionDescription("proc:bom:insert")]
        public async Task AddProcBomAsync([FromBody] ProcBomCreateDto parm)
        {
             await _procBomService.CreateProcBomAsync(parm);
        }

        /// <summary>
        /// 更新（BOM表）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("update")]
        [LogDescription("BOM维护", BusinessType.UPDATE)]
        [PermissionDescription("proc:bom:update")]
        public async Task UpdateProcBomAsync([FromBody] ProcBomModifyDto parm)
        {
             await _procBomService.ModifyProcBomAsync(parm);
        }

        /// <summary>
        /// 删除（BOM表）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("delete")]
        [LogDescription("BOM维护", BusinessType.DELETE)]
        [PermissionDescription("proc:bom:delete")]
        public async Task DeleteProcBomAsync(long[] ids)
        {
            await _procBomService.DeletesProcBomAsync(ids);
        }


        #region 状态变更
        /// <summary>
        /// 启用（BOM维护）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("updateStatusEnable")]
        [LogDescription("BOM维护", BusinessType.UPDATE)]
        [PermissionDescription("proc:bom:updateStatusEnable")]
        public async Task UpdateStatusEnable([FromBody] long id)
        {
            await _procBomService.UpdateStatusAsync(new ChangeStatusDto { Id = id, Status = SysDataStatusEnum.Enable });
        }

        /// <summary>
        /// 保留（BOM维护）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("updateStatusRetain")]
        [LogDescription("BOM维护", BusinessType.UPDATE)]
        [PermissionDescription("proc:bom:updateStatusRetain")]
        public async Task UpdateStatusRetain([FromBody] long id)
        {
            await _procBomService.UpdateStatusAsync(new ChangeStatusDto { Id = id, Status = SysDataStatusEnum.Retain });
        }

        /// <summary>
        /// 废除（BOM维护）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("updateStatusAbolish")]
        [LogDescription("BOM维护", BusinessType.UPDATE)]
        [PermissionDescription("proc:bom:updateStatusAbolish")]
        public async Task UpdateStatusAbolish([FromBody] long id)
        {
            await _procBomService.UpdateStatusAsync(new ChangeStatusDto { Id = id, Status = SysDataStatusEnum.Abolish });
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
            await _procBomService.DownloadImportTemplateAsync(stream);
            return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"Bom导入模板.xlsx");
        }

        /// <summary>
        /// 导入Bom数据
        /// </summary>
        /// <param name="formFile"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("importBom")]
        public async Task ImportCustomAsync([FromForm(Name = "file")] IFormFile formFile)
        {

            await _procBomService.ImportBomAsync(formFile);
        }

        /// <summary>
        /// 导出客户维护信息
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("export")]
        [PermissionDescription("proc:parameter:export")]
        public async Task<BomExportResultDto> ExprotBomPageListAsync([FromQuery] ProcBomPagedQuery param)
        {
            return await _procBomService.ExprotBomPageListAsync(param);
        }

        /// <summary>
        /// 判断bom是否被激活工单引用
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("judgeBomIdIsUsedByActivatedOrder/{id}")]
        public async Task<bool> JudgeBomIsReferencedByActivatedWorkOrder(long id) 
        {
            return await _procBomService.JudgeBomIsReferencedByActivatedWorkOrder(id);
        }
    }
}