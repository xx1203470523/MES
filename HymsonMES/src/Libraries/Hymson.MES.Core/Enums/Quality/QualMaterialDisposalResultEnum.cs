using System.ComponentModel;

namespace Hymson.MES.Core.Enums.Quality
{
    public enum QualMaterialDisposalResultEnum : sbyte
    {
        /// <summary>
        /// 打开
        /// </summary>
        [Description("放行")]
        Open = 1,
        /// <summary>
        /// 关闭
        /// </summary>
        [Description("退料")]
        Close = 2
    }
}
