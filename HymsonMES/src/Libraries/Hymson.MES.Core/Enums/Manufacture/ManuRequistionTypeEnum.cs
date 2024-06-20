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
        PICKING = 1,
        /// <summary>
        /// REPLENISHMENT
        /// </summary>
        [Description("工单补料")]
        REPLENISHMENT = 2
    }
}
