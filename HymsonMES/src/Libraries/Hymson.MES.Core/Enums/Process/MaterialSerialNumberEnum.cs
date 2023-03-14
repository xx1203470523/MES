using System.ComponentModel;

namespace Hymson.MES.Core.Enums
{
    /// <summary>
    /// 物料数据收集方式枚举
    /// </summary>
    public enum MaterialSerialNumberEnum : sbyte
    {
        /// <summary>
        /// 内部
        /// </summary>
        [Description("内部")]
        Inside = 1,
        /// <summary>
        /// 外部
        /// </summary>
        [Description("外部")]
        Outside = 2,
        /// <summary>
        /// 批次
        /// </summary>
        [Description("批次")]
        Batch = 3
    }
}
