using Hymson.MES.Core.Domain.Equipment;
using Hymson.MES.Data.Repositories.Equipment;
using Hymson.MES.EquipmentServices.Request.Equipment;
using Hymson.Snowflake;
using Hymson.Utils;
using Hymson.Utils.Tools;
using Hymson.Web.Framework.WorkContext;

namespace Hymson.MES.EquipmentServices.Services.Equipment
{
    /// <summary>
    /// 设备服务
    /// </summary>
    public class EquipmentMonitorService : IEquipmentMonitorService
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
        /// 仓储（设备报警）
        /// </summary>
        private readonly IEquipmentAlarmRepository _equipmentAlarmRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="currentEquipment"></param>
        /// <param name="equipmentHeartbeatRepository"></param>
        /// <param name="equipmentAlarmRepository"></param>
        public EquipmentMonitorService(ICurrentEquipment currentEquipment,
            IEquipmentHeartbeatRepository equipmentHeartbeatRepository,
            IEquipmentAlarmRepository equipmentAlarmRepository)
        {
            _currentEquipment = currentEquipment;
            _equipmentHeartbeatRepository = equipmentHeartbeatRepository;
            _equipmentAlarmRepository = equipmentAlarmRepository;
        }


        /// <summary>
        /// 设备心跳
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task EquipmentHeartbeatAsync(EquipmentHeartbeatRequest request)
        {
            var userCode = request.EquipmentCode; //_currentEquipment.Code
            var nowTime = HymsonClock.Now();

            var entity = new EquipmentHeartbeatEntity
            {
                Id = IdGenProvider.Instance.CreateId(),
                SiteId = _currentEquipment.SiteId,
                CreatedBy = userCode,
                CreatedOn = nowTime,
                UpdatedBy = userCode,
                UpdatedOn = nowTime,
                EquipmentId = _currentEquipment.Id ?? 0,
                Status = request.IsOnline,
                LastOnLineTime = request.LocalTime
            };

            using var trans = TransactionHelper.GetTransactionScope();
            await _equipmentHeartbeatRepository.InsertAsync(entity);
            await _equipmentHeartbeatRepository.InsertRecordAsync(new EquipmentHeartbeatRecordEntity
            {
                Id = IdGenProvider.Instance.CreateId(),
                SiteId = entity.SiteId,
                CreatedBy = entity.CreatedBy,
                CreatedOn = entity.CreatedOn,
                UpdatedBy = entity.UpdatedBy,
                UpdatedOn = entity.UpdatedOn,
                EquipmentId = entity.EquipmentId,
                Status = entity.Status,
                LocalTime = request.LocalTime
            });
            trans.Complete();
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
            var userCode = request.EquipmentCode; //_currentEquipment.Code
            var nowTime = HymsonClock.Now();

            await _equipmentAlarmRepository.InsertAsync(new EquipmentAlarmEntity
            {
                Id = IdGenProvider.Instance.CreateId(),
                SiteId = _currentEquipment.SiteId,
                CreatedBy = userCode,
                CreatedOn = nowTime,
                UpdatedBy = userCode,
                UpdatedOn = nowTime,
            });
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
