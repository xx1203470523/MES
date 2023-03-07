using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Core.Enums.Integrated
{
    public enum WorkCenterSource:short
    {
        /// <summary>
        /// 工厂
        /// </summary>
        [Description("工厂")]
        ERP = 0,

        /// <summary>
        /// 产线
        /// </summary>
        [Description("产线")]
        MES = 1,
    }
}
