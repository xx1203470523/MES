using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Report;
using Hymson.MES.Services.Services.Report.MaterialDeductionRecord;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.Report
{
    /// <summary>
    /// 控制器（扣料记录）
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class MaterialDeductionReportController : ControllerBase
    {
        /// <summary>
        /// 接口（扣料记录）
        /// </summary>
        private readonly ILogger<MaterialDeductionReportController> _logger;

        private readonly IMaterialDeductionRecordService _materialDeductionRecordService;

        /// <summary>
        /// 构造函数（扣料记录）
        /// </summary>
        /// <param name="materialDeductionRecordService"></param>
        /// <param name="logger"></param>
        public MaterialDeductionReportController(ILogger<MaterialDeductionReportController> logger, IMaterialDeductionRecordService materialDeductionRecordService)
        {
            _logger = logger;
            _materialDeductionRecordService = materialDeductionRecordService;
        }

        /// <summary>
        /// 分页查询列表（扣料记录）
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("pagelist")]
        public async Task<PagedInfo<MaterialDeductionRecordResultDto>> QueryPagedComUsageAsync([FromQuery] ComUsageReportPagedQueryDto param)
        {
            return await _materialDeductionRecordService.GetMaterialDeductionRecorPageListAsync(param);
        }
    }
}