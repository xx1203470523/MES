using System.ComponentModel;

namespace Hymson.MES.Core.Enums.Integrated
{
    /// <summary>
    /// 自定义字段语言类型枚举
    /// </summary>
    public enum InteCustomFieldLanguageTypeEnum : sbyte
    {
        /// <summary>
        /// 中文
        /// </summary>
        [Description("中文")]
        ZH_CN = 1,

        /// <summary>
        /// English
        /// </summary>
        [Description("English")]
        EN = 2
    }
    
}
