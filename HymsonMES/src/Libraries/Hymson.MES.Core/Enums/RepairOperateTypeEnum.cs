using System.ComponentModel;

namespace Hymson.MES.Core.Enums
{
    public enum RepairOperateTypeEnum : sbyte
    {
        /// <summary>
        /// 维修NG OK确认
        /// </summary>
        [Description("OK确认")]
        OK = 1,
    }

    public enum RepairOutTypeEnum
    {
        /// <summary>
        /// 维修NG OK确认
        /// </summary>
        [Description("NG")]
        NG = 0,
        [Description("OK")]
        OK = 1,
    }
}
