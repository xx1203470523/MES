﻿using Hymson.MES.EquipmentServices.Request.Equipment;
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
        /// 
        /// </summary>
        /// <param name="validationInBoundMoreRequestRules"></param>
        /// <param name="currentEquipment"></param>
        public EquipmentService(ICurrentEquipment currentEquipment)
        {
            _currentEquipment = currentEquipment;
        }

        /// <summary>
        /// 设备心跳
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task EquipmentHeartbeatAsync(EquipmentHeartbeatRequest request)
        {
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
