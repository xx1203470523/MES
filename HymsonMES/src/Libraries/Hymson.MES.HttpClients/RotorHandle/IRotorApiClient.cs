using Hymson.MES.Core.Domain.Process;
using Hymson.MES.HttpClients.Requests;
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
        Task<bool> WorkOrderSync(RotorWorkOrderSync rotorWorkOrder);
        /// <summary>
        /// 工单激活
        /// </summary>
        /// <param name="workOrderCode"></param>
        /// <returns></returns>
        Task<bool> WorkOrderStart(string workOrderCode);
        /// <summary>
        /// 工单取消激活
        /// </summary>
        /// <param name="workOrderCode"></param>
        /// <returns></returns>
        Task<bool> WorkOrderStop(string workOrderCode);
        /// <summary>
        /// 物料同步
        /// </summary>
        /// <param name="materialEntities"></param>
        /// <returns></returns>
        Task<bool> MaterialSync(IEnumerable<ProcMaterialEntity> materialEntities);
        /// <summary>
        /// 工序同步
        /// </summary>
        /// <param name="procedureEntities"></param>
        /// <returns></returns>
        Task<bool> ProcedureSync(IEnumerable<ProcProcedureEntity> procedureEntities);
        /// <summary>
        /// 工艺路线同步
        /// </summary>
        /// <param name="procProcessRouteDetails"></param>
        /// <returns></returns>
        Task<bool> ProcedureLineSync(IEnumerable<ProcProcessRouteDetailLinkEntity> procProcessRouteDetails);  
    }
}
