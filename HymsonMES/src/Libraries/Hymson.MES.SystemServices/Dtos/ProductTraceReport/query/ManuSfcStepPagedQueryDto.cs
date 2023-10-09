using Hymson.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.SystemServices.Dtos.ProductTraceReport.query;

/// <summary>
/// 条码履历
/// </summary>
public class ManuSfcStepPagedQueryDto : PagerInfo
{
    public string SFC { get; set; }
}