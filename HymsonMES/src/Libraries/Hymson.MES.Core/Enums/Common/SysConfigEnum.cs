using System.ComponentModel;

namespace Hymson.MES.Core.Enums
{
    /// <summary>
    /// 枚举（系统配置）
    /// </summary>
    public enum SysConfigEnum : sbyte
    {
        /// <summary>
        /// AQL检验水平
        /// </summary>
        [Description("AQLLevel")]
        AQLLevel = 1,
        /// <summary>
        /// AQL检验计划
        /// </summary>
        [Description("AQLPlan")]
        AQLPlan = 2,
        /// <summary>
        /// 请求站点（默认值）
        /// </summary>
        [Description("MainSite")]
        MainSite = 3,
        /// <summary>
        /// 转子配置
        /// </summary>
        [Description("Rotor")]
        Rotor = 4,
        /// <summary>
        /// 定子配置
        /// </summary>
        [Description("Stator")]
        Stator = 5,
    }
}
