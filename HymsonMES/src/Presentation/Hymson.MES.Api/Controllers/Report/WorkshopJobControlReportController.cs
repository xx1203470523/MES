using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.NioPushCollection;
using Hymson.MES.Services.Dtos.Report;
using Hymson.MES.Services.Services.Report;
using Hymson.Web.Framework.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.Report
{
    /// <summary>
    /// 控制器（车间作业控制报告）
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class WorkshopJobControlReportController : ControllerBase
    {
        /// <summary>
        /// 接口（不良报告）
        /// </summary>
        private readonly ILogger<BadRecordReportController> _logger;

        private readonly IWorkshopJobControlReportService _workshopJobControlReportService;

        /// <summary>
        /// 构造函数（不良报告）
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="workshopJobControlReportService"></param>
        public WorkshopJobControlReportController(ILogger<BadRecordReportController> logger, IWorkshopJobControlReportService workshopJobControlReportService)
        {
            _logger = logger;
            _workshopJobControlReportService = workshopJobControlReportService;
        }


        /// <summary>
        /// 分页查询列表（车间作业控制）
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("pagelist")]
        public async Task<PagedInfo<WorkshopJobControlReportViewDto>> QueryPagedWorkshopJobControlAsync([FromQuery] WorkshopJobControlReportOptimizePagedQueryDto param)
        {
            return await _workshopJobControlReportService.GetWorkshopJobControlPageListAsync(param);
        }

        /// 导出信息
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("export")]
        [PermissionDescription("proc:workshopJobControlReport:export")]
        public async Task<NioPushCollectionExportResultDto> ExprotAsync([FromQuery] WorkshopJobControlReportOptimizePagedQueryDto param)
        {
            return await _workshopJobControlReportService.ExprotAsync(param);
        }
        

        /// <summary>
        /// 获取SFC的车间作业控制步骤
        /// </summary>
        /// <param name="sfc"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("getSfcInOut/{sfc}")]
        public async Task<WorkshopJobControlStepReportDto> QuerySfcInOutRecordAsync(string sfc)
        {
            return await _workshopJobControlReportService.GetSfcInOutInfoAsync(sfc);
        }

        /// <summary>
        /// 根据SFC分页获取条码步骤信息
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("getSfcStepBySFCPageList")]
        public async Task<PagedInfo<ManuSfcStepBySfcViewDto>> QueryPagedSFCStepBySFCAsync([FromQuery] ManuSfcStepBySfcPagedQueryDto param)
        {
            return await _workshopJobControlReportService.GetSFCStepsBySFCPageListAsync(param);
        }

        /// <summary>
        ///获取步骤详情
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("getStepDetail")]
        public async Task<StepDetailDto> GetStepDetailAsync([FromQuery] StepQueryDto param)
        {
            return await _workshopJobControlReportService.GetStepDetailAsync(param);
        }
    }
}