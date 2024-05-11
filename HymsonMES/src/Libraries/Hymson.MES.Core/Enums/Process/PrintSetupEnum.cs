using System.ComponentModel;

namespace Hymson.MES.Core.Enums
{
    /// <summary>
    /// 配置类型枚举
    /// </summary>
    public enum PrintSetupEnum : sbyte
    {
        /// <summary>
        /// 资源
        /// </summary>
        [Description("资源")]
        Resource = 1,
        /// <summary>
        /// 类
        /// </summary>
        [Description("类")]
        Class = 2
    }
}
