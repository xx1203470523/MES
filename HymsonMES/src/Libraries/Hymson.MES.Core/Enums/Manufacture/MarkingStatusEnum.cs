using System.ComponentModel;

namespace Hymson.MES.Core.Enums.Manufacture
{
    /// <summary>
    /// Marking状态枚举
    /// </summary>
    public enum MarkingStatusEnum : sbyte
    {
        /// <summary>
        /// 关闭
        /// </summary>
        [Description("关闭")]
        Close = 0,
        /// <summary>
        /// 开启
        /// </summary>
        [Description("开启")]
        Open = 1
    }

    /// <summary>
    /// Marking类型枚举
    /// </summary>
    public enum MarkingTypeEnum : sbyte
    {
        /// <summary>
        /// 拦截
        /// </summary>
        [Description("拦截")]
        Intercept = 1,
        /// <summary>
        /// 标记
        /// </summary>
        [Description("标记")]
        Mark = 2
    }

    /// <summary>
    /// Marking来源枚举
    /// </summary>
    public enum MarkingSourceTypeEnum : sbyte
    {
        /// <summary>
        /// 直接录入
        /// </summary>
        [Description("直接录入")]
        Directly = 1,
        /// <summary>
        /// 继承
        /// </summary>
        [Description("继承")]
        Inherited = 2
    }
}
