using Hymson.MES.HttpClients.Requests;
using Hymson.MES.HttpClients.Requests.Rotor;
using Hymson.MES.HttpClients.Responses.Rotor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.HttpClients.RotorHandle
{
    /// <summary>
    /// 转子线服务
    /// </summary>
    public interface IRotorApiClient
    {
        /// <summary>
        /// 下发派工单
        /// </summary>
        /// <param name="rotorWorkOrder"></param>
        /// <returns></returns>
        Task<RotorResponse> WorkOrderAsync(RotorWorkOrderRequest rotorWorkOrder);

        /// <summary>
        /// 工单激活
        /// </summary>
        /// <param name="workOrderCode"></param>
        /// <returns></returns>
        Task<bool> WorkOrderStartAsync(string workOrderCode);

        /// <summary>
        /// 工单取消激活
        /// </summary>
        /// <param name="workOrderCode"></param>
        /// <returns></returns>
        Task<bool> WorkOrderStopAsync(string workOrderCode);

        /// <summary>
        /// 物料同步
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        Task<RotorResponse> MaterialAsync(IEnumerable<RotorMaterialRequest> list);

        ///// <summary>
        ///// 工序同步
        ///// </summary>
        ///// <param name="list"></param>
        ///// <returns></returns>
        //Task<int> ProcedureAsync(IEnumerable<ProcProcedureEntity> list);

        ///// <summary>
        ///// 工艺路线同步
        ///// </summary>
        ///// <param name="procProcessRouteDetails"></param>
        ///// <returns></returns>
        //Task<bool> ProcedureLineSync(IEnumerable<ProcProcessRouteDetailLinkEntity> procProcessRouteDetails);  
    }
}
