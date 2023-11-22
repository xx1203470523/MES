using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Integrated;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.SystemServices.Dtos.Api;

/// <summary>
/// 条码信息
/// </summary>
public record GetSFCInfoViewDto
{
    /// <summary>
    /// 条码
    /// </summary>
    public string? SFC { get; set; }

    /// <summary>
    /// 条码
    /// </summary>
    public IEnumerable<ProcductTraceViewDto>? ProductTrace { get; set; }

    /// <summary>
    /// 条码
    /// </summary>
    public IEnumerable<SFCStepViewDto>? SfcStep { get; set; }

    /// <summary>
    /// 条码
    /// </summary>
    public IEnumerable<ProductParameterViewDto>? ProductParameter { get; set; }
}

/// <summary>
/// 条码绑定信息
/// </summary>
public class ProcductTraceViewDto
{
    /// <summary>
    /// 条码
    /// </summary>
    public string? SFC { get; set; }

    /// <summary>
    /// 层级
    /// </summary>
    public long? Level { get; set; }

    /// <summary>
    /// 设备编码
    /// </summary>
    public string? EquipmentCode { get; set; }

    /// <summary>
    /// 绑定条码
    /// </summary>
    public string? CirculationBarCode { get; set; }
}

/// <summary>
/// 条码步骤信息
/// </summary>
public class SFCStepViewDto
{
    /// <summary>
    /// 工序编码
    /// </summary>
    public string? ProcedureCode { get; set; }

    /// <summary>
    /// 工序名称
    /// </summary>
    public string? ProcedureName { get; set; }

    /// <summary>
    /// 工序类型
    /// </summary>
    public ProcedureTypeEnum? ProcedureType { get; set; }

    /// <summary>
    /// 产品名称
    /// </summary>
    public string? ProductName { get; set; }

    /// <summary>
    /// 资源名称
    /// </summary>
    public string? ResourceName { get; set; }

    /// <summary>
    /// 工单类型
    /// </summary>
    public PlanWorkOrderTypeEnum? WorkOrderType { get; set; }

    /// <summary>
    /// 过站时间    
    /// </summary>
    public DateTime? CreateOn { get; set; }

    /// <summary>
    /// 是否合格
    /// </summary>
    public int? Passed { get; set; }
}

/// <summary>
/// 产品采集参数
/// </summary>
public class ProductParameterViewDto
{
    /// <summary>
    /// 工序编码
    /// </summary>
    public string? ProcedureCode { get; set; }

    /// <summary>
    /// 工序名称
    /// </summary>
    public string? ProcedureName { get; set; }

    /// <summary>
    /// 设备名称
    /// </summary>
    public string? EquipmentName { get; set; }

    /// <summary>
    /// 参数编码
    /// </summary>
    public string? ParameterCode { get; set; }

    /// <summary>
    /// 参数名称
    /// </summary>
    public string? ParameterName { get; set; }

    /// <summary>
    /// 参数值
    /// </summary>
    public string? ParameterValue { get; set; }

    /// <summary>
    /// 参数结果
    /// </summary>
    public string? JudgmentResult { get; set; }

    /// <summary>
    /// 参数下限
    /// </summary>
    public string? StandardLowerLimit { get; set; }

    /// <summary>
    /// 参数上限
    /// </summary>
    public string? StandardUpperLimit { get; set; }

    /// <summary>
    /// 参数上限
    /// </summary>
    public DateTime? LocalTime { get; set; }

}