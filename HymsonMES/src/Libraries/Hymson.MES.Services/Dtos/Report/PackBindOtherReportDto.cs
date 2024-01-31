using Hymson.Infrastructure;
using Hymson.MES.Core.Enums.Manufacture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Services.Dtos.Report;

/// <summary>
/// 查询参数
/// </summary>
public record PackBindOtherQueryDto 
{
    /// <summary>
    /// 主条码
    /// </summary>
    public string? SFC { get; set; }

    /// <summary>
    /// 绑定条码
    /// </summary>
    public string? BindSfc { get; set; }
}

/// <summary>
/// 查询参数
/// </summary>
public class PackBindOtherPageQueryPagedDto : PagerInfo
{
    /// <summary>
    /// 主条码
    /// </summary>
    public string? Sfc { get; set; }

    /// <summary>
    /// 绑定条码
    /// </summary>
    public string? BindSfc { get; set; }
}

/// <summary>
/// Pack绑定外箱码
/// </summary>
public record PackBindOtherReportViewDto : BaseEntityDto
{
    /// <summary>
    /// 条码
    /// </summary>
    public string? SFC { get; set; }

    /// <summary>
    /// 绑定条码
    /// </summary>
    public string? BindSfc { get; set; }

    /// <summary>
    /// 绑定类型
    /// </summary>
    public SfcCirculationTypeEnum? CirculationType { get; set; }

    /// <summary>
    /// 绑定时间
    /// </summary>
    public DateTime? CreatedOn { get; set; }

    /// <summary>
    /// 绑定人
    /// </summary>
    public string? CreatedBy { get; set; }
}
