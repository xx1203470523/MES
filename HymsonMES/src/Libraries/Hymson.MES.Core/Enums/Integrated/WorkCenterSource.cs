using System;
using System.Collections.Generic;
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
        ERP = 0,

        /// <summary>
        /// 产线
        /// </summary>
        MES = 1,
    }
}
