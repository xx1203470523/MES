using System.ComponentModel;

namespace Hymson.MES.Core.Enums.Equipment
{
    /// <summary>
    /// 点检任务状态
    /// </summary>
    public enum EquSpotcheckTaskStautusEnum : sbyte
    {
        /// <summary>
        /// 待检验
        /// </summary>
        [Description("待处理")]
        WaitInspect = 1,
        /// <summary>
        /// 检验中
        /// </summary>
        [Description("处理中")]
        Inspecting = 2,
        /// <summary>
        /// 待审核（已检验）
        /// </summary>
        [Description("待审核")]
        Completed = 3,
        /// <summary>
        /// 已关闭
        /// </summary>
        [Description("已关闭")]
        Closed = 4
    }
}
