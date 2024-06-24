using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.EquSparepartRecord;
using Hymson.MES.Services.Dtos.Report;
using Hymson.MES.Services.Services.EquSparepartRecord;
using Hymson.MES.Services.Services.Report;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.Report
{
    /// <summary>
    /// 控制器（备件记录）
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class EquSparepartRecordController : ControllerBase
    {
        /// <summary>
        /// 接口（备件记录）
        /// </summary> 
        private readonly IEquSparepartRecordService _equSparepartRecordService;
        private readonly ILogger<EquSparepartRecordController> _logger;

        /// <summary>
        /// 构造函数（备件记录）
        /// </summary>
        /// <param name="equSparepartRecordService"></param>
        /// <param name="logger"></param>
        public EquSparepartRecordController(ILogger<EquSparepartRecordController> logger, IEquSparepartRecordService equSparepartRecordService)
        {
            _logger = logger;
            _equSparepartRecordService = equSparepartRecordService;
        }


        /// <summary>
        /// 分页查询列表（备件记录）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("pagelist")]
        public async Task<PagedInfo<EquSparepartRecordPagedViewDto>> QueryPagedBadRecordReportAsync([FromQuery] EquSparepartRecordPagedQueryDto parm)
        {
            return await _equSparepartRecordService.GetPagedListAsync(parm);
        }

    }
}