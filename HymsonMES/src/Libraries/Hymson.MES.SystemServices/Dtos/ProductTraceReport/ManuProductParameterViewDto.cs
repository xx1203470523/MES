using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.SystemServices.Dtos.ProductTraceReport;

/// <summary>
/// 获取参数信息
/// </summary>
public record ManuProductParameterViewDto : BaseEntityDto
{
    /// <summary>
    /// 站点ID
    /// </summary>
    public long SiteId { get; set; }
    /// <summary>
    /// 工序ID
    /// </summary>
    public long ProcedureId { get; set; }
    /// <summary>
    /// 工序编码
    /// </summary>
    public string ProcedureCode { get; set; }
    /// <summary>
    /// 工序名称
    /// </summary>
    public string ProcedureName { get; set; }
    /// <summary>
    /// 资源ID
    /// </summary>
    public string ResourceId { get; set; }
    /// <summary>
    /// 资源编码
    /// </summary>
    public string ResourceCode { get; set; }
    /// <summary>
    /// 资源名称
    /// </summary>
    public string ResourceName { get; set; }
    /// <summary>
    /// 设备ID
    /// </summary>
    public long EquipmentId { get; set; }
    /// <summary>
    /// 设备编码
    /// </summary>
    public string EquipmentCode { get; set; }
    /// <summary>
    /// 设备名称
    /// </summary>
    public string EquipmentName { get; set; }
    /// <summary>
    /// 条码
    /// </summary>
    public string SFC { get; set; }
    /// <summary>
    /// 工单ID
    /// </summary>
    public long WorkOrderId { get; set; }
    /// <summary>
    /// 产品ID
    /// </summary>
    public long ProductId { get; set; }
    /// <summary>
    /// 设备本地时间
    /// </summary>
    public DateTime LocalTime { get; set; }
    /// <summary>
    /// 参数编码
    /// </summary>
    public string ParameterCode { get; set; }
    /// <summary>
    /// 参数名称
    /// </summary>
    public string ParameterName { get; set; }
    /// <summary>
    /// 参数值
    /// </summary>
    public string ParameterValue { get; set; }
    /// <summary>
    /// 参数单位
    /// </summary>
    public string ParameterUnit { get; set; }
    /// <summary>
    /// 步骤ID，出站步骤ID
    /// </summary>
    public long StepId { get; set; }
    /// <summary>
    /// 参数类型
    /// </summary>
    public ParameterTypeEnum ParameterType { get; set; }
    public string CreatedBy { get; set; }
    public DateTime CreatedOn { get; set; }

    /// <summary>
    /// 上限
    /// </summary>
    public string StandardUpperLimit { get; set; }

    /// <summary>
    /// 下限
    /// </summary>
    public string StandardLowerLimit { get; set; }

    /// <summary>
    /// 判断结果
    /// </summary>
    public string JudgmentResult { get; set; }
}
