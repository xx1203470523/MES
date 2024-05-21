using System.ComponentModel;

namespace Hymson.MES.Core.Enums.Quality
{
    public enum QualMaterialUnqualifiedStatusEnum : sbyte
    {
        /// <summary>
        /// 打开
        /// </summary>
        [Description("打开")]
        Open = 1,
        /// <summary>
        /// 关闭
        /// </summary>
        [Description("关闭")]
        Close = 2
    }
}
