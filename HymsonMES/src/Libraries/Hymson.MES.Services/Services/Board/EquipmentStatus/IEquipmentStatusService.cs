using Hymson.MES.Core.Domain.ManuEuqipmentNewestInfoEntity;
using Hymson.MES.Data.Repositories.Equipment.Qkny.ManuEuqipmentNewestInfo.View;
using Hymson.MES.Services.Dtos.Board;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Services.Services.Board.EquipmentStatus
{
    /// <summary>
    /// 设备状态
    /// </summary>
    public interface IEquipmentStatusService
    {
        /// <summary>
        /// 获取设备最新信息
        /// </summary>
        /// <returns></returns>
        Task<List<ManuEquipmentNewestInfoView>> GetEquNewestInfoList(EquipmentNewestInfoQueryDto queryDto);
    }
}
