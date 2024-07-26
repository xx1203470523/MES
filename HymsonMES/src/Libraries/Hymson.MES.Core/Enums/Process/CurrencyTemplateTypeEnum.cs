using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Core.Enums.Process
{
    /// <summary>
    /// 模板通用类型
    /// </summary>
    public enum CurrencyTemplateTypeEnum
    {
        /// <summary>
        /// 原材料通用
        /// </summary>
        [Description("原材料通用")]
        Material = 1,
        /// <summary>
        /// 生产通用
        /// </summary>
        [Description("生产通用")]
        Production = 2
    }
}
