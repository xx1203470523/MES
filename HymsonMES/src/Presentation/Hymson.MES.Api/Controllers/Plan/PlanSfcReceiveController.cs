/*
 *creator: Karl
 *
 *describe: 条码接收    控制器 | 代码由框架生成  
 *builder:  pengxin
 *build datetime: 2023-03-21 04:33:58
 */
using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Plan;
using Hymson.MES.Services.Services.Plan;
//using Hymson.Utils.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Extensions;

namespace Hymson.MES.Api.Controllers.Plan
{
    /// <summary>
    /// 控制器（条码接收）
    /// @author pengxin
    /// @date 2023-03-21 04:33:58
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class PlanSfcReceiveController : ControllerBase
    {
        /// <summary>
        /// 接口（条码接收）
        /// </summary>
        private readonly IPlanSfcReceiveService _planSfcInfoService;
        private readonly ILogger<PlanSfcReceiveController> _logger;

        /// <summary>
        /// 构造函数（条码接收）
        /// </summary>
        /// <param name="planSfcInfoService"></param>
        public PlanSfcReceiveController(IPlanSfcReceiveService planSfcInfoService, ILogger<PlanSfcReceiveController> logger)
        {
            _planSfcInfoService = planSfcInfoService;
            _logger = logger;
        }

        /// <summary>
        /// 分页查询列表（条码接收）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("pagelist")]
        public async Task<PagedInfo<PlanSfcReceiveDto>> QueryPagedPlanSfcInfoAsync([FromQuery] PlanSfcReceivePagedQueryDto parm)
        {
            return await _planSfcInfoService.GetPageListAsync(parm);
        }

        /// <summary>
        /// 添加（条码接收）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("create")]
        public async Task AddPlanSfcInfoAsync([FromBody] PlanSfcReceiveCreateDto parm)
        {
            await _planSfcInfoService.CreatePlanSfcInfoAsync(parm);
        }


        /// <summary>
        /// 删除（条码接收）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("delete")]
        public async Task DeletePlanSfcInfoAsync(long[] ids)
        {
            await _planSfcInfoService.DeletesPlanSfcInfoAsync(ids);
        }

    }
}