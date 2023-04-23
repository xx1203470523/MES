using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Core.Enums.Manufacture
{
    /// <summary>
    /// 操作类型;1、装载2、移除
    /// </summary>
    public enum ManuContainerPackRecordOperateTypeEnum
    {
        /// 装载
        /// </summary>
        [Description("装载")]
        Load = 1,
        /// <summary>
        /// 移除
        /// </summary>
        [Description("移除")]
        Remove = 2
    }
}
