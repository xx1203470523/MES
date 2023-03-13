using System.ComponentModel;

namespace Hymson.MES.Core.Enums.Integrated
{
    /// <summary>
    /// 匹配方式枚举
    /// </summary>
    public enum MatchModeEnum : sbyte
    {
        /// <summary>
        /// 全码
        /// </summary>
        [Description("全码")] 
        Whole = 0,
        /// <summary>
        /// 起始
        /// </summary>
        [Description("起始")]
        Start = 1,
        /// <summary>
        /// 中间
        /// </summary>
        [Description("中间")]
        Middle = 2,
        /// <summary>
        /// 结束
        /// </summary>
        [Description("结束")] 
        End = 3
    }

}
