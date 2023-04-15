/*
 *creator: Karl
 *
 *describe: 工单激活（物理删除）    控制器 | 代码由框架生成  
 *builder:  Karl
 *build datetime: 2023-04-12 11:14:23
 */
using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Plan;
using Hymson.MES.Services.Services.Plan;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.Plan
{
    /// <summary>
    /// 控制器（工单激活（物理删除））
    /// @author Karl
    /// @date 2023-04-12 11:14:23
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class PlanWorkOrderBindController : ControllerBase
    {
        /// <summary>
        /// 接口（工单激活（物理删除））
        /// </summary>
        private readonly IPlanWorkOrderBindService _planWorkOrderBindService;
        private readonly ILogger<PlanWorkOrderBindController> _logger;

        /// <summary>
        /// 构造函数（工单激活（物理删除））
        /// </summary>
        /// <param name="planWorkOrderBindService"></param>
        public PlanWorkOrderBindController(IPlanWorkOrderBindService planWorkOrderBindService, ILogger<PlanWorkOrderBindController> logger)
        {
            _planWorkOrderBindService = planWorkOrderBindService;
            _logger = logger;
        }

        #region 框架生成方法

        ///// <summary>
        ///// 分页查询列表（工单激活（物理删除））
        ///// </summary>
        ///// <param name="parm"></param>
        ///// <returns></returns>
        //[HttpGet]
        //[Route("pagelist")]
        //public async Task<PagedInfo<PlanWorkOrderBindDto>> QueryPagedPlanWorkOrderBindAsync([FromQuery] PlanWorkOrderBindPagedQueryDto parm)
        //{
        //    return await _planWorkOrderBindService.GetPagedListAsync(parm);
        //}

        ///// <summary>
        ///// 查询详情（工单激活（物理删除））
        ///// </summary>
        ///// <param name="id"></param>
        ///// <returns></returns>
        //[HttpGet("{id}")]
        //public async Task<PlanWorkOrderBindDto> QueryPlanWorkOrderBindByIdAsync(long id)
        //{
        //    return await _planWorkOrderBindService.QueryPlanWorkOrderBindByIdAsync(id);
        //}

        ///// <summary>
        ///// 添加（工单激活（物理删除））
        ///// </summary>
        ///// <param name="parm"></param>
        ///// <returns></returns>
        //[HttpPost]
        //[Route("create")]
        //public async Task AddPlanWorkOrderBindAsync([FromBody] PlanWorkOrderBindCreateDto parm)
        //{
        //     await _planWorkOrderBindService.CreatePlanWorkOrderBindAsync(parm);
        //}

        ///// <summary>
        ///// 更新（工单激活（物理删除））
        ///// </summary>
        ///// <param name="parm"></param>
        ///// <returns></returns>
        //[HttpPut]
        //[Route("update")]
        //public async Task UpdatePlanWorkOrderBindAsync([FromBody] PlanWorkOrderBindModifyDto parm)
        //{
        //     await _planWorkOrderBindService.ModifyPlanWorkOrderBindAsync(parm);
        //}

        ///// <summary>
        ///// 删除（工单激活（物理删除））
        ///// </summary>
        ///// <param name="ids"></param>
        ///// <returns></returns>
        //[HttpDelete]
        //[Route("delete")]
        //public async Task DeletePlanWorkOrderBindAsync([FromBody] long[] ids)
        //{
        //    await _planWorkOrderBindService.DeletesPlanWorkOrderBindAsync(ids);
        //}

        #endregion

        /// <summary>
        /// 绑定/取消绑定工单
        /// </summary>
        /// <param name="bindActivationWorkOrder"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("bindActivationWorkOrder")]
        public async Task BindActivationWorkOrder(BindActivationWorkOrderDto bindActivationWorkOrder) 
        {
            await _planWorkOrderBindService.BindActivationWorkOrder(bindActivationWorkOrder);
        }

        /// <summary>
        /// 获取资源id上已经绑定的工单
        /// </summary>
        /// <param name="resourceId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("getHasBindWorkOrder/{resourceId}")]
        public async Task<List<HasBindWorkOrderInfoDto>> GetHasBindWorkOrder(long resourceId) 
        {
             return await _planWorkOrderBindService.GetHasBindWorkOrder(resourceId);
        }
    }
}