using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Core.Enums.Integrated
{
    /// <summary>
    /// 字段定义来源 物料条码，供应商，客户，产品序列码
    /// </summary>
    public enum FieldDefinitionSourceEnum : sbyte
    {
        /// <summary>
        /// 物料条码
        /// </summary>
        [Description("物料条码")]
        MaterialBarcode = 1,
        /// <summary>
        /// 供应商
        /// </summary>
        [Description("供应商")]
        Supplier = 2,
        /// <summary>
        /// 客户
        /// </summary>
        [Description("客户")]
        Customer = 3,
        /// <summary>
        /// 产品序列码
        /// </summary>
        [Description("产品序列码")]
        ProductSerialCode = 4
    }
}
