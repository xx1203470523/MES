using Hymson.MES.Core.Domain.Equipment;
using Hymson.MES.Core.Enums;
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
        private readonly IEquHeartbeatRepository _equipmentHeartbeatRepository;

        /// <summary>
        /// 仓储（设备报警）
        /// </summary>
        private readonly IEquAlarmRepository _equipmentAlarmRepository;

        /// <summary>
        /// 仓储（设备状态）
        /// </summary>
        private readonly IEquStatusRepository _equipmentStatusRepository;

        /// <summary>
        /// 仓储（设备状态）
        /// </summary>

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="currentEquipment"></param>
        /// <param name="equipmentHeartbeatRepository"></param>
        /// <param name="equipmentAlarmRepository"></param>
        /// <param name="equipmentStatusRepository"></param>
        public EquipmentMonitorService(ICurrentEquipment currentEquipment,
            IEquHeartbeatRepository equipmentHeartbeatRepository,
            IEquAlarmRepository equipmentAlarmRepository,
            IEquStatusRepository equipmentStatusRepository)
        {
            _currentEquipment = currentEquipment;
            _equipmentHeartbeatRepository = equipmentHeartbeatRepository;
            _equipmentAlarmRepository = equipmentAlarmRepository;
            _equipmentStatusRepository = equipmentStatusRepository;
        }


        /// <summary>
        /// 设备心跳
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task EquipmentHeartbeatAsync(EquipmentHeartbeatRequest request)
        {
            // TODO
            var userCode = request.EquipmentCode; //_currentEquipment.Code
            var nowTime = HymsonClock.Now();

            var entity = new EquHeartbeatEntity
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
            await _equipmentHeartbeatRepository.InsertRecordAsync(new EquHeartbeatRecordEntity
            {
                Id = IdGenProvider.Instance.CreateId(),
                SiteId = entity.SiteId,
                CreatedBy = entity.CreatedBy,
                CreatedOn = entity.CreatedOn,
                UpdatedBy = entity.UpdatedBy,
                UpdatedOn = entity.UpdatedOn,
                EquipmentId = entity.EquipmentId,
                LocalTime = request.LocalTime,
                Status = entity.Status
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
            // TODO
            var userCode = request.EquipmentCode; //_currentEquipment.Code
            var nowTime = HymsonClock.Now();

            await UpdateEquipmentStatusAsync(new EquStatusEntity
            {
                Id = IdGenProvider.Instance.CreateId(),
                SiteId = _currentEquipment.SiteId,
                CreatedBy = userCode,
                CreatedOn = nowTime,
                UpdatedBy = userCode,
                UpdatedOn = nowTime,
                EquipmentId = _currentEquipment.Id ?? 0,
                LocalTime = request.LocalTime,
                EquipmentStatus = request.StateCode
            });
        }

        /// <summary>
        /// 设备报警
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task EquipmentAlarmAsync(EquipmentAlarmRequest request)
        {
            // TODO
            var userCode = request.EquipmentCode; //_currentEquipment.Code
            var nowTime = HymsonClock.Now();

            await _equipmentAlarmRepository.InsertAsync(new EquAlarmEntity
            {
                Id = IdGenProvider.Instance.CreateId(),
                SiteId = _currentEquipment.SiteId,
                CreatedBy = userCode,
                CreatedOn = nowTime,
                UpdatedBy = userCode,
                UpdatedOn = nowTime,
                EquipmentId = _currentEquipment.Id ?? 0,
                LocalTime = request.LocalTime,
                FaultCode = request.AlarmCode,
                AlarmMsg = request.AlarmMsg ?? "",
                Status = request.Status
            });
        }

        /// <summary>
        /// 设备停机原因
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task EquipmentDownReasonAsync(EquipmentDownReasonRequest request)
        {
            // TODO
            var userCode = request.EquipmentCode; //_currentEquipment.Code
            var nowTime = HymsonClock.Now();

            await UpdateEquipmentStatusAsync(new EquStatusEntity
            {
                Id = IdGenProvider.Instance.CreateId(),
                SiteId = _currentEquipment.SiteId,
                CreatedBy = userCode,
                CreatedOn = nowTime,
                UpdatedBy = userCode,
                UpdatedOn = nowTime,
                EquipmentId = _currentEquipment.Id ?? 0,
                LocalTime = request.LocalTime,
                EquipmentStatus = EquipmentStateEnum.DownNormal, // 暂定为正常停机
                LossRemark = request.DownReasonCode.GetDescription(),
                BeginTime = request.BeginTime,
                EndTime = request.EndTime
            });
        }




        /// <summary>
        /// 更新设备状态
        /// </summary>
        /// <param name="currentStatusEntity"></param>
        /// <returns></returns>
        private async Task UpdateEquipmentStatusAsync(EquStatusEntity currentStatusEntity)
        {
            // 最近的状态记录
            var lastStatusEntity = await _equipmentStatusRepository.GetLastEntityByEquipmentIdAsync(currentStatusEntity.EquipmentId);

            // 开启事务
            using var trans = TransactionHelper.GetTransactionScope();

            // 更新设备当前状态
            await _equipmentStatusRepository.InsertAsync(currentStatusEntity);

            // 更新统计表
            if (lastStatusEntity != null && lastStatusEntity.EquipmentStatus != lastStatusEntity.EquipmentStatus)
            {
                await _equipmentStatusRepository.InsertStatisticsAsync(new EquStatusStatisticsEntity
                {
                    Id = IdGenProvider.Instance.CreateId(),
                    SiteId = currentStatusEntity.SiteId,
                    CreatedBy = currentStatusEntity.CreatedBy,
                    CreatedOn = currentStatusEntity.CreatedOn,
                    UpdatedBy = currentStatusEntity.UpdatedBy,
                    UpdatedOn = currentStatusEntity.UpdatedOn,
                    EquipmentId = currentStatusEntity.EquipmentId,
                    EquipmentStatus = lastStatusEntity.EquipmentStatus,
                    SwitchEquipmentStatus = currentStatusEntity.EquipmentStatus,
                    BeginTime = lastStatusEntity.LocalTime,
                    EndTime = currentStatusEntity.LocalTime
                });
            }

            trans.Complete();
        }

    }
}
