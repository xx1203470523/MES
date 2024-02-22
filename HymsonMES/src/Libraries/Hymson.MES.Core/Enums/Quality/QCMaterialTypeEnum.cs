using System.ComponentModel;

namespace Hymson.MES.Core.Enums.Quality
{
    /// <summary>
    /// QC物料类型
    /// </summary>
    public enum QCMaterialTypeEnum : sbyte
    {
        /// <summary>
        /// 通用
        /// </summary>
        [Description("通用")]
        General = 1,
        /// <summary>
        /// 物料
        /// </summary>
        [Description("物料")]
        Material = 2
    }
}
