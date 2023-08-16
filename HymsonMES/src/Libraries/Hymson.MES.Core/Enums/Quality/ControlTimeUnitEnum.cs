using System.ComponentModel;

namespace Hymson.MES.Core.Enums.Quality
{
    /// <summary>
    /// 管控时间单位
    /// </summary>
    public enum ControlTimeUnitEnum : sbyte
    {
        /// <summary>
        /// 小时
        /// </summary>
        [Description("小时")]
        Hour = 1,
        /// <summary>
        /// 分钟
        /// </summary>
        [Description("分钟")]
        Minute = 2,
    }
}
