using Hymson.MES.Data.Repositories.Equipment.Qkny.ManuEuqipmentNewestInfo.View;
using Hymson.MES.Data.Repositories.ManuEuqipmentNewestInfo.Query;
using Hymson.MES.Services.Dtos.Board;
using Hymson.MES.Services.Services.Board.EquipmentStatus;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.Board
{
    /// <summary>
    /// 设备状态看板
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    [AllowAnonymous]
    public class EquipmentStatusBoardController : ControllerBase
    {
        /// <summary>
        /// 设备状态
        /// </summary>
        private readonly IEquipmentStatusService _equipmentStatusService;

        /// <summary>
        /// 构造函数
        /// </summary>
        public EquipmentStatusBoardController(IEquipmentStatusService equipmentStatusService)
        {
            _equipmentStatusService = equipmentStatusService;
        }

        /// <summary>
        /// 获取设备最新信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetEquNewestInfoList")]
        public async Task<List<ManuEquipmentNewestInfoView>> GetEquNewestInfoList([FromQuery]EquipmentNewestInfoQueryDto queryDto)
        {
            if(queryDto.SiteId == 0)
            {
                queryDto.SiteId = 42874561778253824;
            }
            EquipmentNewestInfoQueryDto query = new EquipmentNewestInfoQueryDto() { SiteId = queryDto.SiteId };
            var dataList = await _equipmentStatusService.GetEquNewestInfoList(query);

            return dataList!;
        }
    }
}
