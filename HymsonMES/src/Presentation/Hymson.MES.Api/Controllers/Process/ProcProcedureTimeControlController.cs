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
    /// 控制器（跨工序时间管控）
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ProcProcedureTimeControlController : ControllerBase
    {
        /// <summary>
        /// 日志
        /// </summary>
        private readonly ILogger<ProcProcedureTimeControlController> _logger;

        /// <summary>
        /// 接口（跨工序时间管控）
        /// </summary>
        private readonly IProcProcedureTimeControlService _manuProcedureTimeControlService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="manuProcedureTimeControlService"></param>
        public ProcProcedureTimeControlController(ILogger<ProcProcedureTimeControlController> logger,
            IProcProcedureTimeControlService manuProcedureTimeControlService)
        {
            _logger = logger;
            _manuProcedureTimeControlService = manuProcedureTimeControlService;
        }


        /// <summary>
        /// 添加（跨工序时间管控）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("create")]
        [LogDescription("跨工序时间管控", BusinessType.INSERT)]
        [PermissionDescription("process:procProcedureTimeControl:insert")]
        public async Task AddAsync([FromBody] ProcProcedureTimeControlCreateDto parm)
        {
            await _manuProcedureTimeControlService.CreateAsync(parm);
        }

        /// <summary>
        /// 更新（跨工序时间管控）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("update")]
        [LogDescription("跨工序时间管控", BusinessType.UPDATE)]
        [PermissionDescription("process:procProcedureTimeControl:update")]
        public async Task UpdateAsync([FromBody] ProcProcedureTimeControlModifyDto parm)
        {
            await _manuProcedureTimeControlService.ModifyAsync(parm);
        }

        /// <summary>
        /// 删除（跨工序时间管控）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("delete")]
        [LogDescription("跨工序时间管控", BusinessType.DELETE)]
        [PermissionDescription("process:procProcedureTimeControl:delete")]
        public async Task DeleteAsync([FromBody] long[] ids)
        {
            await _manuProcedureTimeControlService.DeletesAsync(ids);
        }

        /// <summary>
        /// 分页查询列表（跨工序时间管控）
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("pagelist")]
        public async Task<PagedInfo<ProcProcedureTimeControlDto>> GetPagedListAsync([FromQuery] ProcProcedureTimeControlPagedQueryDto pagedQueryDto)
        {
            return await _manuProcedureTimeControlService.GetPagedListAsync(pagedQueryDto);
        }

        /// <summary>
        /// 查询详情（跨工序时间管控）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ProcProcedureTimeControlDetailDto> QueryByIdAsync(long id)
        {
            return await _manuProcedureTimeControlService.QueryByIdAsync(id);
        }


        #region 状态变更
        /// <summary>
        /// 启用
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("enable")]
        [LogDescription("启用", BusinessType.OTHER)]
        [PermissionDescription("process:procProcedureTimeControl:enable")]
        public async Task EnableAsync([FromBody] long id)
        {
            await _manuProcedureTimeControlService.UpdateStatusAsync(new ChangeStatusDto
            {
                Id = id,
                Status = SysDataStatusEnum.Enable
            });
        }

        /// <summary>
        /// 保留
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("retain")]
        [LogDescription("保留", BusinessType.OTHER)]
        [PermissionDescription("process:procProcedureTimeControl:retain")]
        public async Task RetainAsync([FromBody] long id)
        {
            await _manuProcedureTimeControlService.UpdateStatusAsync(new ChangeStatusDto
            {
                Id = id,
                Status = SysDataStatusEnum.Retain
            });
        }

        /// <summary>
        /// 废除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("abolish")]
        [LogDescription("废除", BusinessType.OTHER)]
        [PermissionDescription("process:procProcedureTimeControl:abolish")]
        public async Task AbolishAsync([FromBody] long id)
        {
            await _manuProcedureTimeControlService.UpdateStatusAsync(new ChangeStatusDto
            {
                Id = id,
                Status = SysDataStatusEnum.Abolish
            });
        }
        #endregion

    }
}