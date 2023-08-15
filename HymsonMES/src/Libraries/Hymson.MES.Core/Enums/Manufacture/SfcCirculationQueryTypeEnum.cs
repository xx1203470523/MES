using System.ComponentModel;

namespace Hymson.MES.Core.Enums.Manufacture
{
    public enum SfcCirculationQueryTypeEnum : sbyte
    {
        /// <summary>
        /// 工单号
        /// </summary>
        [Description("工单号")]
        OrderCode = 1,
        /// <summary>
        /// PackC条码
        /// </summary>
        [Description("Pack条码")]
        PackCode = 2,
    }
}
