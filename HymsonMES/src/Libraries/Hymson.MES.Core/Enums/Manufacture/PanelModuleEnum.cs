using System.ComponentModel;

namespace Hymson.MES.Core.Enums
{
    /// <summary>
    /// 面板组件类型
    /// </summary>
    public enum PanelModuleEnum : sbyte
    {
        /// <summary>
        /// 组装
        /// </summary>
        [Description("组装")]
        Package = 1,

        /// <summary>
        /// 不良录入
        /// </summary>
        [Description("不良录入")]
        BadRecord = 2,

        /// <summary>
        /// 参数收集
        /// </summary>
        [Description("参数收集")]
        ParameterCollect = 3
    }
}
