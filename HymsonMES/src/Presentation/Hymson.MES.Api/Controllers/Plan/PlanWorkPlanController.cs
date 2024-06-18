using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Plan;
using Hymson.MES.Services.Services.Plan;
using Hymson.Web.Framework.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.Plan
{
    /// <summary>
    /// 控制器（生产计划）
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class PlanWorkPlanController : ControllerBase
    {
        /// <summary>
        /// 日志接口
        /// </summary>
        private readonly ILogger<PlanWorkPlanController> _logger;

        /// <summary>
        /// 接口（生产计划）
        /// </summary>
        private readonly IPlanWorkPlanService _planWorkPlanService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="planWorkPlanService"></param>
        public PlanWorkPlanController(ILogger<PlanWorkPlanController> logger, IPlanWorkPlanService planWorkPlanService)
        {
            _logger = logger;
            _planWorkPlanService = planWorkPlanService;
        }


        /// <summary>
        /// 分页查询列表（生产计划）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpGet("pagelist")]
        public async Task<PagedInfo<PlanWorkPlanDto>> GetPageListAsync([FromQuery] PlanWorkPlanPagedQueryDto parm)
        {
            return await _planWorkPlanService.GetPageListAsync(parm);
        }

        /// <summary>
        /// 查询详情（生产计划）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<PlanWorkPlanDto?> QueryByIdAsync(long id)
        {
            return await _planWorkPlanService.QueryByIdAsync(id);
        }

        /// <summary>
        /// 添加（生产计划）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPost("save")]
        [LogDescription("生产计划", BusinessType.INSERT)]
        [PermissionDescription("plan:workPlan:insert")]
        public async Task<long> SaveAsync([FromBody] PlanWorkPlanSaveDto parm)
        {
            return await _planWorkPlanService.SaveAsync(parm);
        }

        /// <summary>
        /// 删除（生产计划）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpDelete("delete")]
        [LogDescription("生产计划", BusinessType.DELETE)]
        [PermissionDescription("plan:workPlan:delete")]
        public async Task DeletesAsync([FromBody] long[] ids)
        {
            await _planWorkPlanService.DeletesAsync(ids);
        }

    }
}