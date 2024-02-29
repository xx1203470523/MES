using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Core.Enums.Report;

public enum DateTypeEnum : sbyte
{
    /// <summary>
    /// 活动
    /// </summary>
    [Description("日")]
    Day = 0,

    /// <summary>
    /// 活动
    /// </summary>
    [Description("周")]
    Week = 1,

    /// <summary>
    /// 活动
    /// </summary>
    [Description("月")]
    Month = 2,

    /// <summary>
    /// 活动
    /// </summary>
    [Description("季")]
    Season = 3,

    /// <summary>
    /// 活动
    /// </summary>
    [Description("年")]
    Year = 4,
}
