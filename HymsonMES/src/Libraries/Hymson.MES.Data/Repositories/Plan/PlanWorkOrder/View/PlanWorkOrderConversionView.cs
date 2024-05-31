using Hymson.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Data.Repositories.Plan.PlanWorkOrder.View;

public class PlanWorkOrderConversionView : BaseEntity
{
    /// <summary>
    /// 工单Id
    /// </summary>
    public long PlanWorkOrderId { get; set; }

    /// <summary>
    /// 模组转换系数
    /// </summary>
    public decimal ModuleConversion { get; set; }

    /// <summary>
    /// 模组转换系数
    /// </summary>
    public decimal PackConversion { get; set; }
}
