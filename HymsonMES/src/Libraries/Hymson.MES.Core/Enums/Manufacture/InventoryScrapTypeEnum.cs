using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Core.Enums.Manufacture
{
    /// <summary>
    /// 库存报废类型
    /// </summary>
    public enum InventoryScrapTypeEnum : sbyte
    {
        /// <summary>
        /// 来料不良
        /// </summary>
        [Description("来料不良")]
        IncomingDefective = 1,
        /// <summary>
        /// 作业不良
        /// </summary>
        [Description("作业不良")]
        TaskDefective = 2,
        /// <summary>
        /// 制程不良
        /// </summary>
        [Description("制程不良")]
        ProcessDefective = 3
    }
}
