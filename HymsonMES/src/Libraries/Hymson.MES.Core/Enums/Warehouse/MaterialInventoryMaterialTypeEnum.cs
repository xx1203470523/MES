using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Core.Enums.Warehouse
{
    /// <summary>
    /// 物料类型
    /// </summary>
    public  enum MaterialInventoryMaterialTypeEnum : sbyte
    {
        /// <summary>
        /// 自制件
        /// </summary>
        [Description("自制件")]
        SelfMadeParts = 1,
        /// <summary>
        /// 采购件
        /// </summary>
        [Description("采购件")]
        PurchaseParts = 2
    }
}
