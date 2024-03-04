using Hymson.MES.Core.Enums.Report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Services.Dtos.SystemApi;

public class OneQualifiedMonthViewDto
{
    /// <summary>
    /// 日期
    /// </summary>
    public string DayTime { get; set; }

    /// <summary>
    /// cell,module,pack
    /// </summary>
    public string Type { get; set; }

    /// <summary>
    /// 日一次合格率
    /// </summary>
    public decimal? OneQualifiedRate { get; set; }
}

public class OneQualifiedMonthQueryDto
{
    /// <summary>
    /// 风冷/液冷
    /// </summary>
    public string? ProductName{ get; set; }

    /// <summary>
    /// 风冷/液冷（分别对应不同产品）
    /// </summary>
    public string? Type { get; set; }

    /// <summary>
    /// 日/月
    /// </summary>
    public DateTypeEnum? DateType { get; set; }
}