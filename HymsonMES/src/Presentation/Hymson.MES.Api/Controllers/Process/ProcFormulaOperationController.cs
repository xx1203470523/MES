using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;
using Hymson.MES.Services.Dtos.Common;
using Hymson.MES.Services.Dtos.Integrated;
using Hymson.MES.Services.Dtos.Process;
using Hymson.MES.Services.Services.Process;
using Hymson.Web.Framework.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.Process
{
    /// <summary>
    /// 控制器（配方操作）
    /// @author hjy
    /// @date 2023-12-15 02:08:48
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ProcFormulaOperationController : ControllerBase
    {
        /// <summary>
        /// 日志
        /// </summary>
        private readonly ILogger<ProcFormulaOperationController> _logger;
        /// <summary>
        /// 服务接口（配方操作）
        /// </summary>
        private readonly IProcFormulaOperationService _procFormulaOperationService;


        /// <summary>
        /// 构造函数（配方操作）
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="procFormulaOperationService"></param>
        public ProcFormulaOperationController(ILogger<ProcFormulaOperationController> logger, IProcFormulaOperationService procFormulaOperationService)
        {
            _logger = logger;
            _procFormulaOperationService = procFormulaOperationService;
        }

        /// <summary>
        /// 添加（配方操作）
        /// </summary>
        /// <param name="addDto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("create")]
        [LogDescription("配方操作", BusinessType.INSERT)]
        public async Task AddProcFormulaOperationAsync([FromBody] AddFormulaOperationDto addDto)
        {
             await _procFormulaOperationService.CreateProcFormulaOperationAsync(addDto);
        }

        /// <summary>
        /// 更新（配方操作）
        /// </summary>
        /// <param name="addDto"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("update")]
        [LogDescription("配方操作", BusinessType.UPDATE)]
        public async Task UpdateProcFormulaOperationAsync([FromBody] AddFormulaOperationDto addDto)
        {
             await _procFormulaOperationService.ModifyProcFormulaOperationAsync(addDto);
        }

        /// <summary>
        /// 删除（配方操作）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("delete")]
        [LogDescription("配方操作", BusinessType.DELETE)]
        public async Task DeletesProcFormulaOperationAsync([FromBody] long[] ids)
        {
            await _procFormulaOperationService.DeletesProcFormulaOperationAsync(ids);
        }

        /// <summary>
        /// 查询详情（配方操作）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ProcFormulaOperationDto?> QueryProcFormulaOperationByIdAsync(long id)
        {
            return await _procFormulaOperationService.QueryProcFormulaOperationByIdAsync(id);
        }

        /// <summary>
        /// 分页查询列表（配方操作）
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("pagelist")]
        public async Task<PagedInfo<ProcFormulaOperationDto>> QueryPagedProcFormulaOperationAsync([FromQuery] ProcFormulaOperationPagedQueryDto pagedQueryDto)
        {
            return await _procFormulaOperationService.GetPagedListAsync(pagedQueryDto);
        }

        /// <summary>
        /// 获取配方操作设置值信息
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("setValue/list")]
        public async Task<PagedInfo<ProcFormulaOperationSetDto>> GetFormulaOperationConfigSetListAsync([FromQuery] ProcFormulaOperationSetPagedQueryDto pagedQueryDto)
        {
            return await _procFormulaOperationService.GetFormulaOperationConfigSetListAsync(pagedQueryDto);
        }

        #region 状态变更
        /// <summary>
        /// 启用（配方操作维护）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("updateStatusEnable")]
        [LogDescription("配方操作维护-启用", BusinessType.UPDATE)]
        public async Task UpdateStatusEnable([FromBody] long id)
        {
            await _procFormulaOperationService.UpdateStatusAsync(new ChangeStatusDto { Id = id, Status = SysDataStatusEnum.Enable });
        }

        /// <summary>
        /// 保留（配方操作维护）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("updateStatusRetain")]
        [LogDescription("配方操作维护", BusinessType.UPDATE)]
        public async Task UpdateStatusRetain([FromBody] long id)
        {
            await _procFormulaOperationService.UpdateStatusAsync(new ChangeStatusDto { Id = id, Status = SysDataStatusEnum.Retain });
        }

        /// <summary>
        /// 废除（配方操作维护）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("updateStatusAbolish")]
        [LogDescription("配方操作维护", BusinessType.UPDATE)]
        public async Task UpdateStatusAbolish([FromBody] long id)
        {
            await _procFormulaOperationService.UpdateStatusAsync(new ChangeStatusDto { Id = id, Status = SysDataStatusEnum.Abolish });
        }

        #endregion
    }
}