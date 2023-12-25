using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Core.Enums.Integrated
{
    /// <summary>
    /// 发布状态
    /// </summary>
    public enum SysReleaseRecordStatusEnum : short
    {
        /// <summary>
        /// 预留
        /// </summary>
        [Description("预留")]
        reserve = 1,

        /// <summary>
        /// 发布 
        /// </summary>
        [Description("发布")]
        release = 2,
    }
}
