using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Report;
using Hymson.MES.Services.Services.Report.EquAlarmReport;
using Hymson.Web.Framework.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.Report
{
    /// <summary>
    /// 设备报警报表控制器
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class EquAlarmReportController : ControllerBase
    {
        private readonly IEquAlarmReportService _equAlarmReportService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="equAlarmReportService"></param>
        public EquAlarmReportController(IEquAlarmReportService equAlarmReportService)
        {
            _equAlarmReportService = equAlarmReportService;
        }
        /// <summary>
        /// 根据查询条件获取设备报警报表分页数据
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("pagelist")]
        public async Task<PagedInfo<EquAlarmReportViewDto>> GetEquAlarmReportPageListAsync([FromQuery] EquAlarmReportPagedQueryDto param)
        {
            return await _equAlarmReportService.GetEquAlarmReportPageListAsync(param);
        }

        /// <summary>
        /// 设备报警导出
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        //[HttpGet("export")]
        //public async Task<EquAlarmReportViewDto> EquAlarmReportExport([FromQuery] EquAlarmReportPagedQueryDto param)
        //{
        //    return await _equAlarmReportService.GetEquAlarmReportPageListAsync(param);
        //}
    }
}
