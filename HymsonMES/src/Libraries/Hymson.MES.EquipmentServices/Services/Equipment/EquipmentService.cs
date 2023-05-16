using Hymson.MES.Core.Domain.Equipment;
using Hymson.MES.Data.Repositories.Equipment;
using Hymson.MES.EquipmentServices.Request.Equipment;
using Hymson.Snowflake;
using Hymson.Utils;
using Hymson.Web.Framework.WorkContext;

namespace Hymson.MES.EquipmentServices.Services.Equipment
{
    /// <summary>
    /// 设备服务
    /// </summary>
    public class EquipmentService : IEquipmentService
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly ICurrentEquipment _currentEquipment;

        /// <summary>
        /// 仓储（设备心跳）
        /// </summary>
        private readonly IEquipmentHeartbeatRepository _equipmentHeartbeatRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="currentEquipment"></param>
        /// <param name="equipmentHeartbeatRepository"></param>
        public EquipmentService(ICurrentEquipment currentEquipment,
            IEquipmentHeartbeatRepository equipmentHeartbeatRepository)
        {
            _currentEquipment = currentEquipment;
            _equipmentHeartbeatRepository = equipmentHeartbeatRepository;
        }


        /// <summary>
        /// 设备心跳
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task EquipmentHeartbeatAsync(EquipmentHeartbeatRequest request)
        {
            var id = _currentEquipment.Id;
            // TODO 

            /*
            var userCode = request.EquipmentCode; //_currentEquipment.Code
            var nowTime = HymsonClock.Now();
            await _equipmentHeartbeatRepository.InsertAsync(new EquipmentHeartbeatEntity
            {
                Id = IdGenProvider.Instance.CreateId(),
                SiteId = _currentEquipment.SiteId,
                CreatedBy = userCode, //_currentEquipment.Code
                CreatedOn = nowTime,
                UpdatedBy = userCode,
                UpdatedOn = nowTime,
                EquipmentId = _currentEquipment.Id ?? 0,
                LastOnLineTime = request.LocalTime,
                Status = request.IsOnline
            });
            await _equipmentHeartbeatRepository.InsertRecordAsync(new EquipmentHeartbeatRecordEntity { });
            */

            await Task.CompletedTask;
        }

        /// <summary>
        /// 设备状态监控
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task EquipmentStateAsync(EquipmentStateRequest request)
        {
            await Task.CompletedTask;
        }

        /// <summary>
        /// 设备报警
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task EquipmentAlarmAsync(EquipmentAlarmRequest request)
        {
            await Task.CompletedTask;
        }

        /// <summary>
        /// 设备停机原因
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task EquipmentDownReasonAsync(EquipmentDownReasonRequest request)
        {
            await Task.CompletedTask;
        }

    }
}
