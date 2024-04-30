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
        ParameterCollect = 3,

        /// <summary>
        /// 分选规则
        /// </summary>
        [Description("分选规则")]
        ProcSortingRule = 4,

        /// <summary>
        /// ESOP获取
        /// </summary>
        [Description("ESOP获取")]
        ESOPGet = 5,

        /// <summary>
        /// 条码请求
        /// </summary>
        [Description("条码请求")]
        SFCRequest = 6,

        /// <summary>
        /// 产出确认
        /// </summary>
        [Description("产出确认")]
        OutPutconConfirm = 7,
        /// <summary>
        /// 物料加载
        /// </summary>
        [Description("产出确认")]
        Feeding = 8
    }
}
