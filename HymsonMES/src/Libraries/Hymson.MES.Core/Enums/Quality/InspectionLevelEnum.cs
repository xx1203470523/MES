using System.ComponentModel;

namespace Hymson.MES.Core.Enums.Quality
{
    /// <summary>
    /// 检验水平类型
    /// </summary>
    public enum InspectionLevelEnum : sbyte
    {
        /// <summary>
        /// I
        /// </summary>
        [Description("I")]
        I = 1,
        /// <summary>
        /// II
        /// </summary>
        [Description("II")]
        II = 2,
        /// <summary>
        /// III
        /// </summary>
        [Description("III")]
        III = 3,
        /// <summary>
        /// IV
        /// </summary>
        [Description("IV")]
        IV = 4,
        /// <summary>
        /// V
        /// </summary>
        [Description("V")]
        V = 5,
        /// <summary>
        /// VI
        /// </summary>
        [Description("VI")]
        VI = 6,
        /// <summary>
        /// VII
        /// </summary>
        [Description("VII")]
        VII = 7
    }

}
