using Hymson.MES.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Services.Dtos.Report;

public class ProductionDetailsReportDto 
{
    /// <summary>
    /// 产品条码
    /// </summary>
    public string? SFC { get; set; }

    /// <summary>
    /// 设备编码
    /// </summary>
    public string? EquipmentCode { get; set; }

    /// <summary>
    /// 设备名称
    /// </summary>
    public string? EquipmentName { get; set; }

    /// <summary>
    /// 工序编码
    /// </summary>
    public string? ProcedureCode { get; set; }

    /// <summary>
    /// 工序名称
    /// </summary>
    public string? ProcedureName { get; set; }

    /// <summary>
    /// 资源编码
    /// </summary>
    public string? ResourceCode { get; set; }

    /// <summary>
    /// 资源名称
    /// </summary>
    public string? ResourceName { get; set; }

    /// <summary>
    /// 是否合格状态
    /// </summary>
    public TrueOrFalseEnum? QualityStatus { get; set; }

    /// <summary>
    /// 进站时间
    /// </summary>
    public string? BeginTime { get; set; }

    /// <summary>
    /// 过站时间
    /// </summary>
    public string? EndTime { get; set; }

}