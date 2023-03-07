﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Core.Enums.Integrated
{
    /// <summary>
    /// 工作中心类型
    /// </summary>
    public enum WorkCenterType : short
    {
        /// <summary>
        /// 工厂
        /// </summary>
        [Description("工厂")]
        Factory = 0,

        /// <summary>
        /// 产线
        /// </summary>
        [Description("产线")]
        Line = 1,

        /// <summary>
        /// 车间
        /// </summary>
        [Description("车间")]
        Farm = 2
    }
}
