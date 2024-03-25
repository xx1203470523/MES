using System.ComponentModel;

namespace Hymson.MES.Core.Enums.Quality
{
    /// <summary>
    /// 检验单状态
    /// </summary>
    public enum InspectionStatusEnum : sbyte
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
        /// 已检验(待审核)
        /// </summary>
        [Description("待审核")]
        Completed = 3,
        /// <summary>
        /// 已关闭
        /// </summary>
        [Description("已关闭")]
        Closed = 4
    }

    /// <summary>
    /// 检验单状态（页面显示用）
    /// </summary>
    public enum InspectionStatusForPageEnum : sbyte
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
