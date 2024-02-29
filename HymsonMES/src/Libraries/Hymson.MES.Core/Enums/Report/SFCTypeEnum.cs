using Hymson.MES.Core.Enums.Manufacture;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Core.Enums.Report;

/// <summary>
/// 条码状态
/// </summary>
public enum SFCTypeEnum : sbyte
{
    [Description("电芯")]
    Cell = 0,

    [Description("模组")]
    Module = 1,

    [Description("Pack")]
    Pack = 2,
}
