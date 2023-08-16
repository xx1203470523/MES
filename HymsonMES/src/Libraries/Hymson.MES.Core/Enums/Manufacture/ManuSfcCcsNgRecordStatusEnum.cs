using System.ComponentModel;

namespace Hymson.MES.Core.Enums.Manufacture
{
    /// <summary>
    /// 模组CCSNG记录状态枚举
    /// </summary>
    public enum ManuSfcCcsNgRecordStatusEnum
    {
        /// <summary>
        /// NG
        /// </summary>
        [Description("NG")]
        NG = 0,
        /// <summary>
        /// 修复或重新生产
        /// </summary>
        [Description("Confirm")]
        Confirm = 1
    }
}
