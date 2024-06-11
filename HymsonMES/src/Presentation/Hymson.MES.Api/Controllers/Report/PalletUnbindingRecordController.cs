
using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Report;
using Hymson.MES.Services.Services.Report.PalletUnbindingRecord;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.Report
{
    /// <summary>
    /// 控制器（托盘解绑记录）
    /// @author luoxichang 
    /// @date 2023-09-07 16:23:17
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class PalletUnbindingRecordController : ControllerBase
    {
        /// <summary>
        /// 接口（托盘解绑记录）
        /// </summary>
        private readonly IPalletUnbindingRecordService _palletUnbindingRecordService;

        public PalletUnbindingRecordController(IPalletUnbindingRecordService palletUnbindingRecordService)
        {
            _palletUnbindingRecordService = palletUnbindingRecordService;
        }

        /// <summary>
        /// 分页查询列表（托盘解绑记录）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("pagelist")]
        public async Task<PagedInfo<VehicleFreightRecordDto>> GetVehicleFreightRecorPageListAsync([FromQuery] VehicleFreightRecordQueryDto parm)
        {
            return await _palletUnbindingRecordService.GetVehicleFreightRecorPageListAsync(parm);
        }
    }
}