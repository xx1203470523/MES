using System.ComponentModel;

namespace Hymson.MES.Core.Enums.Quality
{
    public enum QualMaterialDisposalResultEnum : sbyte
    {
        /// <summary>
        /// 放行
        /// </summary>
        [Description("放行")]
        Release= 1,
        /// <summary>
        /// 退料
        /// </summary>
        [Description("退料")]
        Compromis = 2
    }
}
