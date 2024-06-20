using System;
using System.ComponentModel;

namespace Hymson.MES.Core.Enums
{
    /// <summary>
    /// 工具类型
    /// </summary>
    public enum ToolingTypeEnum : sbyte
    {
        ///// <summary>
        ///// 分钟
        ///// </summary>
        //[Description("分钟")]
        //Minute = 1,
        /// <summary>
        /// 小时
        /// </summary>
        [Description("小时")]
        Hour = 2,
        /// <summary>
        /// 日
        /// </summary>
        [Description("天")]
        Day = 3,
        /// <summary>
        /// 周
        /// </summary>
        [Description("周")]
        Week = 4,
        /// <summary>
        /// 月
        /// </summary>
        [Description("月")]
        Month = 5
    }
}
