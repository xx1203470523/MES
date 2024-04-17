using System.ComponentModel;

namespace Hymson.MES.Core.Enums
{
    /// <summary>
    /// 点检任务
    /// </summary>
    public enum EquInspectionRecordStatusEnum : sbyte
    {
        /// <summary>
        /// 待检验
        /// </summary>
        [Description("待检验")]
        WaitInspect = 1,

        /// <summary>
        /// 检验中
        /// </summary>
        [Description("检验中")]
        Inspecting = 2,

        /// <summary>
        /// 已完成
        /// </summary>
        [Description("已完成")]
        Completed = 3,
    }
}
