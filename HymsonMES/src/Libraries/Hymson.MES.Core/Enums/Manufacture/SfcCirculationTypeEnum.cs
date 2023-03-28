using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Core.Enums.Manufacture
{
    /// <summary>
    /// ManuSfcCirculation类型枚举
    /// </summary>
    public enum SfcCirculationTypeEnum : sbyte
    {
        /// <summary>
        /// 拆分
        /// </summary>
        [Description("拆分")]
        Split = 1,
        /// <summary>
        /// 合并
        /// </summary>
        [Description("合并")]
        Merge = 2,
        /// <summary>
        /// 转换
        /// </summary>
        [Description("转换")]
        Change = 3,
        /// <summary>
        /// 消耗
        /// </summary>
        [Description("消耗")]
        Consume = 4
    }
}
