using Hymson.Infrastructure;
using Hymson.MES.Core.Enums.Manufacture;
using Hymson.MES.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.SystemServices.Dtos.ProductTraceReport;

/// <summary>
/// 条码履历
/// </summary>
public record ManuSfcStepViewDto : BaseEntityDto
{
    /// <summary>
    /// 站点Id
    /// </summary>
    public long SiteId { get; set; }

    /// <summary>
    /// 条码
    /// </summary>
    public string SFC { get; set; }

    /// <summary>
    /// 产品信息
    /// </summary>
    public long ProductId { get; set; }

    /// <summary>
    /// 工单号
    /// </summary>
    public long WorkOrderId { get; set; }

    /// <summary>
    /// 当前数量
    /// </summary>
    public decimal Qty { get; set; }

    /// <summary>
    /// 工序id
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
    /// 步骤类型; 跟枚举的对应不上了，具体以枚举的为准
    /// </summary>
    public ManuSfcStepTypeEnum Operatetype { get; set; }

    /// <summary>
    /// 当前状态;1：排队；2：激活；3：完工；
    /// </summary>
    public SfcProduceStatusEnum CurrentStatus { get; set; }

    /// <summary>
    /// 复投次数
    /// </summary>
    public int? RepeatedCount { get; set; }

    public bool? IsRepair { get; set; }

    /// <summary>
    /// 是否合格
    /// </summary>
    public int? Passed { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string Remark { get; set; }

    /// <summary>
    /// 工单类型
    /// </summary>
    public PlanWorkOrderTypeEnum WorkOrderType { get; set; }
    /// <summary>
    /// 产品名称
    /// </summary>
    public string ProductName { get; set; }
    /// <summary>
    /// 创建人
    /// </summary>
    public string CreatedBy { get; set; }
    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreatedOn { get; set; }
}
