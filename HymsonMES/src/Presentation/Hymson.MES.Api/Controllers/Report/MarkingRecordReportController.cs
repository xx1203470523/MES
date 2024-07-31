using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Marking;
using Hymson.MES.Services.Services.Marking;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.Marking
{
    /// <summary>
    /// 控制器（Marking拦截汇总表）
    /// @author Kongaomeng
    /// @date 2023-09-21 03:58:50
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class MarkingRecordReportController : ControllerBase
    {
        /// <summary>
        /// 日志
        /// </summary>
        private readonly ILogger<MarkingRecordReportController> _logger;
        /// <summary>
        /// 服务接口（Marking拦截汇总表）
        /// </summary>
        private readonly IMarkingRecordReportService _markingInterceptReportService;

        /// <summary>
        /// 构造函数（Marking拦截汇总表）
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="markingInterceptReportService"></param>
        public MarkingRecordReportController(ILogger<MarkingRecordReportController> logger, IMarkingRecordReportService markingInterceptReportService)
        {
            _logger = logger;
            _markingInterceptReportService = markingInterceptReportService;
        }
        /// <summary>
        /// 分页查询列表（Marking拦截汇总表）
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("pagelist")]
        public async Task<PagedInfo<MarkingRecordReportDto>> QueryPagedMarkingInterceptReportAsync([FromQuery] MarkingInterceptReportPagedQueryDto pagedQueryDto)
        {
            return await _markingInterceptReportService.GetPagedListAsync(pagedQueryDto);
        }

        /// <summary>
        /// 导出查询数据
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("export")]
        public async Task<MarkingRecordExportResultDto> ExprotListAsync([FromQuery] MarkingInterceptReportPagedQueryDto param)
        {
            return await _markingInterceptReportService.ExprotListAsync(param);
        }
    }
}