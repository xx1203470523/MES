using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Core.Enums.Integrated
{
    /// <summary>
    /// 字段定义类型
    /// </summary>
    public enum FieldDefinitionTypeEnum : sbyte
    {
        /// <summary>
        /// 文本
        /// </summary>
        [Description("文本")]
        Text = 1,
        /// <summary>
        /// 文本区域
        /// </summary>
        [Description("文本区域")]
        TextArea = 2,
        /// <summary>
        /// 日期
        /// </summary>
        [Description("日期")]
        Date = 3,
        /// <summary>
        /// 数字
        /// </summary>
        [Description("数字")]
        Number = 4,
        /// <summary>
        /// 复选框
        /// </summary>
        [Description("复选框")]
        Checkbox = 5
    }
}
