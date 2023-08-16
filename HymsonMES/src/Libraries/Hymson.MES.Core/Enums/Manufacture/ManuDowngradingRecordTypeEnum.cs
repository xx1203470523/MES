using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Core.Enums.Manufacture
{
    public enum ManuDowngradingRecordTypeEnum
    {
        /// <summary>
        /// 降级录入
        /// </summary>
        [Description("降级录入")]
        Entry= 0,
        /// <summary>
        /// 降级移除
        /// </summary>
        [Description("降级移除")]
        Remove  =1,
    }
}
