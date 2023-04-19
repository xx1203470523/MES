using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Core.Enums.Manufacture
{
    /// <summary>
    ///在制维修按钮执行Job的返回类型
    /// </summary>
    public enum ManuFacePlateRepairButJobReturnTypeEnum : sbyte
    {
        /// <summary>
        /// 开始维修
        /// </summary>
        [Description("开始维修")]
        JobManuRepairStartService = 1,
        /// <summary>
        /// 结束维修
        /// </summary>
        [Description("结束维修")]
        JobManuCompleteService = 2,
    }
}
