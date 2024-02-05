using System.ComponentModel;

namespace Hymson.MES.Core.Enums.QualUnqualifiedCode
{
    /// <summary>
    /// 不合格代码类型
    /// </summary>
    public enum QualUnqualifiedCodeTypeEnum : short
    {
        /// <summary>
        /// 缺陷
        /// </summary>
        [Description("缺陷")]
        Defect = 0,

        /// <summary>
        /// 标识
        /// </summary>
        [Description("标识")]
        Mark = 1
    }
}
