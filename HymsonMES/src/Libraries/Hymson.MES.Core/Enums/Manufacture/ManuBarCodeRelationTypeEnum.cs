using System.ComponentModel;

namespace Hymson.MES.Core.Enums.Manufacture
{
    /// <summary>
    /// 条码关系类型
    /// </summary>
    public enum ManuBarCodeRelationTypeEnum : sbyte
    {
        /// <summary>
        /// 条码绑定
        /// </summary>
        [Description("条码绑定")]
        SFC_Bind = 1,
        /// <summary>
        /// 条码消耗
        /// </summary>
        [Description("条码消耗")]
        SFC_Consumption = 2,
        /// <summary>
        /// 条码转换
        /// </summary>
        [Description("条码转换")]
        SFC_Conversion = 3,
        /// <summary>
        /// 条码合并
        /// </summary>
        [Description("条码合并")]
        SFC_Combined = 4,
        /// <summary>
        /// 条码拆分
        /// </summary>
        [Description("条码拆分")]
        SFC_Split = 5,
        /// <summary>
        /// 批次合并
        /// </summary>
        [Description("批次合并")]
        Batch_Combined = 6,
        /// <summary>
        /// 批次拆分
        /// </summary>
        [Description("批次拆分")]
        Batch_Split = 7
    }
}
