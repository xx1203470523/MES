using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Core.Enums.Plan
{
    /// <summary>
    /// 班制类型
    /// </summary>
    public enum InteShiftTypeEnum : sbyte
    {

        /// <summary>
        /// 早班 
        /// </summary> 
        [Description("早班")]
        Eshift = 1,
        /// <summary>
        /// 中班
        /// </summary>
        [Description("中班")]
        Mshift = 2,
        /// <summary>
        /// 中班
        /// </summary>
        [Description("晚班")]
        Nshift = 3,
    }

    /// <summary>
    /// 修改类型
    /// </summary>
    public enum InteShiftModifyTypeEnum : sbyte
    {
        [Description("修改")]
        modify = 1,
        /// <summary>
        /// 中班
        /// </summary>
        [Description("新建")]
        create = 2,
        /// <summary>
    }
}
