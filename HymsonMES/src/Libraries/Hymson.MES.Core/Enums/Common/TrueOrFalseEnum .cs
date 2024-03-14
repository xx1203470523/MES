using System.ComponentModel;

namespace Hymson.MES.Core.Enums
{
    /// <summary>
    /// 
    /// </summary>
    public enum TrueOrFalseEnum : sbyte
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
    }
}
