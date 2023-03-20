using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Core.Enums
{
    public enum YesOrNoEnum : sbyte
    {
        /// <summary>
        /// 是
        /// </summary>
        [Description("是")]
        No = 1,
        /// <summary>
        /// 否
        /// </summary>
        [Description("否")]
        Yes = 2,
    }
}
