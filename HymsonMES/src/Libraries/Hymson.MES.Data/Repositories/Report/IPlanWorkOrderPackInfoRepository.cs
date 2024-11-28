using Hymson.Infrastructure;
using Hymson.MES.Data.Repositories.Report.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Data.Repositories.Report;

public interface IPlanWorkOrderPackInfoRepository
{
    /// <summary>
    /// Pack数据追溯查询
    /// </summary>
    /// <param name="manuSfcStepNgPagedQuery"></param>
    /// <returns></returns>
    Task<IEnumerable<PackTraceView>> GetTraceListAsync(PackTraceQuery query);

    /// <summary>
    /// Pack数据查询
    /// </summary>
    /// <param name="query"></param>
    /// <returns></returns>
    Task<IEnumerable<PackTestView>> GetTestListAsync(PackTestQuery query);
}
