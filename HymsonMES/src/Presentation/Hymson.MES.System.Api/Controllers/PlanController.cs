﻿using Hymson.MES.SystemServices.Dtos;
using Hymson.MES.SystemServices.Services.Plan;
using Hymson.Web.Framework.Attributes;
using Hymson.Web.Framework.Filters.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.System.Api.Controllers
{
    /// <summary>
    /// 计划
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PlanController : ControllerBase
    {
        /// <summary>
        /// 接口（生产计划）
        /// </summary>
        private readonly IPlanWorkPlanService _planWorkPlanService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="planWorkPlanService"></param>
        public PlanController(IPlanWorkPlanService planWorkPlanService)
        {
            _planWorkPlanService = planWorkPlanService;
        }

        /// <summary>
        /// 生产计划下发
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpPost("WorkPlan/create")]
        [ProducesResponseType(typeof(ResultDto), 200)]
        [LogDescription("生产计划下发", BusinessType.INSERT)]
        public async Task CreateWorkOrderAsync(WorkPlanDto requestDto)
        {
            await _planWorkPlanService.CreateAsync(requestDto);
        }

    }
}