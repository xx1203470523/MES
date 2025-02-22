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
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        [HttpGet("pagelist")]
        public async Task<PagedInfo<PlanWorkPlanProductDto>> GetPageListAsync([FromQuery] PlanWorkPlanProductPagedQueryDto pagedQueryDto)
        {
            return await _planWorkPlanService.GetPageListAsync(pagedQueryDto);
        }

        /// <summary>
        /// 查询详情（生产计划）
        /// </summary>
        /// <param name="planProductId"></param>
        /// <returns></returns>
        [HttpGet("{planProductId}")]
        public async Task<PlanWorkPlanProductDetailDto?> QueryByIdAsync(long planProductId)
        {
            return await _planWorkPlanService.QueryByIdAsync(planProductId);
        }

        /// <summary>
        /// 读取生产计划已经下发的子工单
        /// </summary>
        /// <param name="planProductId"></param>
        /// <returns></returns>
        [HttpGet("orders/{planProductId}")]
        public async Task<IEnumerable<PlanWorkPlanDetailSaveDto>?> QueryOrderByPlanIdAsync(long planProductId)
        {
            return await _planWorkPlanService.QueryOrderByPlanIdAsync(planProductId);
        }

        /// <summary>
        /// 查询ID集合（生产计划物料）
        /// </summary>
        /// <param name="planProductId"></param>
        /// <returns></returns>
        [HttpGet("materials/{planProductId}")]
        public async Task<IEnumerable<PlanWorkPlanMaterialDto>?> QueryMaterialsByMainIdAsync(long planProductId)
        {
            return await _planWorkPlanService.QueryMaterialsByMainIdAsync(planProductId);
        }

        /// <summary>
        /// 根据数量生成拆分预览（生产计划）
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost("split")]
        [PermissionDescription("plan:workPlan:insert")]
        public async Task<IEnumerable<PlanWorkPlanSplitResponseDto>> SplitAsync([FromBody] PlanWorkPlanSplitRequestDto dto)
        {
            return await _planWorkPlanService.SplitAsync(dto);
        }

        /// <summary>
        /// 添加（生产计划）
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost("save")]
        [PermissionDescription("plan:workPlan:insert")]
        public async Task<int> SaveAsync([FromBody] PlanWorkPlanSaveDto dto)
        {
            return await _planWorkPlanService.SaveAsync(dto);
        }

        /// <summary>
        /// 修改（生产计划）
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost("update")]
        [PermissionDescription("plan:workPlan:update")]
        public async Task<int> UpdateAsync([FromBody] PlanWorkPlanUpdateDto dto)
        {
            return await _planWorkPlanService.UpdateAsync(dto);
        }

        /// <summary>
        /// 删除（生产计划）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpDelete("delete")]
        [PermissionDescription("plan:workPlan:delete")]
        public async Task DeletesAsync([FromBody] long[] ids)
        {
            await _planWorkPlanService.DeletesAsync(ids);
        }

    }
}