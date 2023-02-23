using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Core.Enums.Integrated
{
    /// <summary>
    /// 工作中心类型
    /// </summary>
    public enum WorkCenterType : short
    {
        /// <summary>
        /// 工厂
        /// </summary>
        Factory = 0,

        /// <summary>
        /// 产线
        /// </summary>
        Line = 1,

        /// <summary>
        /// 车间
        /// </summary>
        Farm = 2
    }
}
