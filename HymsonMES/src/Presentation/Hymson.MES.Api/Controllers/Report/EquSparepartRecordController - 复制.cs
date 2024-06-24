using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.EquEquipmentRecord;
using Hymson.MES.Services.Dtos.EquSparepartRecord;
using Hymson.MES.Services.Dtos.Report;
using Hymson.MES.Services.Services.EquEquipmentRecord;
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
    public class EquEquipmentRecordController : ControllerBase
    {
        /// <summary>
        /// 接口（备件记录）
        /// </summary> 
        private readonly IEquEquipmentRecordService _equEquipmentRecordService;
        private readonly ILogger<EquEquipmentRecordController> _logger;

        /// <summary>
        /// 构造函数（备件记录）
        /// </summary>
        /// <param name="equEquipmentRecordService"></param>
        /// <param name="logger"></param>
        public EquEquipmentRecordController(ILogger<EquEquipmentRecordController> logger, IEquEquipmentRecordService equEquipmentRecordService)
        {
            _logger = logger;
            _equEquipmentRecordService = equEquipmentRecordService;
        }


        /// <summary>
        /// 分页查询列表（备件记录）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("pagelist")]
        public async Task<PagedInfo<EquEquipmentRecordPagedViewDto>> QueryPagedBadRecordReportAsync([FromQuery] EquEquipmentRecordPagedQueryDto parm)
        {
            return await _equEquipmentRecordService.GetPagedListAsync(parm);
        }

    }
}