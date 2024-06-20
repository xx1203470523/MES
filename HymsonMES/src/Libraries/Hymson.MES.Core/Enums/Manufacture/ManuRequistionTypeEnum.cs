using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Core.Domain.Manufacture
{
    //领料类型
    public enum ManuRequistionTypeEnum : sbyte
    {
        /// <summary>
        /// ERP
        /// </summary>
        [Description("ERP")]
        ERP = 1
    }
}
