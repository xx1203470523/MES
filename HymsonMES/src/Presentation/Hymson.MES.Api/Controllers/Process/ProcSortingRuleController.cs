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
    /// 控制器（分选规则）
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ProcSortingRuleController : ControllerBase
    {
        /// <summary>
        /// 接口（分选规则）
        /// </summary>
        private readonly IProcSortingRuleService _procSortingRuleService;
        private readonly ILogger<ProcSortingRuleController> _logger;

        /// <summary>
        /// 构造函数（分选规则）
        /// </summary>
        /// <param name="procSortingRuleService"></param>
        /// <param name="logger"></param>
        public ProcSortingRuleController(IProcSortingRuleService procSortingRuleService, ILogger<ProcSortingRuleController> logger)
        {
            _procSortingRuleService = procSortingRuleService;
            _logger = logger;
        }

        #region 框架生成方法

        /// <summary>
        /// 分页查询列表（分选规则）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("pagelist")]
        public async Task<PagedInfo<ProcSortingRuleDto>> QueryPagedProcSortingRuleAsync([FromQuery] ProcSortingRulePagedQueryDto parm)
        {
            return await _procSortingRuleService.GetPagedListAsync(parm);
        }

        /// <summary>
        /// 查询详情（分选规则）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ProcSortingRuleDto> QueryProcSortingRuleByIdAsync(long id)
        {
            return await _procSortingRuleService.QueryProcSortingRuleByIdAsync(id);
        }

        /// <summary>
        /// 获取分选规则参数信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}/parameterDetai")]
        public async Task<IEnumerable<ProcSortingRuleDetailViewDto>> GetSortingRuleDetailList(long id)
        {
            return await _procSortingRuleService.GetProcSortingRuleGradeRuleDetailsAsync(id);
        }

        /// <summary>
        /// 获取档位信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}/grade")]
        public async Task<IEnumerable<SortingRuleGradeDto>> GetProcSortingRuleGrades(long id)
        {
            return await _procSortingRuleService.GetProcSortingRuleGradesAsync(id);
        }

        /// <summary>
        /// 添加（分选规则）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("create")]
        [LogDescription("分选规则", BusinessType.INSERT)]
        [PermissionDescription("proc:procSortingRule:insert")]
        public async Task<long> AddProcSortingRuleAsync([FromBody] ProcSortingRuleCreateDto parm)
        {
           return  await _procSortingRuleService.CreateProcSortingRuleAsync(parm);
        }

        /// <summary>
        /// 更新（分选规则）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("update")]
        [LogDescription("分选规则", BusinessType.UPDATE)]
        [PermissionDescription("proc:procSortingRule:update")]
        public async Task UpdateProcSortingRuleAsync([FromBody] ProcSortingRuleModifyDto parm)
        {
             await _procSortingRuleService.ModifyProcSortingRuleAsync(parm);
        }

        /// <summary>
        /// 删除（分选规则）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("delete")]
        [LogDescription("分选规则", BusinessType.DELETE)]
        [PermissionDescription("proc:procSortingRule:delete")]
        public async Task DeleteProcSortingRuleAsync([FromBody] long[] ids)
        {
            await _procSortingRuleService.DeletesProcSortingRuleAsync(ids);
        }

        #endregion

        /// <summary>
        /// 分页查询列表（分选规则）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("getDetailsByMaterialId")]
        public async Task<IEnumerable<ProcSortingRuleDetailViewDto>> GetSortingRuleDetailListByMaterialIdAsync([FromQuery] ProcSortingRuleDetailQueryDto parm)
        {
            return await _procSortingRuleService.GetSortingRuleDetailListByMaterialIdAsync(parm);
        }

        #region 状态变更
        /// <summary>
        /// 启用（分选规则）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("updateStatusEnable")]
        [LogDescription("分选规则", BusinessType.UPDATE)]
        [PermissionDescription("proc:procSortingRule:updateStatusEnable")]
        public async Task UpdateStatusEnable([FromBody] long id)
        {
            await _procSortingRuleService.UpdateStatusAsync(new ChangeStatusDto { Id = id, Status = SysDataStatusEnum.Enable });
        }

        /// <summary>
        /// 保留（分选规则）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("updateStatusRetain")]
        [LogDescription("分选规则", BusinessType.UPDATE)]
        [PermissionDescription("proc:procSortingRule:updateStatusRetain")]
        public async Task UpdateStatusRetain([FromBody] long id)
        {
            await _procSortingRuleService.UpdateStatusAsync(new ChangeStatusDto { Id = id, Status = SysDataStatusEnum.Retain });
        }

        /// <summary>
        /// 废除（分选规则）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("updateStatusAbolish")]
        [LogDescription("分选规则", BusinessType.UPDATE)]
        [PermissionDescription("proc:procSortingRule:updateStatusAbolish")]
        public async Task UpdateStatusAbolish([FromBody] long id)
        {
            await _procSortingRuleService.UpdateStatusAsync(new ChangeStatusDto { Id = id, Status = SysDataStatusEnum.Abolish });
        }

        #endregion
    }
}