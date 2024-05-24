using Hymson.MES.Core.Domain.Plan;
using Hymson.MES.Data.Repositories.Equipment.EquEquipment.View;
using Hymson.MES.Data.Repositories.Process;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.EquipmentServices.Services.Qkny.PlanWorkOrder
{
    /// <summary>
    /// 工单信息表 service接口
    /// </summary>
    public interface IPlanWorkOrderService
    {
        /// <summary>
        /// 根据产线ID、资源ID获取工单数据（激活的工单）
        /// </summary>
        /// <param name="equResModel"></param>
        /// <returns></returns>
        Task<PlanWorkOrderEntity> GetByWorkLineIdAsync(EquEquipmentResAllView equResModel);

        /// <summary>
        /// 获取工单对应物料
        /// </summary>
        /// <param name="bomId"></param>
        /// <returns></returns>
        Task<List<long>> GetWorkOrderMaterialAsync(long bomId);
    }
}
