using Hymson.MES.Data.Repositories.Common.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Data.Repositories.Integrated.InteVehicleFreight.Command
{
    /// <summary>
    /// 条码明细更新
    /// </summary>
    public class UpdateVehicleFreightStackCommand : UpdateCommand
    {
        /// <summary>
        /// 托盘id
        /// </summary>
        public long VehicleId { get; set; }

        /// <summary>
        /// 条码
        /// </summary>
        public string BarCode { get; set; }
    }
}
