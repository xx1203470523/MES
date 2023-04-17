using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Manufacture;
using Hymson.MES.Services.Dtos.Plan;
using Hymson.MES.Services.Services.Manufacture.ManuSfc;
using Hymson.MES.Services.Services.Plan;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.Plan
{
    /// <summary>
    /// 控制器（条码打印）
    /// @author pengxin
    /// @date 2023-03-21 04:33:58
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class PlanSfcPrintController : ControllerBase
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly ILogger<PlanSfcPrintController> _logger;

        /// <summary>
        /// 接口（条码打印）
        /// </summary>
        private readonly IPlanSfcPrintService _planSfcInfoService;

        /// <summary>
        /// 接口（条码）
        /// </summary>
        private readonly IManuSfcService _manuSfcService;

        /// <summary>
        /// 构造函数（条码打印）
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="planSfcInfoService"></param>
        /// <param name="manuSfcService"></param>
        public PlanSfcPrintController(ILogger<PlanSfcPrintController> logger,
            IPlanSfcPrintService planSfcInfoService,
            IManuSfcService manuSfcService)
        {
            _logger = logger;
            _planSfcInfoService = planSfcInfoService;
            _manuSfcService = manuSfcService;
        }


        /// <summary>
        /// 分页查询列表（条码打印）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("pagelist")]
        public async Task<PagedInfo<PlanSfcPrintDto>> QueryPagedPlanSfcInfoAsync([FromQuery] PlanSfcPrintPagedQueryDto parm)
        {
            return await _planSfcInfoService.GetPageListAsync(parm);
        }

        /// <summary>
        /// 分页查询列表（条码打印）
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("page")]
        public async Task<PagedInfo<ManuSfcPassDownDto>> GetPagedListAsync([FromQuery] ManuSfcPassDownPagedQueryDto pagedQueryDto)
        {
            return await _manuSfcService.GetPagedListAsync(pagedQueryDto);
        }



        /// <summary>
        /// 添加（条码打印）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("create")]
        public async Task AddPlanSfcInfoAsync([FromBody] PlanSfcPrintCreateDto parm)
        {
            await _planSfcInfoService.CreatePlanSfcInfoAsync(parm);
        }

        /// <summary>
        /// 删除（条码打印）
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