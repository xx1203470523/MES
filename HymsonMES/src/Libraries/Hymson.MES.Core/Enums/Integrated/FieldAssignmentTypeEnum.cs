using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Core.Enums.Integrated
{
    /// <summary>
    /// 字段分配类型 1、装配，2、不合格，3、包装
    /// </summary>
    public enum FieldAssignmentTypeEnum : sbyte
    {
        /// <summary>
        /// 装配
        /// </summary>
        [Description("装配")]
        Assembling = 1,
        /// <summary>
        /// 不合格
        /// </summary>
        [Description("不合格")]
        Unqualified = 2,
        /// <summary>
        /// 包装
        /// </summary>
        [Description("包装")]
        Packing = 3
    }
}
