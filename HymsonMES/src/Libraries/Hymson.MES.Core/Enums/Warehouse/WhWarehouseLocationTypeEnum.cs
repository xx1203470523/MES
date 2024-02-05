using System.ComponentModel;

namespace Hymson.MES.Core.Enums
{
    /// <summary>
    /// 库位编码生成类型
    /// </summary>
    public enum WhWarehouseLocationTypeEnum:byte
    {
        /// <summary>
        /// 自动生成
        /// </summary>
        [Description("自动生成")]
        Automatically = 1,

        /// <summary>
        /// 自定义
        /// </summary>
        [Description("自定义生成")]
        Customize = 2,

        /// <summary>
        /// 指定行生成
        /// </summary>
        [Description("指定行生成")]
        SpecifyRow = 3
    }
}
