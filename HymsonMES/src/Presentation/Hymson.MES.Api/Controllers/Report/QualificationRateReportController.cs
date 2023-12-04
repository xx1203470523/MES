using Hymson.Infrastructure;
using Hymson.MES.CoreServices.Dtos.Common;
using Hymson.MES.Services.Dtos.Common;
using Hymson.MES.Services.Dtos.QualificationRateReport;
using Hymson.MES.Services.Dtos.Report;
using Hymson.MES.Services.Services.QualificationRateReport;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.QualificationRateReport
{
    /// <summary>
    /// 控制器（合格率报表）
    /// @author hjy
    /// @date 2023-11-27 10:19:24
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class QualificationRateReportController : ControllerBase
    {
        /// <summary>
        /// 日志
        /// </summary>
        private readonly ILogger<QualificationRateReportController> _logger;
        /// <summary>
        /// 服务接口（合格率报表）
        /// </summary>
        private readonly IQualificationRateReportService _qualificationRateReportService;


        /// <summary>
        /// 构造函数（合格率报表）
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="qualificationRateReportService"></param>
        public QualificationRateReportController(ILogger<QualificationRateReportController> logger, IQualificationRateReportService qualificationRateReportService)
        {
            _logger = logger;
            _qualificationRateReportService = qualificationRateReportService;
        }

        /// <summary>
        /// 分页查询列表（合格率报表）
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("pagelist")]
        public async Task<PagedInfo<QualificationRateReportDto>> QueryPagedQualificationRateReportAsync([FromQuery] QualificationRateReportPagedQueryDto pagedQueryDto)
        {
            return await _qualificationRateReportService.GetPagedListAsync(pagedQueryDto);
        }

        /// <summary>
        /// 查询所有工序（合格率报表）
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("procedures")]
        public async Task<IEnumerable<SelectOptionDto>> GetProcdureInfoAsync()
        {
            return await _qualificationRateReportService.GetProcdureListAsync();
        }

        /// <summary>
        /// 报表-产能报表：导出
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("export")]
        public async Task<ExportResultDto> ExportExcelAsync([FromQuery] QualificationRateReportPagedQueryDto pageQueryDto)
        {
            return await _qualificationRateReportService.ExportExcelAsync(pageQueryDto);
        }
    }
}