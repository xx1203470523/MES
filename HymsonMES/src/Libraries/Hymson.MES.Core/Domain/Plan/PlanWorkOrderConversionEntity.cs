using Hymson.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Core.Domain.Plan;

/// <summary>
/// 工单关联转换系数
/// </summary>
public class PlanWorkOrderConversionEntity : BaseEntity
{
    /// <summary>
    /// 工单Id
    /// </summary>
    public string PlanWorkOrderId { get; set; }

    /// <summary>
    /// 模组转换系数
    /// </summary>
    public decimal ModuleConversion { get; set; }

    /// <summary>
    /// 模组转换系数
    /// </summary>
    public decimal PackConversion { get; set; }
}
