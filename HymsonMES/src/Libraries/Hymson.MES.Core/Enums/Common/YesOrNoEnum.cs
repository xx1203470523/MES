using System.ComponentModel;

namespace Hymson.MES.Core.Enums
{
    /// <summary>
    /// 1是/0否
    /// </summary>
    public enum YesOrNoEnum : sbyte
    {
        /// <summary>
        /// 是
        /// </summary>
        [Description("是")]
        Yes = 1,
        /// <summary>
        /// 否 
        /// </summary>
        [Description("否")]
        No = 2,
    }
}
