using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Core.Enums.Integrated
{
    public enum InteJobBusinessTypeEnum
    {
        /// <summary>
        /// 1资源
        /// </summary>
        [Description("资源")]
        Resource = 1,
        /// <summary>
        /// 2工序
        /// </summary>
        [Description("工序")]
        Procedure = 2,
        /// <summary>
        /// 3不合格代码
        /// </summary>
        [Description("不合格代码")]
        Unqualified = 3,
        /// <summary>
        /// 4子步骤
        /// </summary>
        [Description("子步骤")]
        Substep = 4
    }
}
