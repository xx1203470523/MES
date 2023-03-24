using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Core.Enums.Manufacture
{
    /// <summary>
    /// 质量录入报废操作类型
    /// </summary>
    public enum ScrapOperateTypeEnum : sbyte
    {
        /// <summary>
        /// 报废
        /// </summary>
        [Description("报废")]
        Scrapping = 1,
        /// <summary>
        /// 取消报废
        /// </summary>
        [Description("取消报废")]
        CancelScrap = 2,
    }
}
