using System.ComponentModel;

namespace Hymson.MES.Core.Enums.Integrated
{
    /// <summary>
    /// 定义方式枚举
    /// </summary>
    public enum DefinitionMethodEnum : sbyte
    {
        /// <summary>
        /// 物料
        /// </summary>
        [Description("物料")]
        Material = 1,
        /// <summary>
        /// 物料组
        /// </summary>
        [Description("物料组")]
        MaterialGroup = 2
    }

}
