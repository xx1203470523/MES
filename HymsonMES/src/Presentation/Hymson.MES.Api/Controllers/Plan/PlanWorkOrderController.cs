using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Plan;
using Hymson.MES.Services.Services.Plan;
using Hymson.Web.Framework.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.Plan
{
    /// <summary>
    /// 控制器（工单信息表）
    /// @author Karl
    /// @date 2023-03-20 10:07:17
    /// </summary>

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
        public async Task<PagedInfo<PlanWorkOrderListDetailViewDto>> QueryPagedPlanWorkOrderAsync([FromQuery] PlanWorkOrderPagedQueryDto parm)
        {
            return await _planWorkOrderService.GetPageListAsync(parm);
        }

        /// <summary>
        /// 查询详情（工单信息表）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<PlanWorkOrderDetailViewDto> QueryPlanWorkOrderByIdAsync(long id)
        {
            return await _planWorkOrderService.QueryPlanWorkOrderByIdAsync(id);
        }

        /// <summary>
        /// 查询剩余可下单条码数量
        /// </summary>
        /// <param name="workOrderCode"></param>
        /// <returns></returns>
        [HttpGet("residue/{workOrderCode}")]
        public async Task<decimal> GetPlanWorkOrderByWorkOrderCodeAsync(string workOrderCode)
        {
            return await _planWorkOrderService.GetPlanWorkOrderByWorkOrderCodeAsync(workOrderCode);
        }

        /// <summary>
        /// 添加（工单信息表）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("create")]
        [LogDescription("生产工单", BusinessType.INSERT)]
        [PermissionDescription("plan:workOrder:insert")]
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
        [LogDescription("生产工单", BusinessType.UPDATE)]
        [PermissionDescription("plan:workOrder:update")]
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
        [LogDescription("生产工单", BusinessType.DELETE)]
        [PermissionDescription("plan:workOrder:delete")]
        public async Task DeletePlanWorkOrderAsync([FromBody] long[] ids)
        {
            await _planWorkOrderService.DeletesPlanWorkOrderAsync(ids);
        }

        /// <summary>
        /// 修改订单状态,工单下达
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        [Route("changWorkOrderStatus")]
        [PermissionDescription("plan:workOrder:changWorkOrderStatus")]
        public async Task ChangWorkOrderStatusAsync(List<PlanWorkOrderChangeStatusDto> parms)
        {
            await _planWorkOrderService.ModifyWorkOrderStatusAsync(parms);
        }

        /// <summary>
        /// 改变锁定/解锁
        /// </summary>
        /// <param name="parms"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("changLocked")]
        [PermissionDescription("plan:workOrder:changLocked")]
        public async Task ChangLockedAsync(List<PlanWorkOrderLockedDto> parms)
        {
            await _planWorkOrderService.ModifyWorkOrderLockedAsync(parms);
        }
    }
}