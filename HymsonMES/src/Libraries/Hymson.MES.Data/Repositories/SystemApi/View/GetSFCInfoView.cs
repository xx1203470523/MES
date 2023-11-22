using Hymson.MES.Data.Repositories.Manufacture.ManuSfcCirculation.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Data.Repositories.SystemApi;
public record GetSFCInfoView
{
    /// <summary>
    /// 条码
    /// </summary>
    public string? SFC { get; set; }

    /// <summary>
    /// 条码
    /// </summary>
    public IEnumerable<ProcductTraceView>? ProductTrace { get; set; }

    /// <summary>
    /// 条码
    /// </summary>
    public IEnumerable<SFCStepView>? SfcStep { get; set; }

    /// <summary>
    /// 条码
    /// </summary>
    public IEnumerable<ProductParameterView>? ProductParameter { get; set; }
}

/// <summary>
/// 条码绑定信息
/// </summary>
public class ProcductTraceView
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
    public long? EquipmentCode { get; set; }

    /// <summary>
    /// 绑定条码
    /// </summary>
    public long? CirculationBarCode { get; set; }
}

/// <summary>
/// 条码步骤信息
/// </summary>
public class SFCStepView
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
    public byte? ProcedureType { get; set; }

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
    public string? WorkOrderType { get; set; }

    /// <summary>
    /// 过站时间
    /// </summary>
    public DateTime? CreateOn { get; set; }

    /// <summary>
    /// 是否合格
    /// </summary>
    public bool? Passed { get; set; }
}


public class ProductParameterView
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
    public string StandardLowerLimit { get; set; }

    /// <summary>
    /// 参数上限
    /// </summary>
    public string? StandardUpperLimit { get; set; }

    /// <summary>
    /// 参数上限
    /// </summary>
    public DateTime? LocalTime{ get; set; }

}