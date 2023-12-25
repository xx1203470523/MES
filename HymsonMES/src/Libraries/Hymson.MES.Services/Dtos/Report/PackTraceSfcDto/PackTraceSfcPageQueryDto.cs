using Hymson.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Services.Dtos.Report.PackTraceSfcDto;

/// <summary>
/// Pack追溯电芯码查询Dto
/// </summary>
public class PackTraceSfcQueryDto 
{
    /// <summary>
    /// 查询条码
    /// </summary>
    public string? SFC { get; set; }

    /// <summary>
    /// 查询条码
    /// </summary>
    public IEnumerable<string>? SFCs { get; set; }

    /// <summary>
    /// 起止时间
    /// </summary>
    public DateTime[]? DateList { get; set; }

    /// <summary>
    /// 起止时间
    /// </summary>
    public DateTime? BeginTime { get; set; }

    /// <summary>
    /// 截止时间
    /// </summary>
    public DateTime? EndTime { get; set; }
}

/// <summary>
/// Pack追溯电芯码查询Dto
/// </summary>
public class PackTraceSfcPageQueryDto : PagerInfo
{
    /// <summary>
    /// 查询条码
    /// </summary>
    public string? SFC { get; set; }

    /// <summary>
    /// 查询条码
    /// </summary>
    public IEnumerable<string>? SFCs { get; set; }

    /// <summary>
    /// 起止时间
    /// </summary>
    public DateTime[]? DateList { get; set; }

    /// <summary>
    /// 起止时间
    /// </summary>
    public DateTime? BeginTime { get; set; }

    /// <summary>
    /// 截止时间
    /// </summary>
    public DateTime? EndTime { get; set; }
}
