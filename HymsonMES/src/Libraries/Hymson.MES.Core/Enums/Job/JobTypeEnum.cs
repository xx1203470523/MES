using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Core.Enums.Job
{
    public enum JobTypeEnum : sbyte
    {
        /// <summary>
        /// 标准  组装
        /// </summary>
        [Description("标准")]
        Standard = 1,
    }
}
