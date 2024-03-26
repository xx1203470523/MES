using Hymson.Infrastructure;
using Hymson.MES.Core.Enums.Manufacture;
using OfficeOpenXml.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Services.Dtos.Report;

/// <summary>
/// 查询参数
/// </summary>
public class PackBindOtherQueryDto : QueryDtoAbstraction
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

/// <summary>
/// Pack绑定外箱码
/// </summary>
public record PackBindOtherReportExcelDto : BaseExcelDto
{
    /// <summary>
    /// 绑定类型
    /// </summary>
    [EpplusTableColumn(Header = "绑定类型", Order = 1)]
    public string? CirculationTypeName { get; set; }

    /// <summary>
    /// 条码
    /// </summary>
    [EpplusTableColumn(Header = "门箱包条码", Order = 2)]
    public string? SFC { get; set; }

    /// <summary>
    /// 绑定条码
    /// </summary>
    [EpplusTableColumn(Header = "系统条码", Order = 3)]
    public string? BindSfc { get; set; }

    /// <summary>
    /// 绑定人
    /// </summary>
    [EpplusTableColumn(Header = "绑定人", Order = 4)]
    public string? CreatedBy { get; set; }

    /// <summary>
    /// 绑定时间
    /// </summary>
    [EpplusTableColumn(Header = "绑定时间", Order = 5)]
    public DateTime? CreatedOn { get; set; }
}
