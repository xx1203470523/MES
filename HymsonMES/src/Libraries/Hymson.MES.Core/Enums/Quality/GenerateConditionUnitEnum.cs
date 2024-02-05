using System.ComponentModel;

namespace Hymson.MES.Core.Enums.Quality
{
    /// <summary>
    /// 生成条件单位
    /// </summary>
    public enum GenerateConditionUnitEnum : sbyte
    {
        /// <summary>
        /// 小时
        /// </summary>
        [Description("小时")]
        Hour = 1,
        /// <summary>
        /// 班次
        /// </summary>
        [Description("班次")]
        Shift = 2,
        /// <summary>
        /// 批次
        /// </summary>
        [Description("批次")]
        Batch = 3,
        /// <summary>
        /// 罐
        /// </summary>
        [Description("罐")]
        Pot = 4,
        /// <summary>
        /// 卷
        /// </summary>
        [Description("卷")]
        Volume = 5
    }

    /// <summary>
    /// 生成条件单位-首检
    /// </summary>
    public enum GenerateConditionUnitFaiEnum : sbyte
    {
        /// <summary>
        /// 班次
        /// </summary>
        [Description("班次")]
        Shift = 2
    }

    /// <summary>
    /// 生成条件单位-巡检
    /// </summary>
    public enum GenerateConditionUnitIpqcEnum : sbyte
    {
        /// <summary>
        /// 小时
        /// </summary>
        [Description("小时")]
        Hour = 1,
        /// <summary>
        /// 批次
        /// </summary>
        [Description("批次")]
        Batch = 3,
        /// <summary>
        /// 罐
        /// </summary>
        [Description("罐")]
        Pot = 4,
        /// <summary>
        /// 卷
        /// </summary>
        [Description("卷")]
        Volume = 5
    }

    /// <summary>
    /// 生成条件单位-尾检
    /// </summary>
    public enum GenerateConditionUnitQtiEnum : sbyte
    {
        /// <summary>
        /// 班次
        /// </summary>
        [Description("班次")]
        Shift = 2,
        /// <summary>
        /// 批次
        /// </summary>
        [Description("批次")]
        Batch = 3,
        /// <summary>
        /// 罐
        /// </summary>
        [Description("罐")]
        Pot = 4,
        /// <summary>
        /// 卷
        /// </summary>
        [Description("卷")]
        Volume = 5
    }
}
