using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Core.Enums.Process
{
    /// <summary>
    /// 配方操作类型
    /// </summary>
    public enum FormulaOperationTypeEnum
    {
        /// <summary>
        /// 物料
        /// </summary>
        [Description("物料")]
        Material = 1,
        /// <summary>
        /// 物料组
        /// </summary>
        [Description("物料组")]
        MaterialGroup = 2,
        /// <summary>
        /// 固定值
        /// </summary>
        [Description("固定值")]
        FixedValue = 3,
        /// <summary>
        /// 设定值
        /// </summary>
        [Description("设定值")]
        SetValue = 4,
        /// <summary>
        /// 参数值
        /// </summary>
        [Description("参数值")]
        ParameterValue = 5,
    }
}
