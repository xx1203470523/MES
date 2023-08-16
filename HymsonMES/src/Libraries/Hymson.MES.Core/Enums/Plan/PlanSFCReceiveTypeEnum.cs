using System.ComponentModel;

namespace Hymson.MES.Core.Enums
{
    /// <summary>
    /// 条码接收类型
    /// </summary>
    public enum PlanSFCReceiveTypeEnum : sbyte
    {
        /// <summary>
        /// 物料条码
        /// </summary>
        [Description("物料条码")]
        MaterialSfc = 1,
        /// <summary>
        /// 供应商条码
        /// </summary>
        [Description("供应商条码")]
        SupplierSfc = 2,
    }
}
