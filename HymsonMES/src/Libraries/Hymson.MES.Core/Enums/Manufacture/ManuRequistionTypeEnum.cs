using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Core.Domain.Manufacture
{
    //领料类型
    public enum ManuRequistionTypeEnum : sbyte
    {
        /// <summary>
        /// PICKING
        /// </summary>
        [Description("工单领料")]
        WorkOrderPicking = 0,
        /// <summary>
        /// 工单补料
        /// </summary>
        [Description("工单补料")]
        WorkOrderReplenishment = 1
    }
    public enum ManuReturnTypeEnum : sbyte
    {
        /// <summary>
        /// PICKING
        /// </summary>
        [Description("工单退料")]
        WorkOrderReturn = 0,
        /// <summary>
        /// 工单补料
        /// </summary>
        [Description("工单借料")]
        WorkOrderBorrow = 1
    }
}
