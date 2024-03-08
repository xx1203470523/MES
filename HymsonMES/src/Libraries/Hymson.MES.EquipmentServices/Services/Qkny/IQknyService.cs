using Hymson.MES.EquipmentServices.Dtos.Qkny.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.EquipmentServices.Services.Qkny
{
    /// <summary>
    /// 顷刻能源服务
    /// </summary>
    public interface IQknyService
    {
        /// <summary>
        /// 操作员登录
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task OperatorLoginAsync(OperationLoginDto dto);

        /// <summary>
        /// 心跳
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task HeartbeatAsync(HeartbeatDto dto);

        /// <summary>
        /// 状态
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task StateAsync(StateDto dto);

        /// <summary>
        /// 报警
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task AlarmAsync(AlarmDto dto);
    }
}
