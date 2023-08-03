using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Core.Enums.Manufacture
{
    public enum BakingStatusEnum
    {
        /// <summary>
        /// 烘烤中
        /// </summary>
        [Description("烘烤中")]
        Working = 0,
        /// <summary>
        /// 烘烤结束
        /// </summary>
        [Description("烘烤结束")]
        TheEnd  =1,
        /// <summary>
        /// 烘烤结束
        /// </summary>
        [Description("烘烤终止")]
        Aborted = 2,
    }
}
