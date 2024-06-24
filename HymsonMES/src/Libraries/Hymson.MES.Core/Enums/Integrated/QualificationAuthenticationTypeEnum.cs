using System.ComponentModel;

namespace Hymson.MES.Core.Enums.Integrated
{
    /// <summary>
    /// 日历类型枚举
    /// </summary>
    public enum QualificationAuthenticationTypeEnum : sbyte
    {
        /// <summary>
        /// 人员
        /// </summary>
        [Description("人员")]
        User = 1,
        /// <summary>
        /// 角色
        /// </summary>
        [Description("角色")]
        Role = 2
    }
    
}
