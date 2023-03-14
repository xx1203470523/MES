using System.ComponentModel;

namespace Hymson.MES.Core.Enums.Integrated
{
    /// <summary>
    /// 班制类型
    /// </summary>
    public enum ClassTypeEnum : sbyte
    {
        /// <summary>
        /// 工作
        /// </summary>
        [Description("工作")]
        Work = 1,
        /// <summary>
        /// 休息
        /// </summary>
        [Description("休息")]
        Rest = 2
    }
}
