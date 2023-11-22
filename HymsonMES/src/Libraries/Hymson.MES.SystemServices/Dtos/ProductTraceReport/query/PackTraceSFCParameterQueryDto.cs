﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.SystemServices.Dtos.ProductTraceReport.Query;

/// <summary>
/// Pack追溯电芯码查询参数
/// </summary>
public class PackTraceSFCParameterQueryDto
{
    /// <summary>
    /// 查询PACK码
    /// </summary>
    public IEnumerable<string>? SFC { get; set; }
}
