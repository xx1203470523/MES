/*
 *creator: Karl
 *
 *describe: 工单信息表    控制器 | 代码由框架生成  
 *builder:  Karl
 *build datetime: 2023-03-20 10:07:17
 */
using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Plan;
using Hymson.MES.Services.Services.Plan;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Extensions;

namespace Hymson.MES.Api.Controllers.Plan
{
    /// <summary>
    /// 控制器（工单信息表）
    /// @author Karl
    /// @date 2023-03-20 10:07:17
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class PlanWorkOrderController : ControllerBase
    {
        /// <summary>
        /// 接口（工单信息表）
        /// </summary>
        private readonly IPlanWorkOrderService _planWorkOrderService;
        private readonly ILogger<PlanWorkOrderController> _logger;

        /// <summary>
        /// 构造函数（工单信息表）
        /// </summary>
        /// <param name="planWorkOrderService"></param>
        public PlanWorkOrderController(IPlanWorkOrderService planWorkOrderService, ILogger<PlanWorkOrderController> logger)
        {
            _planWorkOrderService = planWorkOrderService;
            _logger = logger;
        }

        /// <summary>
        /// 分页查询列表（工单信息表）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("pagelist")]
        public async Task<PagedInfo<PlanWorkOrderDto>> QueryPagedPlanWorkOrderAsync([FromQuery] PlanWorkOrderPagedQueryDto parm)
        {
            return await _planWorkOrderService.GetPageListAsync(parm);
        }

        /// <summary>
        /// 查询详情（工单信息表）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<PlanWorkOrderDetailView> QueryPlanWorkOrderByIdAsync(long id)
        {
            return await _planWorkOrderService.QueryPlanWorkOrderByIdAsync(id);
        }

        /// <summary>
        /// 添加（工单信息表）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("create")]
        public async Task AddPlanWorkOrderAsync([FromBody] PlanWorkOrderCreateDto parm)
        {
             await _planWorkOrderService.CreatePlanWorkOrderAsync(parm);
        }

        /// <summary>
        /// 更新（工单信息表）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("update")]
        public async Task UpdatePlanWorkOrderAsync([FromBody] PlanWorkOrderModifyDto parm)
        {
             await _planWorkOrderService.ModifyPlanWorkOrderAsync(parm);
        }

        /// <summary>
        /// 删除（工单信息表）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("delete")]
        public async Task DeletePlanWorkOrderAsync([FromBody] long[] ids)
        {
            await _planWorkOrderService.DeletesPlanWorkOrderAsync(ids);
        }

    }
}