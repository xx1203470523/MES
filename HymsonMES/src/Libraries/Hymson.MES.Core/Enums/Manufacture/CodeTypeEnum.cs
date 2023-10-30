using System.ComponentModel;

namespace Hymson.MES.Core.Enums
{
    /// <summary>
    /// 条码类型
    /// </summary>
    public enum CodeTypeEnum : sbyte
    {
        /// <summary>
        /// 生产条码
        /// </summary>
        [Description("生产条码")]
        SFC = 1,

        /// <summary>
        /// 载具编码
        /// </summary>
        [Description("载具编码")]
        Vehicle = 2
    }
}
