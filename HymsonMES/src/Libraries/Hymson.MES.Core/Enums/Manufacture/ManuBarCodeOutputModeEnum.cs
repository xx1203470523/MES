using System.ComponentModel;

namespace Hymson.MES.Core.Enums.Manufacture
{
    /// <summary>
    /// 条码关系类型
    /// </summary>
    public enum ManuBarCodeOutputModeEnum : sbyte
    {
        /// <summary>
        /// 分组（无条码时采用分组模式）
        /// </summary>
        [Description("分组")]
        Packet = 1,
        /// <summary>
        /// 正常
        /// </summary>
        [Description("正常")]
        Normal = 2
    }
}
