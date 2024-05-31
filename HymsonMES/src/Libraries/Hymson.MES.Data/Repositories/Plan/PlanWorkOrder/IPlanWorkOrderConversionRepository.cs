using Hymson.MES.Data.Repositories.Plan.PlanWorkOrder.Command;
using Hymson.MES.Data.Repositories.Plan.PlanWorkOrder.Query;
using Hymson.MES.Data.Repositories.Plan.PlanWorkOrder.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Data.Repositories.Plan;

public interface IPlanWorkOrderConversionRepository
{
    /// <summary>
    /// 新增工单转换系数
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    Task<int> InsertAsync(PlanWorkOrderConversionCreateCommand command);

    /// <summary>
    /// 更新工单转换系数
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    Task<int> InsertOrUpdateAsync(PlanWorkOrderConversionUpdateCommand command);

    /// <summary>
    /// 获取列表
    /// </summary>
    /// <param name="query"></param>
    /// <returns></returns>
    Task<IEnumerable<PlanWorkOrderConversionView>> GetListAsync(PlanWorkOrderConversionQuery query);

    /// <summary>
    /// 获取一条数据
    /// </summary>
    /// <param name="query"></param>
    /// <returns></returns>
    Task<PlanWorkOrderConversionView> GetOneAsync(PlanWorkOrderConversionQuery query);
}