using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Core.Enums.Manufacture
{
    /// <summary>
    /// manu_product_bad_record 产品不良录入表Source
    /// </summary>
    public enum ProductBadRecordSourceEnum : sbyte
    {
        /// <summary>
        /// 设备复投不良
        /// </summary>
        [Description("设备复投不良")]
        EquipmentReBad = 1,
        /// <summary>
        /// 人工录入不良
        /// </summary>
        [Description("人工录入不良")]
        BadManualEntry = 2,
    }
}
