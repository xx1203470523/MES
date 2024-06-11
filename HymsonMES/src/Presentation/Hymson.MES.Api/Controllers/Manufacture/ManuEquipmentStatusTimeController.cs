using Hymson.Infrastructure;
using Hymson.MES.Api.Controllers.Report;
using Hymson.MES.Services.Dtos.Manufacture;
using Hymson.MES.Services.Dtos.Report;
using Hymson.MES.Services.Services.Manufacture;
using Hymson.MES.Services.Services.Report;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.Manufacture
{
    /// <summary>
    /// 控制器（不良报告）
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ManuEquipmentStatusTimeController : ControllerBase
    {
        /// <summary>
        /// 接口（不良报告）
        /// </summary>
        private readonly IManuEquipmentStatusTimeService _equipmentStatusTimeService;
        private readonly ILogger<ManuEquipmentStatusTimeController> _logger;

        /// <summary>
        /// 构造函数（不良报告）
        /// </summary>
        /// <param name="equipmentStatusTimeService"></param>
        /// <param name="logger"></param>
        public ManuEquipmentStatusTimeController(IManuEquipmentStatusTimeService equipmentStatusTimeService, ILogger<ManuEquipmentStatusTimeController> logger)
        {
            _equipmentStatusTimeService = equipmentStatusTimeService;
            _logger = logger;
        }

        /// <summary>
        /// 分页查询列表
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("pagelist")]
        public async Task<PagedInfo<ManuEquipmentStatusReportViewDto>> QueryPagedBadRecordReportAsync([FromQuery] ManuEquipmentStatusTimePagedQueryDto parm)
        {
            return await _equipmentStatusTimeService.GetPageListAsync(parm);
        }
    }
}