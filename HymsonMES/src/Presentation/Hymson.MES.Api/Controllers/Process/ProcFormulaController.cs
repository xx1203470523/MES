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
    /// 控制器（配方维护）
    /// @author Karl
    /// @date 2023-12-20 08:54:36
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ProcFormulaController : ControllerBase
    {
        /// <summary>
        /// 日志
        /// </summary>
        private readonly ILogger<ProcFormulaController> _logger;
        /// <summary>
        /// 服务接口（配方维护）
        /// </summary>
        private readonly IProcFormulaService _procFormulaService;


        /// <summary>
        /// 构造函数（配方维护）
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="procFormulaService"></param>
        public ProcFormulaController(ILogger<ProcFormulaController> logger, IProcFormulaService procFormulaService)
        {
            _logger = logger;
            _procFormulaService = procFormulaService;
        }

        /// <summary>
        /// 添加（配方维护）
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("create")]
        [LogDescription("配方维护", BusinessType.INSERT)]
        public async Task AddAsync([FromBody] ProcFormulaSaveDto saveDto)
        {
             await _procFormulaService.CreateAsync(saveDto);
        }

        /// <summary>
        /// 更新（配方维护）
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("update")]
        [LogDescription("配方维护", BusinessType.UPDATE)]
        public async Task UpdateAsync([FromBody] ProcFormulaSaveDto saveDto)
        {
             await _procFormulaService.ModifyAsync(saveDto);
        }

        /// <summary>
        /// 删除（配方维护）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("delete")]
        [LogDescription("配方维护", BusinessType.DELETE)]
        public async Task DeleteAsync([FromBody] long[] ids)
        {
            await _procFormulaService.DeletesAsync(ids);
        }

        /// <summary>
        /// 查询详情（配方维护）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ProcFormulaViewDto?> QueryByIdAsync(long id)
        {
            return await _procFormulaService.QueryByIdAsync(id);
        }

        /// <summary>
        /// 分页查询列表（配方维护）
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("pagelist")]
        public async Task<PagedInfo<ProcFormulaViewDto>> QueryPagedListAsync([FromQuery] ProcFormulaPagedQueryDto pagedQueryDto)
        {
            return await _procFormulaService.GetPagedListAsync(pagedQueryDto);
        }

        /// <summary>
        /// 获取详情
        /// </summary>
        /// <param name="formulaId"></param>
        /// <returns></returns>
        [HttpGet("details/{formulaId}")]
        public async Task<IEnumerable<ProcFormulaDetailsViewDto>> GetFormulaDetailsByFormulaIdAsync(long formulaId) 
        {
            return await _procFormulaService.GetFormulaDetailsByFormulaIdAsync(formulaId);
        }

        #region 状态变更
        /// <summary>
        /// 启用
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("updateStatusEnable")]
        [LogDescription("配方操作", BusinessType.UPDATE)]
        [PermissionDescription("proc:procFormula:updateStatusEnable")]
        public async Task UpdateStatusEnable([FromBody] long id)
        {
            await _procFormulaService.UpdateStatusAsync(new ChangeStatusDto { Id = id, Status = SysDataStatusEnum.Enable });
        }

        /// <summary>
        /// 保留（物料维护）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("updateStatusRetain")]
        [LogDescription("配方操作", BusinessType.UPDATE)]
        [PermissionDescription("proc:procFormula:updateStatusRetain")]
        public async Task UpdateStatusRetain([FromBody] long id)
        {
            await _procFormulaService.UpdateStatusAsync(new ChangeStatusDto { Id = id, Status = SysDataStatusEnum.Retain });
        }

        /// <summary>
        /// 废除（物料维护）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("updateStatusAbolish")]
        [LogDescription("配方操作", BusinessType.UPDATE)]
        [PermissionDescription("proc:procFormula:updateStatusAbolish")]
        public async Task UpdateStatusAbolish([FromBody] long id)
        {
            await _procFormulaService.UpdateStatusAsync(new ChangeStatusDto { Id = id, Status = SysDataStatusEnum.Abolish });
        }

        #endregion

    }
}