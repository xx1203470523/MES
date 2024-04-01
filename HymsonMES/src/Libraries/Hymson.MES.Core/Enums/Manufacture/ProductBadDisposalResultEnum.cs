using System.ComponentModel;

namespace Hymson.MES.Core.Enums.Manufacture
{
    /// <summary>
    /// 处置结果
    /// </summary>
    public enum ProductBadDisposalResultEnum
    {
        /// <summary>
        /// 复判关闭
        /// </summary>
        [Description("复判关闭")]
        ReJudgmentClosed = 1,

        /// <summary>
        /// 复判关闭
        /// </summary>
        [Description("复判维修")]
        ReJudgmentRepair = 2,

        /// <summary>
        /// 取消标识
        /// </summary>
        [Description("取消标识")]
        RemoveIdentify = 3,

        /// <summary>
        /// 报废
        /// </summary>
        [Description("报废")]
        Scrap = 4,

        /// <summary>
        /// 自动处理
        /// </summary>
        [Description("自动处理")]
        AutoHandle = 5,

        /// <summary>
        /// 维修
        /// </summary>
        [Description("维修")]
        Repair = 6,

        /// <summary>
        /// 等待判定
        /// </summary>
        [Description("等待判定")]
        WaitingJudge = 7,

        /// <summary>
        /// 设备误判
        /// </summary>
        [Description("设备误判")]
        Misjudgment = 8

    }
}
