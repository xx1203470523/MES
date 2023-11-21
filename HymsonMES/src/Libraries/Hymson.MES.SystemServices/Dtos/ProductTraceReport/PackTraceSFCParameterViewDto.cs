using Hymson.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.SystemServices.Dtos.ProductTraceReport;

public record PackTraceSFCParameterViewDto : BaseEntityDto
{
    /// <summary>
    /// PACK模组码
    /// </summary>
    public string? Pack { get; set; }

    /// <summary>
    /// SFC电芯码
    /// </summary>
    public string? SFC { get; set; }

    /// <summary>
    /// 设备名称
    /// </summary>
    public string? EquipmentName { get; set; }

    /// <summary>
    /// 测试结果
    /// </summary>
    public string? JudgmentResult { get; set; }

    /// <summary>
    /// 检验时间
    /// </summary>
    public string? LocalTime { get; set; }

    /// <summary>
    /// 质检参数编码 
    /// </summary>
    public string? ParameterCode { get; set; }

    /// <summary>
    /// 质检参数名称 
    /// </summary>
    public string? ParameterName { get; set; }

    /// <summary>
    /// 测试值
    /// </summary>
    public string? ParameterValue { get; set; }

    /// <summary>
    /// 检测工序编码
    /// </summary>
    public string? ProcedureCode { get; set; }

    /// <summary>
    /// 检测工序名称
    /// </summary>
    public string? ProcedureName { get; set; }

    /// <summary>
    /// 参考下限
    /// </summary>
    public string? StandardLowerLimit { get; set; }

    /// <summary>
    /// 参考上限
    /// </summary>
    public string? StandardUpperLimit { get; set; }
}

