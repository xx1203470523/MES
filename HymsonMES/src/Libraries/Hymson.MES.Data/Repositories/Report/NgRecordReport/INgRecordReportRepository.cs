using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Data.Repositories.Manufacture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Data.Repositories.Report;

public interface INgRecordReportRepository
{

    /// <summary>
    /// 联表分页查询
    /// </summary>
    /// <param name="manuSfcStepNgPagedQuery"></param>
    /// <returns></returns>
    Task<PagedInfo<NgRecordReportView>> GetJoinPagedInfoAsync(NgRecordReportPageQuery manuSfcStepNgPagedQuery);

    /// <summary>
    /// 列表数据查询
    /// </summary>
    /// <param name="query"></param>
    /// <returns></returns>
    Task<IEnumerable<NgRecordReportView>> GetJoinListAsync(NgRecordReportQuery query);
}

