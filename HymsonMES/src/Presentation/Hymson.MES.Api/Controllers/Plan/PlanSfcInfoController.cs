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
    public class PlanSfcInfoController : ControllerBase
    {
        /// <summary>
        /// 接口（条码接收）
        /// </summary>
        private readonly IPlanSfcInfoService _planSfcInfoService;
        private readonly ILogger<PlanSfcInfoController> _logger;

        /// <summary>
        /// 构造函数（条码接收）
        /// </summary>
        /// <param name="planSfcInfoService"></param>
        public PlanSfcInfoController(IPlanSfcInfoService planSfcInfoService, ILogger<PlanSfcInfoController> logger)
        {
            _planSfcInfoService = planSfcInfoService;
            _logger = logger;
        }

        /// <summary>
        /// 分页查询列表（条码接收）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("pagelist")]
        public async Task<PagedInfo<PlanSfcInfoDto>> QueryPagedPlanSfcInfoAsync([FromQuery] PlanSfcInfoPagedQueryDto parm)
        {
            return await _planSfcInfoService.GetPageListAsync(parm);
        }

        /// <summary>
        /// 查询详情（条码接收）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<PlanSfcInfoDto> QueryPlanSfcInfoByIdAsync(long id)
        {
            return await _planSfcInfoService.QueryPlanSfcInfoByIdAsync(id);
        }

        /// <summary>
        /// 添加（条码接收）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("create")]
        public async Task AddPlanSfcInfoAsync([FromBody] PlanSfcInfoCreateDto parm)
        {
             await _planSfcInfoService.CreatePlanSfcInfoAsync(parm);
        }

        /// <summary>
        /// 更新（条码接收）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("update")]
        public async Task UpdatePlanSfcInfoAsync([FromBody] PlanSfcInfoModifyDto parm)
        {
             await _planSfcInfoService.ModifyPlanSfcInfoAsync(parm);
        }

        /// <summary>
        /// 删除（条码接收）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("delete")]
        public async Task DeletePlanSfcInfoAsync(string ids)
        {
            //long[] idsArr = StringExtension.SpitLongArrary(ids);
            await _planSfcInfoService.DeletesPlanSfcInfoAsync(ids);
        }

    }
}