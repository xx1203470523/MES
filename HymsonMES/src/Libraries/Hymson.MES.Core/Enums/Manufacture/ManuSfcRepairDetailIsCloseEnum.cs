using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Core.Enums.Manufacture
{
    /// <summary>
    /// manu_product_bad_record 产品不良录入表Status
    /// </summary>
    public enum ProductBadRecordStatusEnum : sbyte
    {
        /// <summary>
        /// 开启
        /// </summary>
        [Description("开启")]
        Open = 1,
        /// <summary>
        /// 关闭
        /// </summary>
        [Description("关闭")]
        Close = 2,
    }
}
