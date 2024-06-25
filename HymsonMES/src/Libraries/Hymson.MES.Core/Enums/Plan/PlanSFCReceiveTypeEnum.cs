using System.ComponentModel;

namespace Hymson.MES.Core.Enums
{
    /// <summary>
    /// 条码接收类型
    /// </summary>
    public enum PlanSFCReceiveTypeEnum : sbyte
    {
        /// <summary>
        /// 产品序列码
        /// </summary>
        [Description("产品序列码")]
        MaterialSfc = 1,
        /// <summary>
        /// 供应商条码
        /// </summary>
        [Description("供应商条码")]
        SupplierSfc = 2,
    }
}
