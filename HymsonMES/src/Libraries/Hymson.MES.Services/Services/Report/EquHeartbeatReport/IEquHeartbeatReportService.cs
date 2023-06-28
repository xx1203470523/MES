using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Services.Services.Report.EquHeartbeatReport
{
    public interface IEquHeartbeatReportService
    {
        /// <summary>
        /// 根据查询条件获取设备心跳状态报表分页数据
        /// </summary>
        /// <param name="pageQuery"></param>
        /// <returns></returns>
        Task<PagedInfo<EquHeartbeatReportViewDto>> GetEquHeartbeatReportPageListAsync(EquHeartbeatReportPagedQueryDto pageQuery);
    }
}
