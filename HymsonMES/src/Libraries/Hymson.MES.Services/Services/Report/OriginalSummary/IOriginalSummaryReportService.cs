using Hymson.MES.Services.Dtos.Report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Services.Services.Report
{
    public interface IOriginalSummaryReportService
    {
        /// <summary>
        /// 根据ID查询Bom 主物料以及组件信息详情
        /// </summary>
        /// <param name="queryDto"></param>
        /// <returns></returns>
        Task<List<OriginalSummaryReportDto>> GetOriginalSummaryAsync(OriginalSummaryQueryDto queryDto);
    }
}
