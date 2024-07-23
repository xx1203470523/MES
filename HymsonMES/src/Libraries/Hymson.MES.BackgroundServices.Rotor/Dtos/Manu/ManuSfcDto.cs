using Hymson.MES.Core.Domain.Plan;
using Hymson.MES.Data.Repositories.Manufacture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.BackgroundServices.Rotor.Dtos.Manu
{
    /// <summary>
    /// 
    /// </summary>
    public class ManuSfcDto : ManuSfcUpdateStatusCommand
    {
        /// <summary>
        /// 工单
        /// </summary>
        public PlanWorkOrderEntity? WorkOrder { get; set; }

        /// <summary>
        /// 物料编码，上料物料时，物料编码取这个
        /// </summary>
        public string MaterialCode { get; set; }
    }
}
