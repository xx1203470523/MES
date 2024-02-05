using System.ComponentModel;

namespace Hymson.MES.Core.Enums.Quality
{
    /// <summary>
    /// 不合格处理方式
    /// </summary>
    public enum HandMethodEnum : sbyte
    {
        /// <summary>
        /// 让步
        /// </summary>
        [Description("让步")]
        Concession = 1,
        /// <summary>
        /// 不合格
        /// </summary>
        [Description("不合格")]
        Unqualified = 2
    }
}
