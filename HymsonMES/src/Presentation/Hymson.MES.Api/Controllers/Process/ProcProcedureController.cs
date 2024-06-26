using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;
using Hymson.MES.Services.Dtos.Common;
using Hymson.MES.Services.Dtos.Integrated;
using Hymson.MES.Services.Dtos.Process;
using Hymson.MES.Services.Services.Process.Procedure;
using Hymson.Web.Framework.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers
{
    /// <summary>
    /// 控制器（工序表）
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ProcProcedureController : ControllerBase
    {
        /// <summary>
        /// 接口（工序表）
        /// </summary>
        private readonly IProcProcedureService _procProcedureService;
        private readonly ILogger<ProcProcedureController> _logger;

        /// <summary>
        /// 构造函数（工序表）
        /// </summary>
        /// <param name="procProcedureService"></param>
        /// <param name="logger"></param>
        public ProcProcedureController(IProcProcedureService procProcedureService, ILogger<ProcProcedureController> logger)
        {
            _procProcedureService = procProcedureService;
            _logger = logger;
        }

        /// <summary>
        /// 分页查询列表（工序表）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("list")]
        public async Task<PagedInfo<ProcProcedureViewDto>> QueryPagedProcProcedure([FromQuery] ProcProcedurePagedQueryDto parm)
        {
            return await _procProcedureService.GetPageListAsync(parm);
        }

        /// <summary>
        /// 分页查询列表（工序表）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("getByProcessRouteId")]
        public async Task<PagedInfo<ProcProcedureDto>> GetPagedInfoByProcessRouteIdAsync([FromQuery] ProcProcedurePagedQueryDto parm)
        {
            return await _procProcedureService.GetPagedInfoByProcessRouteIdAsync(parm);
        }

        /// <summary>
        /// 查询详情（工序表）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<QueryProcProcedureDto> GetProcProcedureById(long id)
        {
            return await _procProcedureService.GetProcProcedureByIdAsync(id);
        }

        /// <summary>
        /// 获取工序配置打印信息
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("print/list")]
        public async Task<PagedInfo<ProcProcedurePrintReleationDto>> GetProcedureBomConfigPrintListAsync([FromQuery] ProcProcedurePrintReleationPagedQueryDto parm)
        {
            return await _procProcedureService.GetProcedureConfigPrintListAsync(parm);
        }

        /// <summary>
        /// 获取工序配置Job信息
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("job/list")]
        public async Task<PagedInfo<ProcedureJobReleationDto>> GetProcedureBomConfigJobList([FromQuery] InteJobBusinessRelationPagedQueryDto parm)
        {
            return await _procProcedureService.GetProcedureConfigJobListAsync(parm);
        }

        /// <summary>
        /// 获取工序产出设置
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("product/list")]
        public async Task<IEnumerable<ProcProductSetDto>> GetProcedureProductSetListAsync([FromQuery] ProcProductSetQueryDto parm)
        {
            return await _procProcedureService.GetProcedureProductSetListAsync(parm);
        }

        /// <summary>
        /// 获取资质认证设置
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("auth/{id}")]
        [HttpGet]
        public async Task<IEnumerable<ProcQualificationAuthenticationDto>> GetProcedureAuthSetListAsync(long id)
        {
            return await _procProcedureService.GetProcedureAuthSetListAsync(id);
        }

        /// <summary>
        /// 添加（工序表）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPost]
        [LogDescription("工序维护", BusinessType.INSERT)]
        [PermissionDescription("proc:procedure:insert")]
        public async Task AddProcProcedureAsync([FromBody] AddProcProcedureDto parm)
        {
            await _procProcedureService.AddProcProcedureAsync(parm);
        }

        /// <summary>
        /// 更新（工序表）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPut]
        [LogDescription("工序维护", BusinessType.UPDATE)]
        [PermissionDescription("proc:procedure:update")]
        public async Task UpdateProcProcedureAsync([FromBody] UpdateProcProcedureDto parm)
        {
            await _procProcedureService.UpdateProcProcedureAsync(parm);
        }

        /// <summary>
        /// 删除（工序表）
        /// </summary>
        /// <param name="deleteDto"></param>
        /// <returns></returns>
        [HttpDelete]
        [LogDescription("工序维护", BusinessType.DELETE)]
        [PermissionDescription("proc:procedure:delete")]
        public async Task DeleteProcProcedureAsync(DeleteDto deleteDto)
        {
            await _procProcedureService.DeleteProcProcedureAsync(deleteDto.Ids);
        }

        /// <summary>
        /// 根据工序读取工序详细信息和资源信息
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        [HttpGet("getByCode/{code}")]
        public async Task<ProcProcedureCodeDto> GetByCodeAsync(string code)
        {
            return await _procProcedureService.GetByCodeAsync(code);
        }

        #region 状态变更
        /// <summary>
        /// 启用（工序维护）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("updateStatusEnable")]
        [LogDescription("工序维护", BusinessType.UPDATE)]
        [PermissionDescription("proc:procedure:updateStatusEnable")]
        public async Task UpdateStatusEnable([FromBody] long id)
        {
            await _procProcedureService.UpdateStatusAsync(new ChangeStatusDto { Id = id, Status = SysDataStatusEnum.Enable });
        }

        /// <summary>
        /// 保留（工序维护）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("updateStatusRetain")]
        [LogDescription("工序维护", BusinessType.UPDATE)]
        [PermissionDescription("proc:procedure:updateStatusRetain")]
        public async Task UpdateStatusRetain([FromBody] long id)
        {
            await _procProcedureService.UpdateStatusAsync(new ChangeStatusDto { Id = id, Status = SysDataStatusEnum.Retain });
        }

        /// <summary>
        /// 废除（工序维护）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("updateStatusAbolish")]
        [LogDescription("工序维护", BusinessType.UPDATE)]
        [PermissionDescription("proc:procedure:updateStatusAbolish")]
        public async Task UpdateStatusAbolish([FromBody] long id)
        {
            await _procProcedureService.UpdateStatusAsync(new ChangeStatusDto { Id = id, Status = SysDataStatusEnum.Abolish });
        }

        [HttpPost]
        [Route("create")]
        public async Task CreateProductParameterAsync()
        {
            await _procProcedureService.CreateProductParameterAsync();
        }

        #endregion
    }
}