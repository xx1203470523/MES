using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Core.Enums.Quality
{
    /// <summary>
    /// 品检状态
    /// </summary>
    public enum ProductReceiptQualifiedStatusEnum : sbyte
    {
        /// <summary>
        /// 合格
        /// </summary>
        [Description("合格")]
        Qualified = 1,
        /// <summary>
        /// 不合格
        /// </summary>
        [Description("不合格")]
        Unqualified = 0,
        /// <summary>
        /// 待检验
        /// </summary>
        [Description("待检验")]
        ToBeBnspected = 2,
    }
}
