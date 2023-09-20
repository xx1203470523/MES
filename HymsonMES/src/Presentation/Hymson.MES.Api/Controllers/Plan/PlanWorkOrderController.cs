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
        /// 模糊查询工单
        /// </summary>
        /// <param name="workOrderCode"></param>
        /// <returns></returns>
        [HttpGet("fuzzy/{workOrderCode}")]
        public async Task<IEnumerable<PlanWorkOrderDto>> QueryPlanWorkOrderByWorkOrderCodeAsync(string workOrderCode)
        {
            return await _planWorkOrderService.QueryPlanWorkOrderByWorkOrderCodeAsync(workOrderCode);
        }


        /// <summary>
        /// 获取资源id上已经绑定的工单
        /// </summary>
        /// <param name="resourceId"></param>
        /// <returns></returns>
        //[HttpGet]
        //[Route("getHasBindWorkOrder/{resourceId}")]
        //public async Task<List<HasBindWorkOrderInfoDto>> GetHasBindWorkOrder(long resourceId)
        //{
        //    return await _planWorkOrderBindService.GetHasBindWorkOrderAsync(resourceId);
        //}

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
        [Route("sendDown")]
        [PermissionDescription("plan:workOrder:sendDown")]
        public async Task SendDownWorkOrderStatusAsync(List<PlanWorkOrderChangeStatusDto> parms)
        {
            await _planWorkOrderService.ModifyWorkOrderStatusAsync(parms);
        }

        /// <summary>
        /// 工单完工
        /// </summary>
        /// <param name="parms"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("finish")]
        [PermissionDescription("plan:workOrder:finish")]
        public async Task FinishWorkOrderStatusAsync(List<PlanWorkOrderChangeStatusDto> parms)
        {
            await _planWorkOrderService.ModifyWorkOrderStatusAsync(parms);
        }

        /// <summary>
        /// 关闭完工
        /// </summary>
        /// <param name="parms"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("closeOrder")]
        [PermissionDescription("plan:workOrder:closeOrder")]
        public async Task CloseWorkOrderStatusAsync(List<PlanWorkOrderChangeStatusDto> parms)
        {
            await _planWorkOrderService.ModifyWorkOrderStatusAsync(parms);
        }

        /// <summary>
        /// 锁定
        /// </summary>
        /// <param name="parms"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("locked")]
        [PermissionDescription("plan:workOrder:locked")]
        public async Task LockedAsync(List<PlanWorkOrderLockedDto> parms)
        {
            await _planWorkOrderService.ModifyWorkOrderLockedAsync(parms);
        }

        /// <summary>
        /// 解锁
        /// </summary>
        /// <param name="parms"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("cancelLocked")]
        [PermissionDescription("plan:workOrder:cancelLocked")]
        public async Task CancelLockedAsync(List<PlanWorkOrderLockedDto> parms)
        {
            await _planWorkOrderService.ModifyWorkOrderLockedAsync(parms);
        }
    }
}