using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Common;
using Hymson.MES.Services.Dtos.Report;
using Hymson.MES.Services.Services.Report.ManuProductParameterReport;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.Report
{
    /// <summary>
    /// 设备报警报表控制器
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ManuProductParameterReportController : ControllerBase
    {
        private readonly IManuProductParameterReportService _manuProductParameterReportService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="manuProductParameterReportService"></param>
        public ManuProductParameterReportController(IManuProductParameterReportService manuProductParameterReportService)
        {
            _manuProductParameterReportService = manuProductParameterReportService;
        }
        /// <summary>
        /// 根据查询条件获取产品参数报表分页数据
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet]//后续查询条码过多可能需要换成POST
        [Route("pagelist")]
        public async Task<PagedInfo<ManuProductParameterReportViewDto>> GetManuProductParameterReportPageListAsync([FromQuery] ManuProductParameterReportPagedQueryDto param)
        {
            return await _manuProductParameterReportService.GetManuProductParameterReportPageListAsync(param);
        }

        /// <summary>
        /// 产品参数导出
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet("export")]
        public async Task<ExportResultDto> ManuProductParameterReportExportAsync([FromQuery] ManuProductParameterReportPagedQueryDto param)
        {
            return await _manuProductParameterReportService.ManuProductParameterReportExportAsync(param);
        }

    }
}
