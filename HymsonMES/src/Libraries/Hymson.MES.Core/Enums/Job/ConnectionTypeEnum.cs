using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Core.Enums.Job
{
    /// <summary>
    /// 
    /// </summary>
    public enum ConnectionTypeEnum : sbyte
    {
        /// <summary>
        /// 资源工序
        /// </summary>
        [Description("资源工序")]
        procedureAndResource = 1,
    }
}
