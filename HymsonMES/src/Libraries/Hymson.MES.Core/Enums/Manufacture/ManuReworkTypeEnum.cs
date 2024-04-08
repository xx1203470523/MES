using System.ComponentModel;

namespace Hymson.MES.Core.Enums.Manufacture
{
    /// <summary>
    /// 返工类型
    /// </summary>
    public enum ManuReworkTypeEnum : sbyte
    {
        /// <summary>
        /// 原工单返工
        /// </summary>
        [Description("原工单返工")]
        OriginalOrder = 1,
        /// <summary>
        /// 新工单返工
        /// </summary>
        [Description("新工单返工")]
        NewOrder = 2,
        /// <summary>
        /// 新工单返工（成品电芯）
        /// </summary>
        [Description("新工单返工（成品电芯）")]
        NewOrderCell = 3
    }
}
