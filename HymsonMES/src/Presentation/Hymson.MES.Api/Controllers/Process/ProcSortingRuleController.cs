/*
 *creator: Karl
 *
 *describe: 分选规则    控制器 | 代码由框架生成  
 *builder:  zhaoqing
 *build datetime: 2023-07-25 03:24:54
 */
using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Process;
using Hymson.MES.Services.Services.Process;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.Process
{
    /// <summary>
    /// 控制器（分选规则）
    /// @author zhaoqing
    /// @date 2023-07-25 03:24:54
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
        public async Task AddProcSortingRuleAsync([FromBody] ProcSortingRuleCreateDto parm)
        {
             await _procSortingRuleService.CreateProcSortingRuleAsync(parm);
        }

        /// <summary>
        /// 更新（分选规则）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("update")]
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
        [Route("getDetails")]
        public async Task<IEnumerable<ProcSortingRuleDetailViewDto>> GetSortingRuleDetailListAsync([FromQuery] ProcSortingRuleDetailQueryDto parm)
        {
            return await _procSortingRuleService.GetSortingRuleDetailListAsync(parm);
        }

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
    }
}