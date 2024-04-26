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
    /// 控制器（产品检验参数组）
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ProcProductParameterGroupController : ControllerBase
    {
        /// <summary>
        /// 日志
        /// </summary>
        private readonly ILogger<ProcProductParameterGroupController> _logger;
        /// <summary>
        /// 服务接口（产品检验参数组）
        /// </summary>
        private readonly IProcProductParameterGroupService _procProductParameterGroupService;


        /// <summary>
        /// 构造函数（产品检验参数组）
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="procProductParameterGroupService"></param>
        public ProcProductParameterGroupController(ILogger<ProcProductParameterGroupController> logger, IProcProductParameterGroupService procProductParameterGroupService)
        {
            _logger = logger;
            _procProductParameterGroupService = procProductParameterGroupService;
        }

        /// <summary>
        /// 添加（产品检验参数组）
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("create")]
        [LogDescription("产品检验参数组", BusinessType.INSERT)]
        [PermissionDescription("process:procProductParameterGroup:insert")]
        public async Task<long> AddAsync([FromBody] ProcProductParameterGroupSaveDto saveDto)
        {
            return await _procProductParameterGroupService.CreateAsync(saveDto);
        }

        /// <summary>
        /// 更新（产品检验参数组）
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("update")]
        [LogDescription("产品检验参数组", BusinessType.UPDATE)]
        [PermissionDescription("process:procProductParameterGroup:update")]
        public async Task UpdateAsync([FromBody] ProcProductParameterGroupSaveDto saveDto)
        {
            await _procProductParameterGroupService.ModifyAsync(saveDto);
        }

        /// <summary>
        /// 删除（产品检验参数组）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("delete")]
        [LogDescription("产品检验参数组", BusinessType.DELETE)]
        [PermissionDescription("process:procProductParameterGroup:delete")]
        public async Task DeleteAsync([FromBody] long[] ids)
        {
            await _procProductParameterGroupService.DeletesAsync(ids);
        }

        /// <summary>
        /// 查询详情（产品检验参数组）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ProcProductParameterGroupInfoDto?> QueryByIdAsync(long id)
        {
            return await _procProductParameterGroupService.QueryByIdAsync(id);
        }

        /// <summary>
        /// 根据ID获取关联明细列表（产品检验参数表）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("details/{id}")]
        public async Task<IEnumerable<ProcProductParameterGroupDetailDto>?> QueryDetailsByMainIdAsync(long id)
        {
            return await _procProductParameterGroupService.QueryDetailsByMainIdAsync(id);
        }

        /// <summary>
        /// 分页查询列表（产品检验参数组）
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("pagelist")]
        public async Task<PagedInfo<ProcProductParameterGroupDto>> QueryPagedListAsync([FromQuery] ProcProductParameterGroupPagedQueryDto pagedQueryDto)
        {
            return await _procProductParameterGroupService.GetPagedListAsync(pagedQueryDto);
        }

        #region 状态变更
        /// <summary>
        /// 启用（产品检验参数组）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("updateStatusEnable")]
        [LogDescription("产品检验参数组", BusinessType.UPDATE)]
        [PermissionDescription("process:procProductParameterGroup:updateStatusEnable")]
        public async Task UpdateStatusEnable([FromBody] long id)
        {
            await _procProductParameterGroupService.UpdateStatusAsync(new ChangeStatusDto { Id = id, Status = SysDataStatusEnum.Enable });
        }

        /// <summary>
        /// 保留（产品检验参数组）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("updateStatusRetain")]
        [LogDescription("产品检验参数组", BusinessType.UPDATE)]
        [PermissionDescription("process:procProductParameterGroup:updateStatusRetain")]
        public async Task UpdateStatusRetain([FromBody] long id)
        {
            await _procProductParameterGroupService.UpdateStatusAsync(new ChangeStatusDto { Id = id, Status = SysDataStatusEnum.Retain });
        }

        /// <summary>
        /// 废除（产品检验参数组）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("updateStatusAbolish")]
        [LogDescription("产品检验参数组", BusinessType.UPDATE)]
        [PermissionDescription("process:procProductParameterGroup:updateStatusAbolish")]
        public async Task UpdateStatusAbolish([FromBody] long id)
        {
            await _procProductParameterGroupService.UpdateStatusAsync(new ChangeStatusDto { Id = id, Status = SysDataStatusEnum.Abolish });
        }

        #endregion

        /// <summary>
        /// 根据条码与工序查询当前版本的产品参数收集详情
        /// </summary>
        /// <param name="queryDto"></param>
        /// <returns></returns>
        [HttpGet("getBySfcsAndProcedureId")]
        public async Task<IEnumerable<ProcProductParameterGroupDetailDto>> GetBySfcsAndProcedureIdAsync([FromQuery] ProcProductParameterGroupToParameterCollectionQueryDto queryDto)
        {
            return await _procProductParameterGroupService.GetBySfcsAndProcedureIdAsync(queryDto);
        }
    }
}