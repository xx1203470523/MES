using System.ComponentModel;

namespace Hymson.MES.Core.Enums.Common
{
    public enum TrueFalseEmptyEnum : sbyte
    {
        /// <summary>
        /// 是1
        /// </summary>
        [Description("是")]
        Yes = 1,
        /// <summary>
        /// 否
        /// </summary>
        [Description("否")]
        No = 0,
        /// <summary>
        /// 无
        /// </summary>
        [Description("无")]
        Empty = 2,
    }
}
