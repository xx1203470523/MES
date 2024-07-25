using System.ComponentModel;

namespace Hymson.MES.Core.Enums.Integrated
{
    /// <summary>
    /// 编码规则-编码类型
    /// </summary>
    public enum CodeRuleCodeTypeEnum : sbyte
    {
        /// <summary>
        /// 过程控制序列码
        /// </summary>
        [Description("过程控制序列码")]
        ProcessControlSeqCode = 1,
        /// <summary>
        /// 包装序列码
        /// </summary>
        [Description("包装序列码")]
        PackagingSeqCode = 2,

        /// <summary>
        /// IQC
        /// </summary>
        [Description("IQC")]
        IQC = 3,

        /// <summary>
        /// OQC
        /// </summary>
        [Description("OQC")]
        OQC = 4,

        /// <summary>
        /// FQC
        /// </summary>
        [Description("FQC")]
        FQC = 5,

        /// <summary>
        /// FQC
        /// </summary>
        [Description("环境检测")]
        Environment = 6,

        /// <summary>
        /// 物料拆分
        /// </summary>
        [Description("物料拆分")]
        WhSfcSplitAdjust = 7,

        /// <summary>
        /// 物料拆分
        /// </summary>
        [Description("物料合并")]
        WhSfcMergeAdjust = 8,

        /// <summary>
        /// 设备点检
        /// </summary>
        [Description("设备点检")]
        Spotcheck = 9,

        /// <summary>
        /// 设备保养
        /// </summary>
        [Description("设备保养")]
        Maintain = 10,

        /// <summary>
        /// 车间库存
        /// </summary>
        [Description("车间库存")]
        WorkshopInventory = 11,

        /// <summary>
        /// 设备维修
        /// </summary>
        [Description("设备维修")]
        EquRepairOrder = 12,

        /// <summary>
        /// 退料单号
        /// </summary>
        [Description("退料单号")]
        MaterialReturnOrder = 13
    }
}
